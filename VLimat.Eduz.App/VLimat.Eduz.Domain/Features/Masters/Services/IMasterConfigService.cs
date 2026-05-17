using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Features.Masters;

namespace VLimat.Eduz.Domain.Features.Masters.Services
{
    public interface IMasterConfigService
    {
        Task<MasterConfig> AddAsync(MasterConfig entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(MasterConfig entity, CancellationToken cancellationToken = default);
        Task<MasterConfig?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<MasterConfig?>> GetAllAsync(int academicId, string configuration, CancellationToken cancellationToken = default);
    }
}