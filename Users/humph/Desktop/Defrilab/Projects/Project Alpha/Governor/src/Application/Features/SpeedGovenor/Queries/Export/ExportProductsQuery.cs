using _.Application.Interfaces.Repositories;
using _.Application.Interfaces.Services;
using _.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _.Application.Extensions;
using _.Application.Specifications.Catalog;
using _.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace _.Application.Features.Products.Queries.Export
{
    public class ExportProductsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportProductsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportProductsQueryHandler : IRequestHandler<ExportProductsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;

        public ExportProductsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProductsQuery request, CancellationToken cancellationToken)
        {
            var productFilterSpec = new ProductFilterSpecification(request.SearchString);
            var products = await _unitOfWork.Repository<SpeedGovernor>().Entities
                .Specify(productFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<SpeedGovernor, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["PlateNumber"], item => item.PlateNummber },
                { _localizer["PhoneNumber"], item => item.PhoneNumber },
                { _localizer["ImageDataURL"], item => item.ImageDataURL },
            }, sheetName: _localizer["Products"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}