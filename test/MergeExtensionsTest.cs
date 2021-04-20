using bizconAG.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bizconAg.Extensions.Test
{
    public class MergePoco
    {
        public int? Age { get; set; }
        public int? Lenght { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }

    public class MergeBasePoco
    {
        public int? Age { get; set; }
        public int? Lenght { get; set; }
        public string Adress { get; set; }
        public string Comment { get; set; }
    }

    [TestCategory("Unit")]
    [TestClass]
    public class MergeExtensionsTest
    {
        private const string expectedEqual = "Expected equals";
        private const string expectedNull = "Expected null";
        private const string expectedNotNull = "Expected not null";

        [DataTestMethod]
        public void BasicTest()
        {
            MergeBasePoco basePoco = new MergeBasePoco() { Age = 1, Comment = "Comment", Lenght = 7, Adress = "Adress" };
            MergePoco mergePoco = new MergePoco() { Age = 2, Name = "Name" };

            MergeBasePoco mergedPoco = basePoco.MergeType(mergePoco);

            Assert.IsNotNull(mergedPoco, expectedNotNull);
            Assert.AreEqual(basePoco, mergedPoco, expectedEqual);
            Assert.AreEqual(basePoco.Age, mergePoco.Age, expectedEqual);
            Assert.AreEqual(2, mergedPoco.Age, expectedEqual);
            Assert.AreEqual(mergedPoco.Adress, mergedPoco.Adress, expectedEqual);
            Assert.AreEqual("Adress", mergedPoco.Adress, expectedEqual);
            Assert.AreEqual(mergedPoco.Comment, basePoco.Comment, expectedEqual);
            Assert.AreEqual("Comment", mergedPoco.Comment, expectedEqual);
            Assert.IsNull(mergePoco.Lenght, expectedNull);
            Assert.AreEqual(7, mergedPoco.Lenght, expectedEqual);
        }
    }
}
