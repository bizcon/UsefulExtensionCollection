using bizconAG.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace bizconAg.Extensions.Test
{
    public class ReflectionPoco
    {
        public int Age { get; set; }
        public string Comment { get; set; }
        public DateTime? Birthday { get; set; }
    }

    [TestCategory("Unit")]
    [TestClass]
    public class ReflectionExtensionsTest
    {
        private const string expectedEqual = "Expected equals";
        private const string expectedNoException = "Expected no exception";

        [DataTestMethod]
        public void SetPropertyValueTest()
        {
            DateTime birthday = DateTime.Now;
            ReflectionPoco poco1 = new() { Age = 1, Comment = "Comment", Birthday = birthday };

            poco1.SetPropertyValue("Age", 2);
            poco1.SetPropertyValue("Comment", "Comment2");
            poco1.SetPropertyValue("Birthday", birthday.AddDays(1));

            Assert.AreEqual(2, poco1.Age, expectedEqual);
            Assert.AreEqual("Comment2", poco1.Comment, expectedEqual);
            Assert.AreEqual(birthday.AddDays(1), poco1.Birthday, expectedEqual);

            try
            {
                poco1.SetPropertyValue("Unknown", "test");
            }
            catch (Exception e)
            {
                Assert.Fail($"{expectedNoException}, but: {e.Message}");
            }
        }

        [DataTestMethod]
        public void GetPropertyValueTest()
        {
            DateTime birthday = DateTime.Now;
            ReflectionPoco poco1 = new() { Age = 1, Comment = "Comment", Birthday = birthday };

            int age = (int)poco1.GetPropertyValue("Age");
            string comment = (string)poco1.GetPropertyValue("Comment");
            DateTime pocoBirthday = (DateTime)poco1.GetPropertyValue("Birthday");

            Assert.AreEqual(poco1.Age, age, expectedEqual);
            Assert.AreEqual(poco1.Comment, comment, expectedEqual);
            Assert.AreEqual(poco1.Birthday, pocoBirthday, expectedEqual);

            try
            {
                poco1.GetPropertyValue("Unknown");
            }
            catch (Exception e)
            {
                Assert.Fail($"{expectedNoException}, but: {e.Message}");
            }
        }
    }
}
