using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Features.Masters;

namespace VLimat.Eduz.Domain.Repositories
{
    public interface IEntityMasterConfigRepository
    {
        Task<MasterConfig?> GetAsync(int Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<MasterConfig?>> GetAllAsync(int academicId, string configKey, CancellationToken cancellationToken = default);
        Task<MasterConfig> AddAsync(MasterConfig entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(MasterConfig entity, CancellationToken cancellationToken = default);
    }
}