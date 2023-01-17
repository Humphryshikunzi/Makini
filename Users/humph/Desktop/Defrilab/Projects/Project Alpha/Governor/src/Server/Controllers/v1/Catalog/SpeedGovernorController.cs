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
    public class SpeedGovernorController : BaseApiController<SpeedGovernorController>
    {
        /// <summary>
        /// Get All Products
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
            var products = await _mediator.Send(new GetAllProductQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(products);
        }

        /// <summary>
        /// Get a Product Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.SpeedGovernor.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetProductImageAsync(int id)
        {
            var result = await _mediator.Send(new GetProductImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Product
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.SpeedGovernor.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditSpeedGovCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.SpeedGovernor.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand { Id = id }));
        }

        /// <summary>
        /// Search Products and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.SpeedGovernor.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProductsQuery(searchString)));
        }
    }
}