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
    public class ValueLoaderTests
    {
        [TestMethod()]
        public void IsIntegerTest_IntegerInput()
        {
            Assert.IsTrue(ValueLoader.IsInteger("1"));
        }

        [TestMethod()]
        public void IsIntegerTest_StringInput()
        {
            Assert.IsFalse(ValueLoader.IsInteger("This is some random string"));
        }

        [TestMethod()]
        public void IsIntegerTest_StringInput_Empty()
        {
            Assert.IsFalse(ValueLoader.IsInteger(""));
        }
    }
}