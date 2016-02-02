using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;

namespace ImageToASCIIConverter
{
    class Program
    {
        static ITAConverter converter;

        static void Main(string[] args)
        {
            Console.Title = "--Image to ASCII Converter--";

            // Header
            Console.WriteLine("- Image to ASCII Converter, Jonas Heinrich -");

            do
            {
                InitializeConverter();

                Stopwatch sw = new Stopwatch();
                sw.Start();
                converter.GenerateASCII(true);
                sw.Stop();
                Console.WriteLine(String.Format("\n\n-DONE IN {0} MS-", sw.ElapsedMilliseconds));

                Console.Write("\nExit? (e): ");
            } while (Console.ReadLine() != "e");
        }

        static void InitializeConverter()
        {
            converter = new ITAConverter()
            {
                ImagePath =  ValueLoader.LoadImagePath(),
                OutputPath = ValueLoader.LoadOutputPath(),
                InvertGreyScale = VerificationLoader.VerifyInvertGreyScale()
            };

            if (VerificationLoader.VerifyCompressionRate())
                converter.LoadCompressionRate(ValueLoader.LoadCompression());
            else
                converter.LoadCompressionRate(new[] { 1, 1 });

            if (VerificationLoader.VerifyDefaultSettings())
                converter.LoadDefaultCharSet();
            else
                converter.LoadCustomCharSet(ValueLoader.LoadCustomCharacterSet());
        }
    }
}
