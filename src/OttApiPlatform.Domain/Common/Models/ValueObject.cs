namespace OttApiPlatform.Domain.Common.Models;

/// <summary>
/// Base class for implementing value objects in domain models. Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects.
/// </summary>
public abstract class ValueObject
{
    #region Public Methods

    /// <summary>
    /// Determines whether the specified object is equal to this object by comparing atomic values.
    /// </summary>
    /// <param name="obj">The object to compare with this object.</param>
    /// <returns>true if the specified object is equal to this object; otherwise, false.</returns>
    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        using var thisValues = GetAtomicValues().GetEnumerator();
        using var otherValues = other.GetAtomicValues().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            if (thisValues.Current is null ^ otherValues.Current is null)
            {
                return false;
            }

            if (thisValues.Current != null &&
                !thisValues.Current.Equals(otherValues.Current))
            {
                return false;
            }
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    #endregion Public Methods

    #region Protected Methods

    /// <summary>
    /// Implements the equality operator.
    /// </summary>
    /// <param name="left">The left-hand value object to compare.</param>
    /// <param name="right">The right-hand value object to compare.</param>
    /// <returns>true if the two value objects are equal; otherwise, false.</returns>
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right) != false;
    }

    /// <summary>
    /// Implements the inequality operator.
    /// </summary>
    /// <param name="left">The left-hand value object to compare.</param>
    /// <param name="right">The right-hand value object to compare.</param>
    /// <returns>true if the two value objects are not equal; otherwise, false.</returns>
    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !EqualOperator(left, right);
    }

    /// <summary>
    /// Gets the atomic values of this value object.
    /// </summary>
    /// <returns>An enumerable of the atomic values.</returns>
    protected abstract IEnumerable<object> GetAtomicValues();

    #endregion Protected Methods
}