namespace OttApiPlatform.Application.Common.Extensions;

public static class LinqExtensions
{
    #region Public Methods

    /// <summary>
    /// Includes related entities in the query by following the navigation properties specified in
    /// the <paramref name="propertyName"/> parameter up to the <paramref name="depth"/> specified.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="source">The source query.</param>
    /// <param name="depth">The depth up to which the navigation properties will be included.</param>
    /// <param name="propertyName">The name of the navigation property to include.</param>
    /// <returns>The queryable with the included entities.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> is null.</exception>
    public static IQueryable<T> IncludeHierarchy<T>(this IQueryable<T> source, uint depth, string propertyName) where T : class
    {
        // Includes related entities in the query by following the navigation properties specified
        // in the propertyName parameter up to the depth specified.
        var temp = source;

        for (var i = 1; i <= depth; i++)
        {
            var sb = new StringBuilder();

            for (var j = 0; j < i; j++)
            {
                if (j > 0)
                    sb.Append(".");

                sb.Append(propertyName);
            }

            var path = sb.ToString();

            temp = temp.Include(path);
        }

        var result = temp;

        return result;
    }

    /// <summary>
    /// Orders the elements of a sequence in ascending order according to a key.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    /// <param name="query">The sequence to sort.</param>
    /// <param name="name">The name of the property to sort by.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements in ascending order.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="query"/> or <paramref name="name"/> is null.
    /// </exception>
    public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name)
    {
        // Get the PropertyInfo object for the specified property name.
        var propInfo = GetPropertyInfo(typeof(T), name);

        // Get the lambda expression to be used for ordering the sequence.
        var expr = GetOrderExpression(typeof(T), propInfo);

        // Get the OrderBy method of the Enumerable class.
        var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);

        // Check if the OrderBy method is null.
        if (method is null)
            throw new ArgumentNullException(nameof(method), Resource.Method_cannot_be_null);

        // Create a generic method using the OrderBy method and the types of T and the property.
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

        // Invoke the generic method to order the sequence and return the ordered sequence.
        return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
    }

    /// <summary>
    /// Orders the elements of a sequence in descending order according to a key.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    /// <param name="query">The sequence to sort.</param>
    /// <param name="name">The name of the property to sort by.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the elements in descending order.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="query"/> or <paramref name="name"/> is null.
    /// </exception>
    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string name)
    {
        // Get the property information for the specified property name.
        var propInfo = GetPropertyInfo(typeof(T), name);

        // Get the order expression.
        var expr = GetOrderExpression(typeof(T), propInfo);

        // Get the OrderByDescending method using reflection.
        var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);

        // Throw an exception if the method is null.
        if (method is null)
            throw new ArgumentNullException(nameof(method), Resource.Method_cannot_be_null);

        // Create a generic method based on the type of the query and the property type.
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

        // Invoke the generic method and return the result.
        return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
    }

    /// <summary>
    /// Sorts the elements of a sequence according to a specified property name and order.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the sequence.</typeparam>
    /// <param name="query">The sequence to sort.</param>
    /// <param name="sort">
    /// A string that specifies the property name and order to sort by, separated by a space. The
    /// order can be "asc" or "desc".
    /// </param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the sorted elements.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="query"/> or <paramref name="sort"/> is null.
    /// </exception>
    public static IEnumerable<T> SortBy<T>(this IEnumerable<T> query, string sort)
    {
        // Split the sort parameter into property name and order.
        var propertyAndOrder = sort.Split(new[] { ' ' });

        // Define the property name.
        var propertyName = string.Empty;

        // Define the order.
        var orderBy = string.Empty;

        // Check if the propertyAndOrder array has at least two elements.
        if (propertyAndOrder.Length == 2)
        {
            // Get the property name.
            propertyName = propertyAndOrder[0];

            // Get the order.
            orderBy = propertyAndOrder[1];
        }
        else
        {
            // Handle the case when the sort parameter is not formatted correctly.
            throw new Exception("Invalid sort parameter. Expected format: {PropertyName} {OrderBy}");
        }

        // Get the PropertyInfo object for the specified property name.
        var propInfo = GetPropertyInfo(typeof(T), propertyName);

        // Get the order expression for the specified PropertyInfo object.
        var expr = GetOrderExpression(typeof(T), propInfo);

        // Determine the OrderBy or OrderByDescending method to use based on the specified order.
        var method = orderBy is "asc" or "Ascending"
            ? typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
            : typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);

        // Throw an exception if the method cannot be found.
        if (method is null)
            throw new ArgumentNullException(nameof(method), Resource.Method_cannot_be_null);

        // Make the generic method for the specified types.
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

        // Invoke the method with the specified parameters and return the result.
        return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, expr.Compile() });
    }

    /// <summary>
    /// Sorts the specified queryable based on the specified sort parameter.
    /// </summary>
    /// <typeparam name="T">The type of elements in the query.</typeparam>
    /// <param name="query">The queryable to sort.</param>
    /// <param name="sort">
    /// The sort parameter, which is a string that specifies the property name and sort order (asc.
    /// or desc).
    /// </param>
    /// <returns>An IQueryable of the sorted elements.</returns>
    public static IQueryable<T> SortBy<T>(this IQueryable<T> query, string sort)
    {
        // Split the sort parameter into property name and order.
        var propertyAndOrder = sort.Split(new[] { ' ' });

        // Define the property name.
        var propertyName = string.Empty;

        // Define the order.
        var orderBy = string.Empty;

        // Check if the propertyAndOrder array has at least two elements.
        if (propertyAndOrder.Length == 2)
        {
            // Get the property name.
            propertyName = propertyAndOrder[0];

            // Get the order.
            orderBy = propertyAndOrder[1];
        }
        else
        {
            // Handle the case when the sort parameter is not formatted correctly.
            throw new Exception("Invalid sort parameter. Expected format: {PropertyName} {OrderBy}");
        }

        // Get the PropertyInfo object for the specified property name.
        var propInfo = GetPropertyInfo(typeof(T), propertyName);

        // Get the order expression for the specified PropertyInfo object.
        var expr = GetOrderExpression(typeof(T), propInfo);

        // Determine the OrderBy or OrderByDescending method to use based on the specified order.
        var method = orderBy is "asc" or "Ascending"
            ? typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
            : typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2);

        // Throw an exception if the method cannot be found.
        if (method is null)
            throw new ArgumentNullException(nameof(method), Resource.Method_cannot_be_null);

        // Make the generic method for the specified types.
        var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);

        // Invoke the method with the specified parameters and return the result.
        return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, expr });
    }

    /// <summary>
    /// Converts the given <see cref="IQueryable{T}"/> object to a paged list asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The <see cref="IQueryable{T}"/> object to convert.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based index).</param>
    /// <param name="totalRowsPerPage">The number of rows per page to retrieve.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation that returns a <see
    /// cref="PagedList{T}"/> object.
    /// </returns>
    /// <remarks>If <paramref name="totalRowsPerPage"/> is -1, retrieves all rows.</remarks>
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageNumber = 1, int totalRowsPerPage = 10) where T : class
    {
        // Initialize a new instance of PagedList<T>
        var result = new PagedList<T>
        {
            // If pageNumber is not equal to 0, set CurrentPage to pageNumber; otherwise, set it to 1.
            CurrentPage = pageNumber != 0 ? pageNumber : 1,
            // If totalRowsPerPage is not equal to 0, set TotalRowsPerPage to totalRowsPerPage;
            // otherwise, set it to 10.
            TotalRowsPerPage = totalRowsPerPage != 0 ? totalRowsPerPage : 10,
            // Set TotalRows to the count of items in the query.
            TotalRows = query.Count()
        };

        // If totalRowsPerPage is not equal to -1 (indicating no paging), calculate paging
        // information and retrieve the items for the current page.
        if (totalRowsPerPage != -1)
        {
            // Calculate the total number of pages.
            var totalPages = (double)result.TotalRows / result.TotalRowsPerPage;
            result.TotalPages = (int)Math.Ceiling(totalPages);

            // Calculate the number of items to skip and retrieve the items for the current page.
            var skip = (result.CurrentPage - 1) * result.TotalRowsPerPage;
            result.Items = await query.Skip(skip).Take(result.TotalRowsPerPage).ToListAsync();
        }
        else
        {
            // If totalRowsPerPage is -1, retrieve all items in the query.
            result.Items = await query.ToListAsync();
        }

        // Return the paged list.
        return result;
    }

    /// <summary>
    /// Converts the given <see cref="IEnumerable{T}"/> object to a paged list asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the query.</typeparam>
    /// <param name="query">The <see cref="IEnumerable{T}"/> object to convert.</param>
    /// <param name="pageNumber">The page number to retrieve (1-based index).</param>
    /// <param name="totalRowsPerPage">The number of rows per page to retrieve.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation that returns a <see
    /// cref="PagedList{T}"/> object.
    /// </returns>
    /// <remarks>If <paramref name="totalRowsPerPage"/> is -1, retrieves all rows.</remarks>
    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> query, int pageNumber = 1, int totalRowsPerPage = 10) where T : class
    {
        // Convert the query to a list so that it can be used for paging.
        var enumerable = query.ToList();

        // Initialize a new instance of PagedList<T>
        var result = new PagedList<T>
        {
            // If pageNumber is not equal to 0, set CurrentPage to pageNumber; otherwise, set it to 1.
            CurrentPage = pageNumber != 0 ? pageNumber : 1,
            // If totalRowsPerPage is not equal to 0, set TotalRowsPerPage to totalRowsPerPage;
            // otherwise, set it to 10.
            TotalRowsPerPage = totalRowsPerPage != 0 ? totalRowsPerPage : 10,
            // Set TotalRows to the count of items in the query.
            TotalRows = enumerable.Count
        };

        // If totalRowsPerPage is not equal to -1 (indicating no paging), calculate paging
        // information and retrieve the items for the current page.
        if (totalRowsPerPage != -1)
        {
            // Calculate the total number of pages.
            var totalPages = (double)result.TotalRows / result.TotalRowsPerPage;
            result.TotalPages = (int)Math.Ceiling(totalPages);

            // Calculate the number of items to skip and retrieve the items for the current page.
            var skip = (result.CurrentPage - 1) * result.TotalRowsPerPage;
            result.Items = await Task.FromResult(enumerable.Skip(skip).Take(result.TotalRowsPerPage).ToList());
        }
        else
        {
            // If totalRowsPerPage is -1, retrieve all items in the query.
            result.Items = await Task.FromResult(enumerable.ToList());
        }

        // Return the paged list.
        return result;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> object for the property with the specified name on the.
    /// specified object type.
    /// </summary>
    /// <param name="objType">The type of the object containing the property.</param>
    /// <param name="name">The name of the property.</param>
    /// <returns>The <see cref="PropertyInfo"/> object for the specified property.</returns>
    private static PropertyInfo GetPropertyInfo(Type objType, string name)
    {
        var properties = objType.GetProperties();

        var matchedProperty = properties.FirstOrDefault(p => p.Name.ToUpper() == name.ToUpper());

        if (matchedProperty == null)
            throw new ArgumentException("name");

        return matchedProperty;
    }

    /// <summary>
    /// Returns a lambda expression that accesses the specified property of an object of the.
    /// specified type.
    /// </summary>
    /// <param name="objType">The type of object that contains the property.</param>
    /// <param name="pi">The <see cref="PropertyInfo"/> object representing the property to access.</param>
    /// <returns>
    /// A lambda expression that accesses the specified property of an object of the specified type.
    /// </returns>
    private static LambdaExpression GetOrderExpression(Type objType, PropertyInfo pi)
    {
        var paramExpr = Expression.Parameter(objType);

        var propAccess = Expression.PropertyOrField(paramExpr, pi.Name);

        var expr = Expression.Lambda(propAccess, paramExpr);

        return expr;
    }

    #endregion Private Methods
}