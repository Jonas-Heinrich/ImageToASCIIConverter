using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToASCIIConverter
{
    static class VerificationLoader
    {

        public static bool VerifyDefaultSettings()
        {
            Console.Write("Do you want to use the default settings? (y/n): ");
            return GetYesNo();
        }

        public static bool VerifyInvertGreyScale()
        {
            Console.Write("Do you want to invert the grayscale? (y/n): ");
            return GetYesNo();
        }

        public static bool VerifyCompressionRate()
        {
            Console.Write("Do you want to compress information? (y/n): ");
            return GetYesNo();
        }

        private static bool GetYesNo()
        {
            if (Console.ReadLine() == "y")
                return true;
            return false;
        }
    }
}
