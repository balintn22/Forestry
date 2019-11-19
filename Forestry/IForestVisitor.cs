using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Forestry
{
    /// <summary>
    /// Represents the generic behavior of a forest visitor.
    /// </summary>
    public interface IForestVisitor<TNode>
    {
        /// <summary>
        /// Starting from a set of nodes perform a depth-first visit of forest nodes.
        /// </summary>
        /// <param name="startNodes"></param>
        void VisitDepthFirst(IEnumerable<TNode> startNodes, Action<TNode> action);

        /// <summary>
        /// Starting from a set of nodes perform a breadth-first visit of forest nodes.
        /// </summary>
        /// <param name="startNodes"></param>
        /// <action>Provides an action to perform on visited nodes.</action>
        void VisitBreadthFirst(IEnumerable<TNode> startNodes, Action<TNode> action);

        /// <summary>
        /// Starting from a set of nodes, perform a reverse breadth-first visit of forest nodes.
        /// Finds all nodes from a specified node set, then visit them from
        /// the deepest, coming back to the starting node(s).
        /// </summary>
        void VisitBottomUp(IEnumerable<TNode> startNodes, Action<TNode> action);


        /// <summary>Gets child nodes of a single node.</summary>
        /// <param name="node">The node whose children to get.</param>
        /// <returns>Collection of child nodes or an empty collection if there are no child nodes.</returns>
        IEnumerable<TNode> ChildrenOf(TNode node);

        /// <summary>Gets child nodes of a set of nodes.</summary>
        /// <param name="nodes">Nodes whose children to get.</param>
        /// <returns>Collection of child nodes or an empty collection if there are no child nodes.</returns>
        IEnumerable<TNode> ChildrenOf(IEnumerable<TNode> nodes);


        /// <summary>Gets the parent of a single node.</summary>
        /// <param name="node">The node whose parent to get.</param>
        /// <returns>Parent node or default(TNode) if there is no parent.</returns>
        TNode ParentOf(TNode node);

        /// <summary>Gets parent nodes of a set of nodes.</summary>
        /// <param name="nodes">Nodes whose parents to get.</param>
        /// <returns>Collection of parent nodes or an empty collection if there are no parent nodes.</returns>
        IEnumerable<TNode> ParentsOf(IEnumerable<TNode> nodes);
    }
}
