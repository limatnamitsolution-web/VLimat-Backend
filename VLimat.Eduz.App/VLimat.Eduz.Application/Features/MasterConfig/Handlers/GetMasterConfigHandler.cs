using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using VLimat.Eduz.Application.Features.MasterConfig.Queries;
using VLimat.Eduz.Domain.Features.Masters.Services;
using VLimat.Eduz.Domain.Repositories;

namespace VLimat.Eduz.Application.Features.MasterConfig.Handlers
{
    // Uses IMasterConfigRepository so DI can provide the Dapper implementation (DapperMasterConfigRepository).
    public class GetMasterConfigHandler : IRequestHandler<GetMasterConfigQuery, MasterConfigResponse?>
    {
        private readonly IMasterConfigService _repo;

        public GetMasterConfigHandler(IMasterConfigService repo) => _repo = repo;

        public async Task<MasterConfigResponse?> Handle(GetMasterConfigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (entity == null) return null;

            return new MasterConfigResponse
            {
                ID = entity.Id,
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
