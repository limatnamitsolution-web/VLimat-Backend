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
    public class CreateMasterConfigHandler : IRequestHandler<MasterConfigCreateCommand, MasterConfigResponse>
    {
        private readonly IMasterConfigService _repo;

        public CreateMasterConfigHandler(IMasterConfigService repo) => _repo = repo;

        public async Task<MasterConfigResponse> Handle(MasterConfigCreateCommand request, CancellationToken cancellationToken)
        {
            var r = request.Request;
            var entity = new VLimat.Eduz.Domain.Features.Masters.MasterConfig
            {
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

            var added = await _repo.AddAsync(entity, cancellationToken).ConfigureAwait(false);

            return new MasterConfigResponse
            {
                AcademicId = added.AcademicId,
                Configuration = added.Configuration,
                ConfigKey = added.ConfigKey,
                ConfigValue = added.ConfigValue,
                Description = added.Description,
                SortOrder = added.SortOrder,
                AC_Yr = added.AC_Yr,
                IsActive = added.IsActive,
                CreatedDate = added.CreatedDate,
                CreatedBy = added.CreatedBy
            };
        }
    }
}
