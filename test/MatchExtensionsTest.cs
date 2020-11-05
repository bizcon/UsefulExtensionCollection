using Bizcon.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UsefulExtensionCollection.Test
{
    [TestCategory("Unit")]
    [TestClass]
    public class MatchExtensionsTest
    {
        private const string expectedEqual = "Expected equal";

        [TestMethod]
        public void ReplaceMatchesGroupsTest()
        {
            string source = "abc{{keyvault:subject}}def";
            string replaceSubjectWith = "object";
            string pattern = "{{keyvault:(?<key>.*)}}";

            var regex = new Regex(pattern);
            var matches = regex.Matches(source);

            source = matches.ReplaceMatchesGroups(source, replaceSubjectWith, new string[] { "key" });

            Assert.AreEqual("abc{{keyvault:object}}def", source, expectedEqual);
        }

        [TestMethod]
        public void ReplaceMatchesTest()
        {
            string source = "abc{{keyvault:subject}}def";
            string replaceSubjectWith = "object";
            string pattern = "{{keyvault:(.*)}}";

            var regex = new Regex(pattern);
            var matches = regex.Matches(source);

            source = matches.ReplaceMatches(source, replaceSubjectWith);

            Assert.AreEqual("abcobjectdef", source, expectedEqual);
        }
    }
}
