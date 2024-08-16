namespace OttApiPlatform.CMS.Pages.POC.Army;

public partial class ApplicantReferencesListForEdit
{
    #region Public Fields

    public List<ReferenceItemForAdd> AddedApplicantReferencesList = new();
    public List<ReferenceItemForEdit> ModifiedApplicantReferencesList = new();
    public List<string> RemovedApplicantReferencesList = new();

    public ReferenceItemForAdd ApplicantReferenceToBeAdded = new();
    public ReferenceItemForEdit ReferenceItemToBeModified = new();

    #endregion Public Fields

    #region Public Properties

    [Parameter] public string ApplicantId { get; set; }

    [Parameter] public EventCallback<List<ReferenceItemForAdd>> OnAddedApplicantReferencesListChanged { get; set; }
    [Parameter] public EventCallback<List<ReferenceItemForEdit>> OnModifiedApplicantReferencesListChanged { get; set; }
    [Parameter] public EventCallback<List<string>> OnRemovedApplicantReferencesListChanged { get; set; }

    #endregion Public Properties

    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }

    private string SearchString { get; set; }
    private MudTable<ApplicantReferenceItem> Table { get; set; }
    private GetApplicantReferencesQuery GetApplicantReferencesQuery { get; set; } = new();
    private ApplicantReferencesResponse ApplicantReferencesResponse { get; set; }

    #endregion Private Properties

    #region Private Methods

    private async Task<TableData<ApplicantReferenceItem>> ServerReload(TableState state)
    {
        GetApplicantReferencesQuery.ApplicantId = ApplicantId;

        GetApplicantReferencesQuery.SearchText = SearchString;

        GetApplicantReferencesQuery.PageNumber = state.Page + 1;

        GetApplicantReferencesQuery.RowsPerPage = state.PageSize;

        GetApplicantReferencesQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await ApplicantsClient.GetApplicantReferences(GetApplicantReferencesQuery);

        var tableData = new TableData<ApplicantReferenceItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            ApplicantReferencesResponse = responseWrapper.Payload;

            RefreshTableData();

            tableData = new TableData<ApplicantReferenceItem>() { TotalItems = ApplicantReferencesResponse.ApplicantReferences.TotalRows, Items = ApplicantReferencesResponse.ApplicantReferences.Items };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        return tableData;
    }

    private void RefreshTableData()
    {
        if (AddedApplicantReferencesList.Count != 0)
            for (var index = 0; index < AddedApplicantReferencesList.Count; index++)
            {
                var item = AddedApplicantReferencesList[index];
                ApplicantReferencesResponse.ApplicantReferences.Items.Insert(index, new ApplicantReferenceItem
                {
                    Id = item.Id,
                    JobTitle = item.JobTitle,
                    Name = item.Name,
                    Phone = item.Phone,
                    IsAddedOrModified = true,
                });
            }

        if (ModifiedApplicantReferencesList.Count != 0)
            foreach (var reference in ModifiedApplicantReferencesList)
            {
                var referencesResponseIndex = ApplicantReferencesResponse.ApplicantReferences.Items.ToList()
                                                                         .FindIndex(item => item.Id == reference.Id);

                if (referencesResponseIndex >= 0)
                    ApplicantReferencesResponse.ApplicantReferences.Items[referencesResponseIndex] =
                        new ApplicantReferenceItem
                        {
                            Id = reference.Id,
                            Name = reference.Name,
                            JobTitle = reference.JobTitle,
                            Phone = reference.Phone,
                            CreatedOn = reference.CreatedOn,
                            IsAddedOrModified = true,
                        };
            }

        if (RemovedApplicantReferencesList.Count != 0)
            foreach (var referenceId in RemovedApplicantReferencesList)
            {
                var referencesResponseObj =
                    ApplicantReferencesResponse.ApplicantReferences.Items.FirstOrDefault(item => item.Id == referenceId);
                if (referencesResponseObj is not null)
                    ApplicantReferencesResponse.ApplicantReferences.Items.Remove(referencesResponseObj);
            }
    }

    private void FilterApplicantReferences(string searchString)
    {
        if (ApplicantReferencesResponse is null)
            return;
        SearchString = searchString;
        Table.ReloadServerData();
    }

    private async Task AddApplicantReference()
    {
        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<AddApplicantReferenceFormDialog>(Resource.Assigned_Roles, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            SearchString = null;
            ApplicantReferenceToBeAdded = (ReferenceItemForAdd)dialogResult.Data;
            var id = Guid.NewGuid().ToString();
            ApplicantReferenceToBeAdded.Id = id;
            AddedApplicantReferencesList.Add(ApplicantReferenceToBeAdded);
            ApplicantReferencesResponse.ApplicantReferences.Items.Insert(0, new ApplicantReferenceItem
            {
                Id = ApplicantReferenceToBeAdded.Id,
                JobTitle = ApplicantReferenceToBeAdded.JobTitle,
                Name = ApplicantReferenceToBeAdded.Name,
                Phone = ApplicantReferenceToBeAdded.Phone,
                IsAddedOrModified = true,
            }); // Inserting an item at index 0 will place the inserted object at the beginning of the list and the other items will be shifted up by one.

            await OnAddedApplicantReferencesListChanged.InvokeAsync(AddedApplicantReferencesList);
        }
    }

    private async Task EditApplicantReference(string id)
    {
        var referenceCommandParam = ApplicantReferencesResponse.ApplicantReferences.Items.Select(a =>
            new ReferenceItemForEdit
            {
                Id = a.Id,
                JobTitle = a.JobTitle,
                Name = a.Name,
                Phone = a.Phone,
                CreatedOn = a.CreatedOn ?? DateTime.Now,
            }).FirstOrDefault(i => i.Id == id);

        var dialogParameters = new DialogParameters { ["ReferenceItemForEdit"] = referenceCommandParam };

        var dialogOptions = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Large };

        var dialog = await DialogService.ShowAsync<EditApplicantReferenceFormDialog>(Resource.Edit_Reference, dialogParameters, dialogOptions);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            SearchString = null;

            ReferenceItemToBeModified = (ReferenceItemForEdit)dialogResult.Data;

            IsNewlyAddedReferenceModified();

            IsCurrentReferenceModified();

            //await Table.ReloadServerData();
            await OnModifiedApplicantReferencesListChanged.InvokeAsync(ModifiedApplicantReferencesList);
        }
    }

    private void IsCurrentReferenceModified()
    {
        var modifiedReferenceIndex =
            ModifiedApplicantReferencesList.FindIndex(item => item.Id == ReferenceItemToBeModified.Id);

        if (modifiedReferenceIndex >= 0)
            ModifiedApplicantReferencesList[modifiedReferenceIndex] = ReferenceItemToBeModified;
        else
            ModifiedApplicantReferencesList.Add(ReferenceItemToBeModified);

        var referencesResponseIndex = ApplicantReferencesResponse.ApplicantReferences.Items.ToList()
                                                                 .FindIndex(item => item.Id == ReferenceItemToBeModified.Id);

        if (referencesResponseIndex >= 0)
            ApplicantReferencesResponse.ApplicantReferences.Items[referencesResponseIndex] =
                new ApplicantReferenceItem
                {
                    Id = ReferenceItemToBeModified.Id,
                    Name = ReferenceItemToBeModified.Name,
                    JobTitle = ReferenceItemToBeModified.JobTitle,
                    Phone = ReferenceItemToBeModified.Phone,
                    CreatedOn = ReferenceItemToBeModified.CreatedOn,
                    IsAddedOrModified = true,
                };
    }

    private void IsNewlyAddedReferenceModified()
    {
        var modifiedNewlyAddedReferenceIndex =
            AddedApplicantReferencesList.FindIndex(item => item.Id == ReferenceItemToBeModified.Id);
        if (modifiedNewlyAddedReferenceIndex >= 0)
            AddedApplicantReferencesList[modifiedNewlyAddedReferenceIndex] = new ReferenceItemForAdd
            {
                Id = ReferenceItemToBeModified.Id,
                JobTitle = ReferenceItemToBeModified.JobTitle,
                Name = ReferenceItemToBeModified.Name,
                Phone = ReferenceItemToBeModified.Phone,
            };
    }

    private async Task RemoveApplicantReference(string id)
    {
        var dialog = await DialogService.ShowAsync<RemoveConfirmationDialog>(Resource.Remove);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            RemoveAddedApplicantReference(id);
            await RemoveCurrentApplicantReference(id);
        }
    }

    private void RemoveAddedApplicantReference(string id)
    {
        var addedApplicantReferenceObj = AddedApplicantReferencesList.FirstOrDefault(item => item.Id == id);

        if (addedApplicantReferenceObj is not null)
            AddedApplicantReferencesList.Remove(addedApplicantReferenceObj);

        var referencesResponseObj = ApplicantReferencesResponse.ApplicantReferences.Items.FirstOrDefault(item => item.Id == id);

        if (referencesResponseObj is not null)
            ApplicantReferencesResponse.ApplicantReferences.Items.Remove(referencesResponseObj);
    }

    private async Task RemoveCurrentApplicantReference(string id)
    {
        var modifiedApplicantReferenceObj = ModifiedApplicantReferencesList.FirstOrDefault(item => item.Id == id);

        if (modifiedApplicantReferenceObj is not null)
            ModifiedApplicantReferencesList.Remove(modifiedApplicantReferenceObj);

        var referencesResponseObj = ApplicantReferencesResponse.ApplicantReferences.Items.FirstOrDefault(item => item.Id == id);

        if (referencesResponseObj is not null)
            ApplicantReferencesResponse.ApplicantReferences.Items.Remove(referencesResponseObj);

        RemovedApplicantReferencesList.Add(id);

        await OnRemovedApplicantReferencesListChanged.InvokeAsync(RemovedApplicantReferencesList);
    }

    #endregion Private Methods
}