namespace OttApiPlatform.Application.Common.Extensions;

public static class TreeExtensions
{
    #region Public Interfaces

    /// <summary>
    /// Generic interface for tree node structure.
    /// </summary>
    /// <typeparam name="T">The type of data stored in each node.</typeparam>
    public interface ITree<T>
    {
        #region Public Properties

        /// <summary>
        /// The data stored in the node.
        /// </summary>
        T Data { get; }

        /// <summary>
        /// The parent node of the current node.
        /// </summary>
        ITree<T> Parent { get; }

        /// <summary>
        /// The children of the current node.
        /// </summary>
        ICollection<ITree<T>> Children { get; }

        /// <summary>
        /// Determines if the current node is the root of the tree.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Determines if the current node is a leaf node (i.e., has no children).
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// The level of the node in the tree hierarchy. The root node has level 0.
        /// </summary>
        int Level { get; }

        #endregion Public Properties
    }

    #endregion Public Interfaces

    #region Public Methods

    /// <summary>
    /// Flattens the tree structure into a plain list of nodes.
    /// </summary>
    /// <typeparam name="TNode">The type of node in the tree structure.</typeparam>
    /// <param name="nodes">The collection of nodes to flatten.</param>
    /// <param name="childrenSelector">The function used to select the children of each node.</param>
    /// <returns>The flattened list of nodes.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the nodes argument is null.</exception>
    public static IEnumerable<TNode> Flatten<TNode>(this IEnumerable<TNode> nodes, Func<TNode, IEnumerable<TNode>> childrenSelector)
    {
        if (nodes == null)
            throw new ArgumentNullException(nameof(nodes));

        var enumerable = nodes as TNode[] ?? nodes.ToArray();
        return enumerable.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(enumerable);
    }

    /// <summary>
    /// Converts a collection of items to a tree structure.
    /// </summary>
    /// <typeparam name="T">The type of data stored in each node.</typeparam>
    /// <param name="items">The collection of items to convert to a tree structure.</param>
    /// <param name="parentSelector">
    /// The function used to select the parent of each item. If null, the first item in the.
    /// collection is assumed to be the root node.
    /// </param>
    /// <returns>The root node of the resulting tree structure.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the items argument is null.</exception>
    public static ITree<T> ToTree<T>(this IReadOnlyList<T> items, Func<T, T, bool> parentSelector = null)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector?.Invoke(parent, item) ?? ReferenceEquals(parent, item)), child => child);

        return Tree<T>.FromLookup(lookup);
    }

    #endregion Public Methods

    #region Internal Classes

    /// <summary>
    /// Internal implementation of <see cref="ITree{T}"/>
    /// </summary>
    /// <param name="{T}">Custom data type to associate with tree node.</param>
    /// <summary>
    /// Internal implementation of the generic tree node structure.
    /// </summary>
    internal class Tree<T> : ITree<T>
    {
        #region Private Constructors

        /// <summary>
        /// Creates a new instance of the tree node with the given data.
        /// </summary>
        /// <param name="data">The data to associate with the tree node.</param>
        private Tree(T data)
        {
            Children = new LinkedList<ITree<T>>();
            Data = data;
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// Gets the data associated with the tree node.
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Gets the parent of the tree node.
        /// </summary>
        public ITree<T> Parent { get; private init; }

        /// <summary>
        /// Gets the children of the tree node.
        /// </summary>
        public ICollection<ITree<T>> Children { get; }

        /// <summary>
        /// Gets a value indicating whether the tree node is a root node.
        /// </summary>
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Gets a value indicating whether the tree node is a leaf node.
        /// </summary>
        public bool IsLeaf => Children.Count == 0;

        /// <summary>
        /// Gets the level of the tree node.
        /// </summary>
        public int Level => IsRoot ? 0 : Parent.Level + 1;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates a new tree from the given lookup.
        /// </summary>
        /// <param name="lookup">The lookup that maps parent nodes to their child nodes.</param>
        /// <returns>A new instance of the root node of the created tree.</returns>
        public static Tree<T> FromLookup(ILookup<T, T> lookup)
        {
            var rootData = lookup.Count == 1 ? lookup.First().Key : default(T);

            var root = new Tree<T>(rootData);

            root.LoadChildren(lookup);

            return root;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Loads the children of the tree node from the given lookup.
        /// </summary>
        /// <param name="lookup">The lookup that maps parent nodes to their child nodes.</param>
        private void LoadChildren(ILookup<T, T> lookup)
        {
            foreach (var data in lookup[Data])
            {
                var child = new Tree<T>(data) { Parent = this };

                Children.Add(child);

                child.LoadChildren(lookup);
            }
        }

        #endregion Private Methods
    }

    #endregion Internal Classes
}