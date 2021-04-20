using bizconAG.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using static bizconAG.Extensions.CompareExtensions;

namespace bizconAg.Extensions.Test
{
    public class ComparisonPoco
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }
        public DateTime Birthday { get; set; }
        public TimeSpan Since { get; set; }
    }
    public class ComparisonPoco2
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    [TestCategory("Unit")]
    [TestClass]
    public class CompareExtensionsTest
    {
        static ComparisonPoco p1, p2, p3, p4, p5, pNull;
        static ComparisonPoco2 p21, p22;

        private const string expectedEqual = "Expected equals";
        private const string expectedNotEqual = "Expected not equals";

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            p1 = new ComparisonPoco() { Name = "P1", Age = 3, Adress = "A1", Birthday = new DateTime(2020, 5, 9), Since = TimeSpan.FromSeconds(10) };
            p2 = new ComparisonPoco() { Name = "P2", Age = 3, Adress = "A1", Birthday = new DateTime(2020, 5, 9), Since = TimeSpan.FromSeconds(10) };
            p3 = new ComparisonPoco() { Name = "P3", Age = 5, Adress = "A2", Birthday = new DateTime(2020, 5, 9), Since = TimeSpan.FromSeconds(10) };
            p4 = new ComparisonPoco() { Name = "P4", Age = 5, Adress = "A2", Birthday = new DateTime(2020, 5, 10), Since = TimeSpan.FromSeconds(20) };
            p5 = new ComparisonPoco() { Name = "P5", Age = 7, Adress = "A7", Birthday = new DateTime(2020, 7, 7), Since = TimeSpan.FromSeconds(77) };
            pNull = new ComparisonPoco() { Name = null, Age = default, Adress = null, Birthday = default, Since = default };
            p21 = new ComparisonPoco2() { Name = "P21", Age = 33 };
            p22 = new ComparisonPoco2() { Name = "P21", Age = 3 };
        }

        [TestMethod]
        public void CompareGetObject1Test()
        {
            ComparisonPoco result = p1.CompareGetObject(p2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, p2.Name, expectedEqual);
            Assert.AreEqual(result.Age, default, expectedEqual);
            Assert.AreEqual(result.Adress, default, expectedEqual);
            Assert.AreEqual(result.Birthday, default, expectedEqual);
            Assert.AreEqual(result.Since, default, expectedEqual);
        }

        [TestMethod]
        public void CompareGetObject2Test()
        {
            ComparisonPoco result = p2.CompareGetObject(p3);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, p3.Name, expectedEqual);
            Assert.AreEqual(result.Age, p3.Age, expectedEqual);
            Assert.AreEqual(result.Adress, p3.Adress, expectedEqual);
            Assert.AreEqual(result.Birthday, default, expectedEqual);
            Assert.AreEqual(result.Since, default, expectedEqual);
        }

        [TestMethod]
        public void CompareGetObject3Test()
        {
            ComparisonPoco result = p4.CompareGetObject(p5);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, p5.Name, expectedEqual);
            Assert.AreEqual(result.Age, p5.Age, expectedEqual);
            Assert.AreEqual(result.Adress, p5.Adress, expectedEqual);
            Assert.AreEqual(result.Birthday, p5.Birthday, expectedEqual);
            Assert.AreEqual(result.Since, p5.Since, expectedEqual);
        }

        [TestMethod]
        public void CompareGetObject4Test()
        {
            ComparisonPoco result = p5.CompareGetObject(pNull);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, pNull.Name, expectedEqual);
            Assert.AreEqual(result.Age, pNull.Age, expectedEqual);
            Assert.AreEqual(result.Adress, pNull.Adress, expectedEqual);
            Assert.AreEqual(result.Birthday, pNull.Birthday, expectedEqual);
            Assert.AreEqual(result.Since, pNull.Since, expectedEqual);
        }

        [TestMethod]
        public void CompareGetObject5Test()
        {
            ComparisonPoco result = pNull.CompareGetObject(p2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Name, p2.Name, expectedEqual);
            Assert.AreEqual(result.Age, p2.Age, expectedEqual);
            Assert.AreEqual(result.Adress, p2.Adress, expectedEqual);
            Assert.AreEqual(result.Birthday, p2.Birthday, expectedEqual);
            Assert.AreEqual(result.Since, p2.Since, expectedEqual);
        }

        [TestMethod]
        public void CompareGetVariance1Test()
        {
            List<Variance> result = p1.CompareGetVariance(p2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 1, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreEqual(result[0].Object1.ToString(), p1.Name, expectedEqual);
            Assert.AreEqual(result[0].Object2.ToString(), p2.Name, expectedEqual);
        }

        [TestMethod]
        public void CompareGetVariance2Test()
        {
            List<Variance> result = p3.CompareGetVariance(p4);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 3, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[1].Object1, result[1].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[2].Object1, result[2].Object2, expectedNotEqual);
        }

        [TestMethod]
        public void CompareGetVariance3Test()
        {
            List<Variance> result = p4.CompareGetVariance(p5);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 5, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[1].Object1, result[1].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[2].Object1, result[2].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[3].Object1, result[3].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[4].Object1, result[4].Object2, expectedNotEqual);
        }

        [TestMethod]
        public void CompareGetVariance4Test()
        {
            List<Variance> result = p5.CompareGetVariance(pNull);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 5, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[1].Object1, result[1].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[2].Object1, result[2].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[3].Object1, result[3].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[4].Object1, result[4].Object2, expectedNotEqual);
        }

        [TestMethod]
        public void CompareGetVariance5Test()
        {
            List<Variance> result = pNull.CompareGetVariance(p2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 5, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[1].Object1, result[1].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[2].Object1, result[2].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[3].Object1, result[3].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[4].Object1, result[4].Object2, expectedNotEqual);
        }

        [TestMethod]
        public void CompareGetObjects1Test()
        {
            Tuple<ComparisonPoco, ComparisonPoco> result = p1.CompareGetObjects(p2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Item1.Age, default, expectedEqual);
            Assert.AreEqual(result.Item2.Age, default, expectedEqual);
            Assert.AreEqual(result.Item1.Name, p1.Name, expectedEqual);
            Assert.AreEqual(result.Item2.Name, p2.Name, expectedEqual);
        }

        [DataTestMethod]
        public void CompareGetObjects2Test()
        {
            Tuple<ComparisonPoco, ComparisonPoco> result = p4.CompareGetObjects(p5);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Item1.Age, p4.Age, expectedEqual);
            Assert.AreEqual(result.Item1.Name, p4.Name, expectedEqual);
            Assert.AreEqual(result.Item1.Adress, p4.Adress, expectedEqual);
            Assert.AreEqual(result.Item1.Birthday, p4.Birthday, expectedEqual);
            Assert.AreEqual(result.Item1.Since, p4.Since, expectedEqual);
            Assert.AreEqual(result.Item2.Age, p5.Age, expectedEqual);
            Assert.AreEqual(result.Item2.Name, p5.Name, expectedEqual);
            Assert.AreEqual(result.Item2.Adress, p5.Adress, expectedEqual);
            Assert.AreEqual(result.Item2.Birthday, p5.Birthday, expectedEqual);
            Assert.AreEqual(result.Item2.Since, p5.Since, expectedEqual);
        }

        [DataTestMethod]
        public void CompareGetObjects3Test()
        {
            Tuple<ComparisonPoco, ComparisonPoco> result = p5.CompareGetObjects(pNull);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Item1.Age, p5.Age, expectedEqual);
            Assert.AreEqual(result.Item1.Name, p5.Name, expectedEqual);
            Assert.AreEqual(result.Item1.Adress, p5.Adress, expectedEqual);
            Assert.AreEqual(result.Item1.Birthday, p5.Birthday, expectedEqual);
            Assert.AreEqual(result.Item1.Since, p5.Since, expectedEqual);
            Assert.AreEqual(result.Item2.Age, pNull.Age, expectedEqual);
            Assert.AreEqual(result.Item2.Name, pNull.Name, expectedEqual);
            Assert.AreEqual(result.Item2.Adress, pNull.Adress, expectedEqual);
            Assert.AreEqual(result.Item2.Birthday, pNull.Birthday, expectedEqual);
            Assert.AreEqual(result.Item2.Since, pNull.Since, expectedEqual);
        }

        [DataTestMethod]
        public void CompareGetObjects4Test()
        {
            Tuple<ComparisonPoco, ComparisonPoco> result = pNull.CompareGetObjects(p2);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Item1.Age, pNull.Age, expectedEqual);
            Assert.AreEqual(result.Item1.Name, pNull.Name, expectedEqual);
            Assert.AreEqual(result.Item1.Adress, pNull.Adress, expectedEqual);
            Assert.AreEqual(result.Item1.Birthday, pNull.Birthday, expectedEqual);
            Assert.AreEqual(result.Item1.Since, pNull.Since, expectedEqual);
            Assert.AreEqual(result.Item2.Age, p2.Age, expectedEqual);
            Assert.AreEqual(result.Item2.Name, p2.Name, expectedEqual);
            Assert.AreEqual(result.Item2.Adress, p2.Adress, expectedEqual);
            Assert.AreEqual(result.Item2.Birthday, p2.Birthday, expectedEqual);
            Assert.AreEqual(result.Item2.Since, p2.Since, expectedEqual);
        }

        [TestMethod]
        public void CompareGetVarianceDifferentTypes1Test()
        {
            List<Variance> result = p1.CompareGetVarianceDifferentTypes(p21);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[1].Object1, result[1].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[2].Object1, result[2].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[3].Object1, result[3].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[4].Object1, result[4].Object2, expectedNotEqual);
        }

        [TestMethod]
        public void CompareGetVarianceDifferentTypes2Test()
        {
            List<Variance> result = p21.CompareGetVarianceDifferentTypes(p1);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
            Assert.AreNotEqual(result[1].Object1, result[1].Object2, expectedNotEqual);
        }

        [TestMethod]
        public void CompareGetVarianceDifferentTypes3Test()
        {
            List<Variance> result = p22.CompareGetVarianceDifferentTypes(p1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count, expectedEqual);
            Assert.AreNotEqual(result[0].Object1, result[0].Object2, expectedNotEqual);
        }
    }
}
