using Forestry;
using System;
using System.Linq;

namespace FrameworkClient
{
    class Program
    {
        private static TextNode[] _nodes = new TextNode[]
        {
            new TextNode{ Id = "1", ParentId = null },
            new TextNode{ Id = "11", ParentId = "1" },
            new TextNode{ Id = "12", ParentId = "1" },
            new TextNode{ Id = "2", ParentId = null },
            new TextNode{ Id = "21", ParentId = "2" },
        };


        static void Main(string[] args)
        {
            IForestVisitor<TextNode> visitor = new ForestVisitor<TextNode>(node =>
                _nodes.Where(n => n.ParentId == node.Id)
            );

            visitor.VisitDepthFirst(
                _nodes.Where(node => node.ParentId == null),
                node => Console.WriteLine(node.Id));
        }
    }
}
