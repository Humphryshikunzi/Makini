using _.Application.Features.Brands.Queries.GetAll;
using _.Application.Features.Products.Queries.GetAllPaged;
using _.Application.Requests.Catalog;
using _.Client.Extensions;
using _.Client.Infrastructure.Managers.Catalog.Speed;
using _.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _.Client.Pages.Location
{
  public partial class Location
  {
    [Inject] private ISpeedGovManager SpeedManager { get; set; }
    [CascadingParameter] private HubConnection HubConnection { get; set; }
    private bool _loaded;
    private string _searchString = "";
    private int _totalItems;
    private int _currentPage;
    private bool _dense = false;
    private bool _striped = true;
    private bool _bordered = false;
    private MudTable<GetAllPagedLocation> _table;
    private IEnumerable<GetAllPagedLocation> _pagedData;
    protected override async Task OnInitializedAsync()
    {
      HubConnection = HubConnection.TryInitialize(_navigationManager);
      if(HubConnection.State == HubConnectionState.Disconnected)
      {
        await HubConnection.StartAsync();
      }
      _loaded = true;
      
      HubConnection.On(ApplicationConstants.SignalR.ReceiveNewLocation, async () =>
      {
          await _table.ReloadServerData();
      });
    }

    private async Task<TableData<GetAllPagedLocation>> ServerReload(TableState state)
    {
      if (!string.IsNullOrWhiteSpace(_searchString))
      {
        state.Page = 0;
      }
      await LoadData(state.Page, state.PageSize, state);
      return new TableData<GetAllPagedLocation> { TotalItems = _totalItems, Items = _pagedData };
    }
    private void OnSearch(string text)
    {
      _searchString = text;
      _table.ReloadServerData();
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
      state.SortDirection = SortDirection.Descending;
      string[] orderings = null;
      if (!string.IsNullOrEmpty(state.SortLabel))
      {
        orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel}" } : new[] { $"{state.SortLabel} {state.SortDirection}" };
      }

      var request = new GetAllLocationRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
      var response = await SpeedManager.GetLocations(request);
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
    private bool Search(GetAllBrandsResponse brand)
    {
      if (string.IsNullOrWhiteSpace(_searchString)) return true;
      if (brand.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
      {
        return true;
      }
      if (brand.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
      {
        return true;
      }
      return false;
    }
  }
}
