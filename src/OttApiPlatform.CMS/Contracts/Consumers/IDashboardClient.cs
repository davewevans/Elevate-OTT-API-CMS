namespace OttApiPlatform.CMS.Contracts.Consumers;

/// <summary>
/// Provides methods for viewing dashboard data.
/// </summary>
public interface IDashboardClient
{
    #region Public Methods

    /// <summary>
    /// Gets the headlines data.
    /// </summary>
    /// <returns>A <see cref="HeadlinesResponse"/>.</returns>
    Task<ApiResponseWrapper<HeadlinesResponse>> GetHeadlinesData();

    #endregion Public Methods
}