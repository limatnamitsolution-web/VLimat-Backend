
using MediatR;
using VLimat.Eduz.Application.Features.MasterConfig.Queries;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;

namespace VLimat.Eduz.Application.Features.MasterConfig.Handlers
{
    public class EntityGetMasterConfigHandler : IRequestHandler<EntityGetMasterConfigQuery, MasterConfigResponse?>
    {
        private readonly IEntityMasterConfigRepository _repo;

        public EntityGetMasterConfigHandler(IEntityMasterConfigRepository repo) => _repo = repo;

        public async Task<MasterConfigResponse?> Handle(EntityGetMasterConfigQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetAsync(request.Id, cancellationToken);
            if (entity == null) return null;

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