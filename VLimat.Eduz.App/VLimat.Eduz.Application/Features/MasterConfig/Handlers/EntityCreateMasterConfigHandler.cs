using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Application.Features.MasterConfig.Commands;
using VLimat.Eduz.Domain.Features.Masters;
using VLimat.Eduz.Application.Features.MasterConfig.DTOs;

namespace VLimat.Eduz.Application.Features.MasterConfig.Handlers
{
    public class EntityCreateMasterConfigHandler : IRequestHandler<EntityCreateCommand, MasterConfigResponse>
    {
        private readonly IEntityMasterConfigRepository _repo;

        public EntityCreateMasterConfigHandler(IEntityMasterConfigRepository repo) => _repo = repo;

        public async Task<MasterConfigResponse> Handle(EntityCreateCommand request, CancellationToken cancellationToken)
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
                IsActive = true, // Set default values as needed
                CreatedDate = DateTime.UtcNow
            };

            var added = await _repo.AddAsync(entity, cancellationToken);

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