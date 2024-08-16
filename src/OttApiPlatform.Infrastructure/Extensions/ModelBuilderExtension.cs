/*
Copyright Phil Haack
Licensed under the MIT license - https://github.com/haacked/CodeHaacks/blob/main/LICENSE.
*/

namespace OttApiPlatform.Infrastructure.Extensions;

public static class ModelBuilderExtension
{
    #region Private Fields

    private static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtension)
                                                              .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                                                              .Single(t => t.IsGenericMethod && t.Name == nameof(SetQueryFilter));

    #endregion Private Fields

    #region Public Methods

    public static void SetQueryFilter<TEntityInterface>(this ModelBuilder builder, Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        foreach (var type in builder.Model.GetEntityTypes()
                                          .Where(t => t.BaseType == null)
                                          .Select(t => t.ClrType)
                                          .Where(t => typeof(TEntityInterface).IsAssignableFrom(t)))
            builder.SetEntityQueryFilter(type, filterExpression);
    }

    #endregion Public Methods

    #region Private Methods

    private static void SetEntityQueryFilter<TEntityInterface>(this ModelBuilder builder, Type entityType, Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        SetQueryFilterMethod.MakeGenericMethod(entityType, typeof(TEntityInterface)).Invoke(null, new object[] { builder, filterExpression });
    }

    private static void SetQueryFilter<TEntity, TEntityInterface>(this ModelBuilder builder, Expression<Func<TEntityInterface, bool>> filterExpression) where TEntityInterface : class
        where TEntity : class, TEntityInterface
    {
        var concreteExpression = filterExpression.Convert<TEntityInterface, TEntity>();
        builder.Entity<TEntity>().AppendQueryFilter(concreteExpression);
    }

    // CREDIT: This comment by magiak on GitHub https://github.com/dotnet/efcore/issues/10275#issuecomment-785916356.
    private static void AppendQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder, Expression<Func<T, bool>> expression) where T : class
    {
        var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);

        var expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

        if (entityTypeBuilder.Metadata.GetQueryFilter() != null)
        {
            var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();

            if (currentQueryFilter != null)
            {
                var currentExpressionFilter = ReplacingExpressionVisitor.Replace(currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);

                expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
            }
        }

        var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);

        entityTypeBuilder.HasQueryFilter(lambdaExpression);
    }

    #endregion Private Methods
}

public static class ExpressionExtension
{
    #region Public Methods

    public static Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(this Expression<Func<TSource, bool>> root)
    {
        var visitor = new ParameterTypeVisitor<TSource, TTarget>();

        return (Expression<Func<TTarget, bool>>)visitor.Visit(root);
    }

    #endregion Public Methods

    #region Private Classes

    private class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
    {
        #region Private Fields

        private ReadOnlyCollection<ParameterExpression> _parameters;

        #endregion Private Fields

        #region Protected Methods

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _parameters?.FirstOrDefault(p => p.Name == node.Name) ?? (node.Type == typeof(TSource) ? Expression.Parameter(typeof(TTarget), node.Name) : node);
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            _parameters = VisitAndConvert(node.Parameters, "VisitLambda");
            return Expression.Lambda(Visit(node.Body), _parameters);
        }

        #endregion Protected Methods
    }

    #endregion Private Classes
}