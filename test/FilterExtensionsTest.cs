using Bizcon.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        [TestMethod]
        public void ExceptionTest()
        {
            FilterPoco poco1 = new FilterPoco() { };
            Assert.IsFalse(poco1.FilterApplies<FilterPoco>("blabla"), expectedFalse);
        }

        [TestMethod]
        public void JsonElementTest()
        {
            JsonDocument jsonDocument = JsonDocument.Parse("{\"name\":\"test\"}");
            bool applies = jsonDocument.RootElement.FilterApplies<JsonElement>("name.Equals(\"test\") && \"test\" == name");
            Assert.IsTrue(applies, expectedTrue);
        }

        [TestMethod]
        public void ParameterRegexMatcherTest()
        {
            string pattern = @"\b(?<!(""|\.))(?<name>\w+)(?=(=|\.|\b))\b";
            string input = "nametest123==\"value123\" && nametest456.Equals(\"value456\")||\"value789\" == nametest789";
            MatchCollection mc = Regex.Matches(input, pattern);
            foreach (Match m in mc.Reverse())
            {
                string replacement = $"test({m.Groups[0]})";
                input = input.Remove(m.Index, m.Groups[0].Length).Insert(m.Index, replacement);
            }

            Assert.AreEqual("test(nametest123)==\"value123\" && test(nametest456).Equals(\"value456\")||\"value789\" == test(nametest789)", input, "Expected to be equals");
        }

        [TestMethod]
        public void DynamicTest()
        {
            dynamic dObject = JsonSerializer.Deserialize<ExpandoObject>("{\"name\":\"test\"}");

            bool applies = FilterExtensions.DynamicFilterApplies(dObject, "name.ToString().Equals(\"test\")");
            Assert.IsTrue(applies, expectedTrue);

        }
    }
}
