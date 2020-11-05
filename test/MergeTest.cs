using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logic.Shared.Test
{
    public class MergePoco
    {
        public MergePoco()
        {

        }
        public int? Age { get; set; }
        public int? Lenght { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }
    public class BasePoco
    {
        public BasePoco()
        {

        }
        public int? Age { get; set; }
        public int? Lenght { get; set; }
        public string Adress { get; set; }
        public string Comment { get; set; }
    }

    [TestCategory("Unit")]
    [TestClass]
    public class MergeTest
    {
        [DataTestMethod]
        public void BasicTest()
        {
            BasePoco basePoco = new BasePoco() { Age = 1, Comment = "Comment", Lenght = 7, Adress = "Adress" };
            MergePoco mergePoco = new MergePoco() { Age = 2, Name = "Name" };

            BasePoco mergedPoco = basePoco.MergeType(mergePoco);

            Assert.IsNotNull(mergedPoco, "Expected to be not null");
            Assert.AreEqual(basePoco, mergedPoco, "Expected to be equal");

            Assert.AreEqual(basePoco.Age, mergePoco.Age, "Expected to be equal");
            Assert.AreEqual(2, mergedPoco.Age, "Expected to be equal");
            Assert.AreEqual(mergedPoco.Adress, mergedPoco.Adress, "Expected to be equal");
            Assert.AreEqual("Adress", mergedPoco.Adress, "Expected to be equal");
            Assert.AreEqual(mergedPoco.Comment, basePoco.Comment, "Expected to be equal");
            Assert.AreEqual("Comment", mergedPoco.Comment, "Expected to be equal");
            Assert.IsNull(mergePoco.Lenght, "Expected to be null");
            Assert.AreEqual(7, mergedPoco.Lenght, "Expected to be equal");
        }
    }
}
