using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageToASCIIConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToASCIIConverter.Tests
{
    [TestClass()]
    public class ITAConverterTests
    {
        [TestMethod()]
        public void GeneratePreComputedSectorsTest_GenerateDefaultCharSet()
        {
            int[][] defaultCharset = new int[][] {  new int[] {0  , 20 },
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
            int[][] generatedCharset = ITAConverter.GeneratePreComputedSectors(new char[] { '@', '%', '#', '+', '=', '-', ';', ':', ',', '\'', '.', ' ' });;

            for (int dimension0 = 0; dimension0 < defaultCharset.GetLength(0); dimension0++)
                for (int dimension1 = 0; dimension1 < defaultCharset[dimension0].Length; dimension1++)
                    if (defaultCharset[dimension0][dimension1] != generatedCharset[dimension0][dimension1])
                        Assert.Fail();
        }
    }
}