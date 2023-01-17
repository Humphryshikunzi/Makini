using _.Application.Features.Products.Commands.AddEdit;
using _.Application.Features.Products.Commands.Delete;
using _.Application.Features.Products.Queries.Export;
using _.Application.Features.Products.Queries.GetAllPaged;
using _.Application.Features.Products.Queries.GetProductImage;
using _.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _.Server.Controllers.v1.Catalog
{
    public class LocationController : BaseApiController<LocationController>
    {
        /// <summary>
        /// Get All Location
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.SpeedGovernor.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var location = await _mediator.Send(new GetAllLocationQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(location);
        }
    
    }
}