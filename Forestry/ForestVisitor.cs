using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Forestry
{
    /// <summary>
    /// Implements a generic forest visitor.
    /// </summary>
    /// <typeparam name="TNode"></typeparam>
    public class ForestVisitor<TNode> : IForestVisitor<TNode>
    {
        private Func<TNode, IEnumerable<TNode>> _childSelector;
        private Func<TNode, TNode> _parentSelector;

        public ForestVisitor(
            Func<TNode, IEnumerable<TNode>> childSelector,
            Func<TNode, TNode> parentSelector = null)
        {
            Guard.AgainstNull(childSelector, nameof(childSelector));
            _childSelector = childSelector;
            _parentSelector = parentSelector;
        }

        #region Visitor Methods

        /// <summary>
        /// Starting from the specified nodes perform a depth-first visit of forest nodes.
        /// </summary>
        /// <param name="startNodes"></param>
        public void VisitDepthFirst(
            IEnumerable<TNode> startNodes, Action<TNode> action)
        {
            if ((startNodes != null) && (0 < startNodes.Count()))
            {
                // Perform the action for each node
                foreach (TNode node in startNodes)
                {
                    action(node);
                    // Recurse into children
                    var childNodes = _childSelector(node);
                    VisitDepthFirst(childNodes, action);
                }
            }
        }

        /// <summary>
        /// Starting from a set of nodes perform a depth-first visit of forest nodes.
        /// </summary>
        /// <param name="startNodes"></param>
        /// <action>When specified, provides an action to perform on visited nodes.
        /// When not specified, performs the default action provided in the constructor.</action>
        public void VisitBreadthFirst(
            IEnumerable<TNode> startNodes, Action<TNode> action)
        {
            Queue<TNode> queueOfNodesToVisit = new Queue<TNode>();

            if ((startNodes != null) && (startNodes.Count() > 0))
            {
                foreach (var node in startNodes)
                    queueOfNodesToVisit.Enqueue(node);
            }

            foreach (var node in VisitBreadthFirst(queueOfNodesToVisit))
                action(node);
        }

        private IEnumerable<TNode> VisitBreadthFirst(Queue<TNode> nodesToVisit)
        {
            if ((nodesToVisit == null) || (nodesToVisit.Count() == 0))
                // Start collection is empty, return empty.
                yield break;

            while (nodesToVisit.Count > 0)
            {
                TNode node = nodesToVisit.Dequeue();
                yield return node;

                foreach (TNode childNode in _childSelector(node))
                    nodesToVisit.Enqueue(childNode);
            }
        }

        /// <summary>
        /// Find all nodes from a specified node set, then visit them from
        /// the deepest, coming back to the starting point.
        /// </summary>
        public void VisitBottomUp(IEnumerable<TNode> startNodes, Action<TNode> action)
        {
            var stackOfNodes = new ConcurrentStack<TNode>();
            VisitBreadthFirst(startNodes, node => stackOfNodes.Push(node));
            while (stackOfNodes.TryPop(out TNode node))
                action(node);
        }

        #endregion Visitor Methods


        #region Collection Methods

        /// <summary>Gets child nodes of a single node.</summary>
        /// <param name="node">The node whose children to get.</param>
        /// <returns>Collection of child nodes or an empty collection if there are no child nodes.</returns>
        public IEnumerable<TNode> ChildrenOf(TNode node)
        {
            return _childSelector(node);
        }

        /// <summary>Gets child nodes of a set of nodes.</summary>
        /// <param name="nodes">Nodes whose children to get.</param>
        /// <returns>Collection of child nodes or an empty collection if there are no child nodes.</returns>
        public IEnumerable<TNode> ChildrenOf(IEnumerable<TNode> nodes)
        {
            List<TNode> children = new List<TNode>();
            foreach (TNode node in nodes)
                children.AddRange(_childSelector(node));

            return children;
        }


        /// <summary>Gets the parent of a single node.</summary>
        /// <param name="node">The node whose parent to get.</param>
        /// <returns>Parent node or default(TNode) if there is no parent.</returns>
        public TNode ParentOf(TNode node)
        {
            Guard.AgainstNull(_parentSelector, nameof(_parentSelector));
            return _parentSelector(node);
        }

        /// <summary>Gets parent nodes of a set of nodes.</summary>
        /// <param name="nodes">Nodes whose parents to get.</param>
        /// <returns>Collection of parent nodes or an empty collection if there are no parent nodes.</returns>
        public IEnumerable<TNode> ParentsOf(IEnumerable<TNode> nodes)
        {
            Guard.AgainstNull(_parentSelector, nameof(_parentSelector));
            List<TNode> parents = new List<TNode>();
            foreach (TNode node in nodes)
            {
                TNode parent = _parentSelector(node);
                if(!IsEmpty(parent))
                    parents.Add(parent);
            }
            return parents;
        }

        #endregion Collection Methods


        #region Helpers

        private static bool IsEmpty<T>(T obj)
        {
            return EqualityComparer<T>.Default.Equals(obj, default(T));
        }

        #endregion Helpers
    }
}
