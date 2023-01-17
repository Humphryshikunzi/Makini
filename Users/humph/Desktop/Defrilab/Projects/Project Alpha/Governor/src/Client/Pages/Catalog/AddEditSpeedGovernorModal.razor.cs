using _.Application.Features.Brands.Queries.GetAll;
using _.Application.Features.Products.Commands.AddEdit;
using _.Application.Requests;
using _.Client.Extensions;
using _.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using _.Client.Infrastructure.Managers.Catalog.Brand;
using _.Client.Infrastructure.Managers.Catalog.Speed;
using _.Client.Infrastructure.Managers.Identity.Users;
using _.Application.Responses.Identity;
using _.Shared.Wrapper;

namespace _.Client.Pages.Catalog
{
    public partial class AddEditSpeedGovernorModal
    {
        [Inject] private ISpeedGovManager ProductManager { get; set; }
        [Inject] private IBrandManager BrandManager { get; set; }
        [Inject] private IUserManager UserManager { get; set; }

        [Parameter] public AddEditSpeedGovCommand AddEditProductModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllBrandsResponse> _brands = new();
        private List<UserResponse> _users = new();

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await ProductManager.SaveAsync(AddEditProductModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadUsersAsync()
        {
            var data = await UserManager.GetAllAsync();
            if (data.Succeeded)
            {
                _users = data.Data;
            }
        }

        private async Task LoadDataAsync()
        {
            await LoadImageAsync();
            await LoadBrandsAsync();
            await LoadUsersAsync();
        }

    

        private async Task LoadBrandsAsync()
        {
            var data = await BrandManager.GetAllAsync();
            if (data.Succeeded)
            {
                _brands = data.Data;
            }
        }

        private async Task LoadImageAsync()
        {
            var data = await ProductManager.GetSpeedGovernorImageAsync(AddEditProductModel.Id);
            if (data.Succeeded)
            {
                var imageData = data.Data;
                if (!string.IsNullOrEmpty(imageData))
                {
                    AddEditProductModel.ImageDataURL = imageData;
                }
            }
        }

        private void DeleteAsync()
        {
            AddEditProductModel.ImageDataURL = null;
            AddEditProductModel.UploadRequest = new UploadRequest();
        }

        private IBrowserFile _file;

        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _file = e.File;
            if (_file != null)
            {
                var extension = Path.GetExtension(_file.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditProductModel.ImageDataURL = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductModel.UploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Product, Extension = extension };
            }
        }

        private async Task<IEnumerable<int>> SearchBrands(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(1);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _brands.Select(x => x.Id);

            return _brands.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<string>> SearchUsers(string value)
        {
            await Task.Delay(1);
            if (string.IsNullOrEmpty(value))
            {
                return _users.Select(x => x.Id);
            }

                return _users.Where(x => x.FirstName.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);

        }
    }
}