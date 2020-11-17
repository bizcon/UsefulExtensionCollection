using Bizcon.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UsefulExtensionCollection.Test
{
    public class FilterPoco
    {
        public int? Age { get; set; }
        public int? Lenght { get; set; }
        public string Adress { get; set; }
        public string Comment { get; set; }
    }

    [TestCategory("Unit")]
    [TestClass]
    public class FilterExtensionsTest
    {
        private const string expectedTrue = "Expected true";
        private const string expectedFalse = "Expected false";

        [TestMethod]
        public void BasicTest()
        {
            FilterPoco poco1 = new FilterPoco() { Age = 1, Comment = "Comment", Lenght = 7, Adress = "Adress" };
            Assert.IsTrue(poco1.FilterApplies<FilterPoco>("Age==1"), expectedTrue);
            Assert.IsTrue(poco1.FilterApplies<FilterPoco>("Comment==\"Comment\""), expectedTrue);
            Assert.IsTrue(poco1.FilterApplies<FilterPoco>("Lenght==7"), expectedTrue);
            Assert.IsTrue(poco1.FilterApplies<FilterPoco>("Adress=\"Adress\""), expectedTrue);
            Assert.IsFalse(poco1.FilterApplies<FilterPoco>("Age==12"), expectedFalse);
            Assert.IsFalse(poco1.FilterApplies<FilterPoco>("Comment==\"Comment2\""), expectedFalse);
            Assert.IsFalse(poco1.FilterApplies<FilterPoco>("Lenght==72"), expectedFalse);
            Assert.IsFalse(poco1.FilterApplies<FilterPoco>("Adress==\"Adress2\""), expectedFalse);
        }
    }
}
