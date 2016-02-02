using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToASCIIConverter
{
    class ITAConverter
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
            preComputedSectors = new int[][] {          new int[] {0  , 19 },
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

        static int[][] GeneratePreComputedSectors(char[] _chars_)
        {
            int size = 255 / _chars_.Length;
            
            int[][] preComputedSectors = new int[_chars_.Length][];

            int previous = -1;
            int i;
            for (i = 0; i < _chars_.Length - 1; i++)
            {
                preComputedSectors[i][0] = previous + 1;
                preComputedSectors[i][1] = previous + size;
                previous += size + 1;
            }

            preComputedSectors[i][0] = previous + 1;
            preComputedSectors[i][1] = 255;

            return preComputedSectors;
        }
    }
}
