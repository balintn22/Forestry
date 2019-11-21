using System;
using System.Collections.Generic;

namespace Forestry
{
    /// <summary>
    /// Implements enumerators for tree/forest walks for collections.
    /// </summary>
    public static class IEnumerableExtensions
    {
        #region Depth-first enumerations

        /// <summary>
        /// Implements a depth-first walk of a forest, from the specified node as an enumerator.
        /// </summary>
        /// <param name="self">Designates the collection of tree/forest nodes to enumerate.</param>
        /// <param name="childSelector">Provides a function, that given a node, returns it children.</param>
        /// <param name="startNode">Specifies the node to begin the walk from.</param>
        /// <returns></returns>
        public static IEnumerable<TNode> DepthFirst<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            TNode startNode)
        {
            Guard.AgainstNull(childSelector, nameof(childSelector));

            if (IsEmpty(startNode))
                yield break;

            yield return startNode;

            var childNodes = childSelector(startNode);
            if (childNodes == null)
                yield break;

            foreach (TNode childNode in childNodes)
            {
                foreach (TNode node in DepthFirst(self, childSelector, childNode))
                    yield return node;
            }
        }

        /// <summary>
        /// Implements a depth-first walk of a forest, from a set of nodes, as an enumerator.
        /// </summary>
        /// <param name="self">Designates the collection of tree/forest nodes to enumerate.</param>
        /// <param name="childSelector">Provides a function, that given a node, returns it children.</param>
        /// <param name="startNode">Specifies the set of nodes to begin the walk from.</param>
        /// <returns></returns>
        public static IEnumerable<TNode> DepthFirst<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            IEnumerable<TNode> startNodes)
        {
            Guard.AgainstNull(childSelector, nameof(childSelector));

            if (startNodes == null)
                yield break;

            foreach (var startNode in startNodes)
            {
                foreach (var node in DepthFirst(self, childSelector, startNode))
                    yield return node;
            }
        }

        #endregion Depth-first enumerations


        #region Breadth-first enumerations

        public static IEnumerable<TNode> BreadthFirst<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            TNode startNode)
        {
            return BreadthFirst(self, childSelector, new List<TNode> { startNode });
        }

        public static IEnumerable<TNode> BreadthFirst<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            IEnumerable<TNode> startNodes)
        {
            Guard.AgainstNull(childSelector, nameof(childSelector));

            Queue<TNode> queueOfNodesToVisit = new Queue<TNode>();

            if (startNodes == null)
                yield break;

            foreach (var node in startNodes)
                queueOfNodesToVisit.Enqueue(node);

            foreach (var node in BreadthFirst(self, childSelector, queueOfNodesToVisit))
                yield return node;
        }

        private static IEnumerable<TNode> BreadthFirst<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            Queue<TNode> nodesToVisit)
        {
            if (nodesToVisit == null)
                yield break;

            while (nodesToVisit.Count > 0)
            {
                TNode node = nodesToVisit.Dequeue();
                yield return node;

                foreach (TNode childNode in childSelector(node))
                    nodesToVisit.Enqueue(childNode);
            }
        }

        #endregion Breadth-first enumerations


        #region Bottom-up enumerations

        public static IEnumerable<TNode> BottomUp<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            TNode startNode)
        {
            return BottomUp(self, childSelector, new List<TNode> { startNode });
        }

        public static IEnumerable<TNode> BottomUp<TNode>(
            this IEnumerable<TNode> self,
            Func<TNode, IEnumerable<TNode>> childSelector,
            IEnumerable<TNode> startNodes)
        {
            var nodesBreadthFirst = new Stack<TNode>();
            foreach (TNode node in BreadthFirst(self, childSelector, startNodes))
                nodesBreadthFirst.Push(node);
            foreach(TNode node in nodesBreadthFirst)
                yield return node;
        }

        #endregion Bottom-up enumerations


        #region Helpers

        private static bool IsEmpty<TNode>(TNode obj)
        {
            return EqualityComparer<TNode>.Default.Equals(obj, default(TNode));
        }

        #endregion Helpers
    }
}
