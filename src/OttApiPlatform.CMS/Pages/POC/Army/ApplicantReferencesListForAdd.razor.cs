namespace OttApiPlatform.CMS.Pages.POC.Army;

public partial class ApplicantReferencesListForAdd
{
    #region Public Properties

    [Parameter] public EventCallback<List<ReferenceItemForAdd>> OnApplicantReferencesChanged { get; set; }

    public ReferenceItemForEdit ReferenceItemForEdit { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private IDialogService DialogService { get; set; }

    private string SearchString { get; set; }
    private List<ReferenceItemForAdd> AddedApplicantReferencesList { get; set; } = new();

    #endregion Private Properties

    #region Private Methods

    private bool FilterReferences(ReferenceItemForAdd item)
    {
        return string.IsNullOrWhiteSpace(SearchString) || item.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase);
    }

    private async Task AddApplicantReference()
    {
        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<AddApplicantReferenceFormDialog>(Resource.Assigned_Roles, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            SearchString = null;
            AddedApplicantReferencesList.Add((ReferenceItemForAdd)dialogResult.Data);
            await OnApplicantReferencesChanged.InvokeAsync(AddedApplicantReferencesList);
        }
    }

    private async Task EditApplicantReference(string id)
    {
        var referenceCommandParam = AddedApplicantReferencesList.Select(a =>
            new ReferenceItemForEdit
            {
                Id = a.Id,
                JobTitle = a.JobTitle,
                Name = a.Name,
                Phone = a.Phone,
            }).FirstOrDefault(i => i.Id == id);

        var dialogParameters = new DialogParameters { ["ReferenceItemForEdit"] = referenceCommandParam };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<EditApplicantReferenceFormDialog>(Resource.Add_Reference, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            SearchString = null;
            ReferenceItemForEdit = (ReferenceItemForEdit)dialogResult.Data;

            var modifiedReferenceIndex = AddedApplicantReferencesList.ToList()
                .FindIndex(item => item.Id == ReferenceItemForEdit.Id);

            if (modifiedReferenceIndex >= 0)
                AddedApplicantReferencesList[modifiedReferenceIndex] = new ReferenceItemForAdd
                {
                    Id = ReferenceItemForEdit.Id,
                    JobTitle = ReferenceItemForEdit.JobTitle,
                    Name = ReferenceItemForEdit.Name,
                    Phone = ReferenceItemForEdit.Phone,
                };
        }
    }

    private async Task RemoveApplicantReference(string id)
    {
        var dialog = await DialogService.ShowAsync<RemoveConfirmationDialog>(Resource.Remove);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var item = AddedApplicantReferencesList.Select(a =>
                new ReferenceItemForAdd
                {
                    Id = a.Id,
                    JobTitle = a.JobTitle,
                    Name = a.Name,
                    Phone = a.Phone,
                }).FirstOrDefault(i => i.Id == id);

            AddedApplicantReferencesList.Remove(item);

            await OnApplicantReferencesChanged.InvokeAsync(AddedApplicantReferencesList);
        }
    }

    #endregion Private Methods
}