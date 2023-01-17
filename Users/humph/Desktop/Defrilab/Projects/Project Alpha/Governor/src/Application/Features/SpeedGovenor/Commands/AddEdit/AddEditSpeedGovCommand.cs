using System.ComponentModel.DataAnnotations;
using AutoMapper;
using _.Application.Interfaces.Repositories;
using _.Application.Interfaces.Services;
using _.Application.Requests;
using _.Domain.Entities.Catalog;
using _.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace _.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditSpeedGovCommand : IRequest<Result<int>>
    {

        public string PlateNummber  { get; set; }
        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int CartypeId { get; set; }
        public string OwnerId { get; set; }
        public int Id { get; set; }
        [Required]
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditProductCommandHandler : IRequestHandler<AddEditSpeedGovCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditSpeedGovCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<SpeedGovernor>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.PlateNummber == command.PlateNummber, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["PlateNumber exist already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.PlateNummber}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var speedgovernor = _mapper.Map<SpeedGovernor>(command);
                if (uploadRequest != null)
                {
                    speedgovernor.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<SpeedGovernor>().AddAsync(speedgovernor);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(speedgovernor.Id, _localizer["Speed Governor Saved"]);
            }
            else
            {
                var speedgovenor = await _unitOfWork.Repository<SpeedGovernor>().GetByIdAsync(command.Id);
                if (speedgovenor != null)
                {
                    speedgovenor.PlateNummber = command.PlateNummber ?? speedgovenor.PlateNummber;
                    if (uploadRequest != null)
                    {
                        speedgovenor.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    speedgovenor.CartypeId = (command.CartypeId == 0) ? speedgovenor.CartypeId : command.CartypeId;
                    speedgovenor.OwnerId = command.OwnerId ?? speedgovenor.OwnerId;
                    await _unitOfWork.Repository<SpeedGovernor>().UpdateAsync(speedgovenor);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(speedgovenor.Id, _localizer["SpeedGovernor Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
                }
            }
        }
    }
}