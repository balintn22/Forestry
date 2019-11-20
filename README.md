# Forestry
Implements generic support for tree- or forest-like structures.

Available as a nuget package as Forestry.

#Usage Example
Assuming you have a type to represent a tree node like this
    class TextNode
    {
        public string Id;
        public string ParentId;
    }

    TextNode[] _nodes = new TextNode[]
    {
        new TextNode{ Id = "1", ParentId = null },
        new TextNode{ Id = "11", ParentId = "1" },
        new TextNode{ Id = "12", ParentId = "1" },
        new TextNode{ Id = "2", ParentId = null },
        new TextNode{ Id = "21", ParentId = "2" },
    };
    
    IForestVisitor<TextNode> visitor = new ForestVisitor<TextNode>(node =>
        _nodes.Where(n => n.ParentId == node.Id)
    );

    var rootNodes = _nodes.Where(node => node.ParentId == null);
    Console.WriteLine("Visiting nodes depth first...");
    visitor.VisitDepthFirst(
        rootNodes,
        node => Console.WriteLine(node.Id));

    Console.WriteLine("... breadth first ...");
    visitor.VisitBreadthFirst(
        rootNodes,
        node => Console.WriteLine(node.Id));

    Console.WriteLine("... and bottom up.");
    visitor.VisitBottomUp(
        rootNodes,
        node => Console.WriteLine(node.Id));

Will print this:
Visiting nodes depth first...
1
11
12
2
21
... breadth first ...
1
2
11
12
21
... and bottom up.
21
12
11
2
1
