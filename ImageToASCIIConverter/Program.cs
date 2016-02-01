using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace ASCIIImageConverter
{
    class Program
    {
        static List<int[]> preComputedSectors = new List<int[]>();
        static char[] chars;

        static void Main(string[] args)
        {
            Console.Title = "--Image to ASCII Converter--";
            Console.WindowWidth = 100;
            Console.WindowHeight = 50;

            while (true)
            {
                try
                {
                    // Header
                    Console.WriteLine("- Image to ASCII Converter, Jonas Heinrich -");
                    WriteLogo();

                    // Image path
                    Console.Write("Please enter the path to the image you want to convert (relative to .exe): ");
                    string filePath = Console.ReadLine();

                    // Output path
                    Console.Write("Please enter the name of the .txt that shall contain the output: ");
                    string newFilePath = Console.ReadLine();

                    // Settings
                    Console.Write("Do you want to use the default settings? (y/n): ");
                    if (Console.ReadLine() != "y")
                        LoadCustom();
                    else
                        LoadDefault();

                    // Invert
                    Console.Write("Do you want to invert the grayscale? (y/n): ");
                    bool invert = false;
                    if (Console.ReadLine() == "y")
                        invert = true;

                    // Width compression
                    Console.Write("Do you want to compress information? (y/n): ");
                    int widthCompressionRate = 1;
                    int heightCompressionRate = 1;
                    if (Console.ReadLine() == "y")
                    {
                        widthCompressionRate = LoadCompressionWidthRate();
                        heightCompressionRate = LoadCompressionHeightRate();
                    }

                    // Generate
                    ConvertToASCII(filePath, newFilePath + ".txt", widthCompressionRate, heightCompressionRate, invert);

                    Console.WriteLine("\n--DONE--");
                    Console.ReadKey();

                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\nSomething bad happened: " + e.Message);
                    Console.ReadKey();
                }
            }
        }

        static void ConvertToASCII(string path, string outputPath, int widthCompression, int heightCompression, bool invert)
        {
            Console.Write("WORKING");

            Bitmap b;

            if (widthCompression == 1 && heightCompression == 1)
            {
                b = new Bitmap(path);
            }
            else if (widthCompression > 1 && heightCompression > 1)
            {
                Bitmap original = new Bitmap(path);
                Size s = new Size(original.Width / widthCompression, original.Height / heightCompression);
                b = new Bitmap(original, s);
            }
            else
            {
                throw new ArgumentException("The compression rate is to small!");
            }

            StreamWriter sw = new StreamWriter(outputPath);

            for (int y = 0; y < b.Height; y++)
            {
                string result = "";
                for (int x = 0; x < b.Width; x++)
                {
                    Color pixelColor = b.GetPixel(x, y);
                    int grey = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);

                    if (invert)
                        grey = 255 - grey;

                    for (int i = 0; i < preComputedSectors.Count; i++)
                    {
                        if (grey >= preComputedSectors[i][0] && grey <= preComputedSectors[i][1])
                        {
                            result += chars[i];
                            break;
                        }
                    }
                }
                sw.Write(result + "\n");
                if (y % 100 == 0)
                    Console.Write(".");
            }
            sw.Close();
        }

        //
        //  Load Compression Rates
        //

        static int LoadCompressionWidthRate()
        {
            Console.Write("Please enter how many width pixels should be compressed into one char: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        static int LoadCompressionHeightRate()
        {
            Console.Write("Please enter how many height pixels should be compressed into one char: ");
            return Convert.ToInt32(Console.ReadLine());
        }

        //
        //  Load Settings
        //

        static void LoadDefault()
        {
            chars = new char[] { '@', '%', '#', '+', '=', '-', ';', ':', ',', '\'', '.', ' ' };
            preComputedSectors = new List<int[]> {  new int[] {0  , 19 },
                                                        new int[] {20 , 49 },
                                                        new int[] {50 , 79 },
                                                        new int[] {80 , 99 },
                                                        new int[] {100, 119},
                                                        new int[] {120, 139},
                                                        new int[] {140, 159},
                                                        new int[] {160, 179},
                                                        new int[] {180, 199},
                                                        new int[] {200, 239},
                                                        new int[] {240, 255}};
        }

        static void LoadCustom()
        {

            Console.Write("Please enter the chars you want to use (darkest to brightest): ");
            chars = Console.ReadLine().ToCharArray();

            int size = 256 / chars.Length;

            int previous = -1;

            for (int i = 0; i < chars.Length - 1; i++)
            {
                preComputedSectors.Add(new int[] { previous + 1, previous + size });
                previous += size + 1;
            }

            preComputedSectors.Add(new int[] { previous + 1, 255 });

            Console.WriteLine();
        }

        static void WriteLogo()
        {
            Console.Write(
                       "%%%%%%%%%%%%%%%;.%%%%%%%%%%%%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%%%..%%%%%%%%%%%%%%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%#..%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%..#%.%%%%%%%..%%%%%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%..#%.%%%%%%.....%%%%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%;..%.%%%%%;.......%%%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%..% -.%%%%...........%%%%%%%.%%%%%%%" + "\n" +
                       "%%%%%%%...%.%%%..............%%%%%%.%%%%%%%%" + "\n" +
                       "%%%%%%%..-..%%%..........%%%.+%%%%%.%%%%%%%%" + "\n" +
                       "%%%%%% -..%..%%%%........%%%%%.%%%% -.%%%%%%" + "\n" +
                       "%%%%%%...%..%%%%.......%%%%%%%%%%%..%#%%%%%%" + "\n" +
                       "%%%%%%......%%%=.......%%%%%%%%%%#..%.%%%%%%" + "\n" +
                       "%%%%%%......%%%........%%%%%%%%%%...%.%%%%%%" + "\n" +
                       "%%%%%%......-%%........'%%%%%%%#....%.%%%%%%" + "\n" +
                       "%%%%%%.......%%.........%%%%%%.....%;.%%%%%%" + "\n" +
                       "%%%%%%........% -...................%..%%%%%" + "\n" +
                       "%%%%%%.........%..................% '..%%%%%" + "\n" +
                       "%%%%%%..........%.................%...%%%%%%" + "\n" +
                       "%%%%%%...........................%....%%%%%%" + "\n" +
                       "%%%%%%..........................%....-%%%%%%" + "\n" +
                       "%%%%%%%..............................%%%%%%%" + "\n" +
                       "%%%%%%%.............................'%%%%%%%" + "\n" +
                       "%%%%%%%%............................%%%%%%%%" + "\n" +
                       "%%%%%%%%...........................%%%%%%%%%" + "\n" +
                       "%%%%%%%%%.........................%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%.......................%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%.....................%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%%....-.............%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%%%.....% '........%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%%%% -......%%%%%%%%.%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%%%%%%%..........-%%%%%%%%%%%%%%%%" + "\n" +
                       "%%%%%%%%%%%%%%%%%%%%%##%%%%%%%%%%%%%%%%%%%%%" + "\n");
        }
    }
}
