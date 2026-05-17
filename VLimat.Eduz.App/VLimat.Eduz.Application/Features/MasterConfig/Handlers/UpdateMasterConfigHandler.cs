using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Application.Features.MasterConfig.Commands;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using VLimat.Eduz.Domain.Features.Masters;
using VLimat.Eduz.Domain.Features.Masters.Services;
using VLimat.Eduz.Domain.Repositories;

namespace VLimat.Eduz.Application.Features.MasterConfig.Handlers
{
    // Uses IMasterConfigRepository so DI can provide the Dapper implementation (DapperMasterConfigRepository).
    public class UpdateMasterConfigHandler : IRequestHandler<MasterConfigUpdateCommand, MasterConfigResponse>
    {
        private readonly IMasterConfigService _repo;

        public UpdateMasterConfigHandler(IMasterConfigService repo) => _repo = repo;

        public async Task<MasterConfigResponse> Handle(MasterConfigUpdateCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;
            var entity = new VLimat.Eduz.Domain.Features.Masters.MasterConfig
            {
                Id = r.Id,
                AcademicId = r.AcademicId,
                Configuration = r.Configuration,
                ConfigKey = r.ConfigKey,
                ConfigValue = r.ConfigValue,
                Description = r.Description,
                SortOrder = r.SortOrder,
                AC_Yr = r.AC_Yr,
                CreatedBy = r.CreatedBy,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            await _repo.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

            // Since UpdateAsync returns void, use the entity for response
            return new MasterConfigResponse
            {
                AcademicId = entity.AcademicId,
                Configuration = entity.Configuration,
                ConfigKey = entity.ConfigKey,
                ConfigValue = entity.ConfigValue,
                Description = entity.Description,
                SortOrder = entity.SortOrder,
                AC_Yr = entity.AC_Yr,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy
            };
        }
    }
}
