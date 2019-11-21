using FluentAssertions;
using Forestry;
using ForestryTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ForestryTest
{
    [TestClass()]
    public class IEnumerableExtensionsTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        // Create a forest like this
        //         1           2
        //         |           |
        //         |           |
        //        /|          /|
        //       / |         / |
        //      11 12       21 22
        //     /|\
        //    / | \
        //   /  |  \
        // 111 112 113
        public IEnumerable<TestNode> CreateForestNodes()
        {
            return new List<TestNode>
            {
                new TestNode(null, "1"),
                new TestNode("1",  "11"),
                new TestNode("1",  "12"),
                new TestNode("11", "111"),
                new TestNode("11", "112"),
                new TestNode("11", "113"),
                new TestNode(null, "2"),
                new TestNode("2",  "21"),
                new TestNode("2",  "22"),
            };
        }

        [TestMethod()]
        public void DepthFirst_ShouldYieldCorrectOrder()
        {
            var target = CreateForestNodes();
            var rootNodes = target.Where(node => node.ParentId == null);
            IEnumerable<string> result = target.DepthFirst(
                x => target.Where(other => x.Id.Equals(other.ParentId)),
                rootNodes)
                .Select(y => y.Id);

            result.Should().Equal(new List<string>
            {
                "1", "11", "111", "112", "113", "12", "2", "21", "22",
            });
        }

        [TestMethod()]
        public void BreadthFirst_ShouldYieldCorrectOrder()
        {
            var target = CreateForestNodes();
            var rootNodes = target.Where(node => node.ParentId == null);
            IEnumerable<string> result = target.BreadthFirst(
                x => target.Where(other => x.Id.Equals(other.ParentId)),
                rootNodes)
                .Select(y => y.Id);

            result.Should().Equal(new List<string>
            {
                "1", "2", "11", "12", "21", "22", "111", "112", "113",
            });
        }

        [TestMethod()]
        public void BottomUp_ShouldYieldCorrectOrder()
        {
            var target = CreateForestNodes();
            var rootNodes = target.Where(node => node.ParentId == null);
            IEnumerable<string> result = target.BottomUp(
                x => target.Where(other => x.Id.Equals(other.ParentId)),
                rootNodes)
                .Select(y => y.Id);

            result.Should().Equal(new List<string>
            {
                "113", "112", "111", "22", "21", "12", "11", "2", "1",
            });
        }
    }
}