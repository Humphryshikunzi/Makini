using _.Application.Features.Brands.Queries.GetAll;
using _.Client.Extensions;
using _.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using _.Client.Infrastructure.Managers.Catalog.Speed;
using _.Application.Features.Products.Queries.GetAllPaged;
using _.Application.Requests.Catalog;
using _.Application.Features.Products.Commands.AddEdit;
using _.Application.Features.Location;

namespace _.Client.Pages.Catalog
{
    public partial class SpeedGovernor
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Inject]private ISpeedGovManager SpeedManager { get; set; }

        private MudTable<GetAllPagedSpeedGovernor> _table;
          private IEnumerable<GetAllPagedSpeedGovernor> _pagedData;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateSpeedGov;
        private bool _canEditSpeedGov;
        private bool _canDeleteSpeedGov;
        private bool _canExportSpeedGov;
        private bool _canSearchSpeedGov;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateSpeedGov = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.SpeedGovernor.Create)).Succeeded;
            _canEditSpeedGov = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.SpeedGovernor.Edit)).Succeeded;
            _canDeleteSpeedGov = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.SpeedGovernor.Delete)).Succeeded;
            _canExportSpeedGov = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.SpeedGovernor.Export)).Succeeded;
            _canSearchSpeedGov = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.SpeedGovernor.Search)).Succeeded;
          
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var speedgov = _pagedData.FirstOrDefault(c => c.Id == id);
                if (speedgov != null)
                {
                    parameters.Add(nameof(AddEditSpeedGovernorModal.AddEditProductModel), new AddEditSpeedGovCommand
                    {
                        Id = speedgov.Id,
                        PlateNummber = speedgov.PlateNumber,
                        PhoneNumber = speedgov.PhoneNumber,
                        OwnerId = speedgov.OwnerId,
                        CartypeId = speedgov.CarTypeId,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditSpeedGovernorModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
        }


        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
                {
                    {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
                };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await SpeedManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }



        private async Task ExportToExcel()
        {
            var response = await SpeedManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(SpeedGovernor).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Products exported"]
                    : _localizer["Filtered Products exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }




        private async Task<TableData<GetAllPagedSpeedGovernor>> ServerReload(TableState state)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedSpeedGovernor> { TotalItems = _totalItems, Items = _pagedData };
        }


        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllSpeedGovernorRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await SpeedManager.GetSpeedGovernors(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }
        private bool Search(LocationResponse locationresponse)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (locationresponse.Date?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (locationresponse.Latitude.ToString()?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }

            if (locationresponse.Long.ToString()?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}
