using FluentAssertions;
using Forestry;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ForestryTest
{
    [TestClass()]
    public class ForestVisitorTests
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
        public void VisitDepthFirst_ShouldWalkInCorrectOrder()
        {
            var visitedOrder = new List<string>();
            var forestNodes = CreateForestNodes();
            var target = new ForestVisitor<TestNode>(node => forestNodes.Where(n => n.ParentId == node.Id));
            target.VisitDepthFirst(
                // Start from root nodes.
                startNodes: forestNodes.Where(node => node.ParentId == null),
                // As we visit them, add their ids to the result list.
                action: visitedNode => visitedOrder.Add(visitedNode.Id));
            visitedOrder.Should().Equal(new List<string>
            {
                "1", "11", "111", "112", "113", "12", "2", "21", "22",
            });
        }

        [TestMethod()]
        public void VisitBreadthFirst_ShouldWalkInCorrectOrder()
        {
            var visitedOrder = new List<string>();
            var forestNodes = CreateForestNodes();
            var target = new ForestVisitor<TestNode>(node => forestNodes.Where(n => n.ParentId == node.Id));
            target.VisitBreadthFirst(
                // Start from root nodes.
                startNodes: forestNodes.Where(node => node.ParentId == null),
                // As we visit them, add their ids to the result list.
                action: visitedNode => visitedOrder.Add(visitedNode.Id));
            visitedOrder.Should().Equal(new List<string>
            {
                "1", "2", "11", "12", "21", "22", "111", "112", "113",
            });
        }

        [TestMethod()]
        public void VisitBottomUp_ShouldWalkInCorrectOrder()
        {
            var visitedOrder = new List<string>();
            var forestNodes = CreateForestNodes();
            var target = new ForestVisitor<TestNode>(node => forestNodes.Where(n => n.ParentId == node.Id));
            target.VisitBottomUp(
                // Start from root nodes.
                startNodes: forestNodes.Where(node => node.ParentId == null),
                // As we visit them, add their ids to the result list.
                action: visitedNode => visitedOrder.Add(visitedNode.Id));

            Assert.IsTrue(visitedOrder.IndexOf("1") > visitedOrder.IndexOf("2"));
            Assert.IsTrue(visitedOrder.IndexOf("1") > visitedOrder.IndexOf("11"));
            Assert.IsTrue(visitedOrder.IndexOf("11") > visitedOrder.IndexOf("111"));
        }
    }
}