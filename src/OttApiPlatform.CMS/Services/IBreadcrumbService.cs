namespace OttApiPlatform.CMS.Services;

public interface IBreadcrumbService
{
    #region Public Events

    event EventHandler BreadcrumbChanged;

    #endregion Public Events

    #region Public Properties

    List<BreadcrumbItem> BreadcrumbItems { get; set; }

    #endregion Public Properties

    #region Public Methods

    void Reset();

    void SetBreadcrumbItems(List<BreadcrumbItem> breadcrumbItems);

    #endregion Public Methods
}