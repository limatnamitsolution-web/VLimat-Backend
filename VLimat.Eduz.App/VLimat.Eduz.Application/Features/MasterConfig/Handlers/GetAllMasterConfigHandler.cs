using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;
using VLimat.Eduz.Application.Features.MasterConfig.Queries;
using VLimat.Eduz.Domain.Features.Masters.Services;
using VLimat.Eduz.Domain.Repositories;

namespace VLimat.Eduz.Application.Features.MasterConfig.Handlers
{
    // Uses IMasterConfigRepository so DI can provide the Dapper implementation (DapperMasterConfigRepository).
    public class GetAllMasterConfigHandler : IRequestHandler<GetAllMasterConfigQuery, IEnumerable<MasterConfigResponse>>
    {
        private readonly IMasterConfigService _repo;

        public GetAllMasterConfigHandler(IMasterConfigService repo) => _repo = repo;

        public async Task<IEnumerable<MasterConfigResponse>> Handle(GetAllMasterConfigQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repo.GetAllAsync(request.AcademicId, request.Configuration, cancellationToken).ConfigureAwait(false);
            if (entities == null) return Enumerable.Empty<MasterConfigResponse>();

            return entities
                .Where(e => e is not null)
                .Select(e => {
                    var item = e!;
                    return new MasterConfigResponse
                    {
                        ID = item.Id,
                        AcademicId = item.AcademicId,
                        Configuration = item.Configuration,
                        ConfigKey = item.ConfigKey,
                        ConfigValue = item.ConfigValue,
                        Description = item.Description,
                        SortOrder = item.SortOrder,
                        AC_Yr = item.AC_Yr,
                        IsActive = item.IsActive,
                        CreatedDate = item.CreatedDate,
                        CreatedBy = item.CreatedBy
                    };
                })
                .ToList();
        }
    }
}
