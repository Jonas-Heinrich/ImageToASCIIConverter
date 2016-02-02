using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToASCIIConverter
{
    public static class ValueLoader
    {
        //
        //  Path loading
        //

        public static string LoadImagePath()
        {
            Console.Write("Please enter the path to the image you want to convert: ");
            return Console.ReadLine();
        }

        public static string LoadOutputPath()
        {
            Console.Write("Please enter the name of the file that shall contain the output: ");
            return Console.ReadLine();
        }

        //
        //  Compression loading
        //

        public static int[] LoadCompression()
        {
            return new[] { LoadWidthCompression(), LoadHeightCompression() };
        }

        private static int LoadWidthCompression()
        {
            return GetNumericValue("Please enter how many width pixels should be compressed into one char: ");
        }

        private static int LoadHeightCompression()
        {
            return GetNumericValue("Please enter how many height pixels should be compressed into one char: ");
        }

        //
        //  Character loading
        //

        public static char[] LoadCustomCharacterSet()
        {
            Console.WriteLine("Please enter the characters you want to use (darkest -> brightest, no space): ");
            return Console.ReadLine().ToCharArray();
        }

        //
        //  Verification
        //

        public static bool IsInteger(string input)
        {
            try
            {
                Convert.ToInt32(input);
                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }

        //
        //  Other
        //

        private static int GetNumericValue(string text)
        {
            string input;
            bool fail = false;
            do
            {
                if (!fail)
                    Console.Write(text);
                else
                    Console.Write("The input is not a number, try again: ");

                fail = true;
            } while (!IsInteger(input = Console.ReadLine()));
            return Convert.ToInt32(input);
        }
    }
}
