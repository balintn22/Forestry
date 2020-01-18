# Forestry

Implements generic support for tree- or forest-like structures, using two different approaches:

First, there is an enumeration extension, to be used much like LINQ.

Second, there is a visitor approach, to invoke a method for each node in order.

Supports depth-first, breadth-first and bottom-up ordered walks.

Available as a nuget package called Forestry.


# Usage Example for the visitor approach

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

    // You create a visitor by specifying a child accessor: a method to -
    // having a node - fetch child nodes.
    IForestVisitor<TextNode> visitor = new ForestVisitor<TextNode>(node =>
        _nodes.Where(n => n.ParentId == node.Id)
    );

    // With that little help, Forestry can perform various tree or forest
    // walks of your tree.
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

will print this:

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

# Usage example for the enumerator approach

Assuming the same node collection as above, you can write
    var rootNodes = target.Where(node => node.ParentId == null);
    foreach(var node in target.DepthFirst(
            x => target.Where(other => x.Id.Equals(other.ParentId)),
            rootNodes)
        Console.WriteLine(node.Id);

will print
 1
 11
 12
 2
 21
