using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToASCIIConverter
{
    public class ITAConverter
    {
        //
        //  Properties
        //

        int[][] preComputedSectors;
        char[] chars;

        string imagePath;
        string outputPath;

        int widthCompression;
        int heightCompression;

        bool invert;

        public string ImagePath
        {
            set
            {
                imagePath = value;
            }
        }

        public string OutputPath
        {
            set
            {
                outputPath = value;
            }
        }

        public bool InvertGreyScale
        {
            set
            {
                invert = value;
            }
        }

        public int WidthCompressionRate
        {
            set
            {
                widthCompression = value;
            }
        }

        public int HeightCompressionRate
        {
            set
            {
                heightCompression = value;
            }
        }

        //
        //  Generating Methods
        //

        public void GenerateASCII(bool workingState)
        {
            if(workingState)
                Console.Write("WORKING");

            Bitmap b = LoadImage();

            using (StreamWriter sw = new StreamWriter(outputPath))
            {
                for (int pixelY = 0; pixelY < b.Height; pixelY++)
                {
                    string lineResult = "";
                    for (int pixelX = 0; pixelX < b.Width; pixelX++)
                    {
                        Color pixelColor = b.GetPixel(pixelX, pixelY);
                        int grey = (int)(pixelColor.R * 0.3 + pixelColor.G * 0.59 + pixelColor.B * 0.11);

                        if (invert)
                            grey = 255 - grey;

                        for (int i = 0; i < preComputedSectors.GetLength(0); i++)
                        {
                            if (grey >= preComputedSectors[i][0] && grey <= preComputedSectors[i][1])
                            {
                                lineResult += chars[i];
                                break;
                            }
                        }
                    }
                    sw.Write(lineResult + "\n");
                    if (workingState && pixelY % 100 == 0)
                        Console.Write(".");
                }
            }
        }

        private Bitmap LoadImage()
        {
            Bitmap b;
            if (widthCompression == 1 && heightCompression == 1)
            {
                b = new Bitmap(imagePath);
            }
            else if (widthCompression > 1 && heightCompression > 1)
            {
                Bitmap original = new Bitmap(imagePath);
                Size s = new Size(original.Width / widthCompression, original.Height / heightCompression);
                b = new Bitmap(original, s);
            }
            else
            {
                throw new ArgumentException("The compression rate is to small!");
            }
            return b;
        }

        //
        //  Property Methods
        //

        public void LoadDefaultCharSet()
        {
            chars = new char[] { '@', '%', '#', '+', '=', '-', ';', ':', ',', '\'', '.', ' ' };
            preComputedSectors = new int[][] {  new int[] {0  , 20 },
                                                new int[] {21 , 41 },
                                                new int[] {42 , 62 },
                                                new int[] {63 , 83 },
                                                new int[] {84 , 104},
                                                new int[] {105, 125},
                                                new int[] {126, 146},
                                                new int[] {147, 167},
                                                new int[] {168, 188},
                                                new int[] {189, 209},
                                                new int[] {210, 230},
                                                new int[] {231, 255}};
        }

        public void LoadCompressionRate(int[] rate)
        {
            if (rate.Length != 2)
                return;

            widthCompression = rate[0];
            heightCompression = rate[1];
        }

        public void LoadCustomCharSet(char[] _chars_)
        {
            chars = _chars_;

            if (chars.Length < 1)
                return;

            preComputedSectors = GeneratePreComputedSectors(_chars_);
        }

        public static int[][] GeneratePreComputedSectors(char[] _chars_)
        {
            if (_chars_.Length < 1)
                throw new ArgumentException("The chars specified are too few.");

            int size = 255 / _chars_.Length;
            
            int[][] preComputedSectors = new int[_chars_.Length][];

            int previous = -1;
            int i;
            for (i = 0; i < _chars_.Length - 1; i++)
            {
                preComputedSectors[i] = new int[] {previous + 1, previous + size };
                previous += size;
            }

            preComputedSectors[i] = new int[] { previous + 1, 255};

            

            return preComputedSectors;
        }
}
}
