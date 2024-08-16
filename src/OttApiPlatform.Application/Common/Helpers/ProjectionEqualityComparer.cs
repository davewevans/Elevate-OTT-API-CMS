namespace OttApiPlatform.Application.Common.Helpers;

public static class ProjectionEqualityComparer
{
    #region Public Methods

    /// <summary>
    /// Creates an instance of ProjectionEqualityComparer using the specified projection.
    /// </summary>
    /// <typeparam name="TSource">Type parameter for the elements to be compared</typeparam>
    /// <typeparam name="TKey">
    /// Type parameter for the keys to be compared, after being projected from the elements.
    /// </typeparam>
    /// <param name="projection">Projection to use when determining the key of an element</param>
    /// <returns>
    /// A comparer which will compare elements by projecting each element to its key, and comparing keys.
    /// </returns>
    public static ProjectionEqualityComparer<TSource, TKey> Create<TSource, TKey>(Func<TSource, TKey> projection)
    {
        return new(projection);
    }

    /// <summary>
    /// Creates an instance of ProjectionEqualityComparer using the specified projection. The.
    /// ignored parameter is solely present to aid type inference.
    /// </summary>
    /// <typeparam name="TSource">Type parameter for the elements to be compared</typeparam>
    /// <typeparam name="TKey">
    /// Type parameter for the keys to be compared, after being projected from the elements.
    /// </typeparam>
    /// <param name="ignored">Value is ignored - type may be used by type inference</param>
    /// <param name="projection">Projection to use when determining the key of an element</param>
    /// <returns>
    /// A comparer which will compare elements by projecting each element to its key, and comparing keys.
    /// </returns>
    public static ProjectionEqualityComparer<TSource, TKey> Create<TSource, TKey>
    (TSource ignored,
        Func<TSource, TKey> projection)
    {
        return new(projection);
    }

    #endregion Public Methods
}

/// <summary>
/// /// Class generic in the source only to produce instances of the doubly generic class,
/// optionally using type inference.
/// </summary>
public static class ProjectionEqualityComparer<TSource>
{
    #region Public Methods

    /// <summary>
    /// Creates an instance of ProjectionEqualityComparer using the specified projection.
    /// </summary>
    /// <typeparam name="TKey">
    /// Type parameter for the keys to be compared, after being projected from the elements.
    /// </typeparam>
    /// <param name="projection">Projection to use when determining the key of an element</param>
    /// <returns>
    /// A comparer which will compare elements by projecting each element to its key, and comparing keys.
    /// </returns>
    public static ProjectionEqualityComparer<TSource, TKey> Create<TKey>(Func<TSource, TKey> projection)
    {
        return new(projection);
    }

    #endregion Public Methods
}

/// Comparer which projects each element of the comparison to a key, and then compares those keys.
/// <summary>
/// using the specified (or default) comparer for the key type.
/// </summary>
/// <typeparam name="TSource">Type of elements which this comparer will be asked to compare</typeparam>
/// <typeparam name="TKey">Type of the key projected from the element</typeparam>
public class ProjectionEqualityComparer<TSource, TKey> : IEqualityComparer<TSource>
{
    #region Private Fields

    private readonly Func<TSource, TKey> _projection;
    private readonly IEqualityComparer<TKey> _comparer;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    /// Creates a new instance using the specified projection, which must not be null. The default.
    /// comparer for the projected type is used.
    /// </summary>
    /// <param name="projection">Projection to use during comparisons</param>
    public ProjectionEqualityComparer(Func<TSource, TKey> projection)
        : this(projection, null)
    {
    }

    /// <summary>
    /// Creates a new instance using the specified projection, which must not be null.
    /// </summary>
    /// <param name="projection">Projection to use during comparisons</param>
    /// <param name="comparer">
    /// The comparer to use on the keys. May be null, in which case the default comparer will be used.
    /// </param>
    public ProjectionEqualityComparer(Func<TSource, TKey> projection, IEqualityComparer<TKey> comparer)
    {
        if (projection is null)
            throw new ArgumentNullException(nameof(projection));

        _comparer = comparer ?? EqualityComparer<TKey>.Default;

        _projection = projection;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Compares the two specified values for equality by applying the projection to each value and.
    /// then using the equality comparer on the resulting keys. Null references are never passed to.
    /// the projection.
    /// </summary>
    public bool Equals(TSource x, TSource y)
    {
        if (x == null && y == null)
            return true;

        if (x == null || y == null)
            return false;

        return _comparer.Equals(_projection(x), _projection(y));
    }

    /// <summary>
    /// Produces a hash code for the given value by projecting it and then asking the equality.
    /// comparer to find the hash code of the resulting key.
    /// </summary>
    public int GetHashCode(TSource sourceObject)
    {
        if (sourceObject == null)
            throw new ArgumentNullException(nameof(sourceObject));

        return _comparer.GetHashCode(_projection(sourceObject));
    }

    #endregion Public Methods
}