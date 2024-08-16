namespace OttApiPlatform.CMS.Features.POC.Applicants.Queries.ExportApplicants
{
    public class ExportApplicantsQuery
    {
        #region Public Properties

        public string SearchText { get; set; }
        public string SortBy { get; set; }
        public bool IsOnDemand { get; set; } = true;

        #endregion Public Properties
    }
}