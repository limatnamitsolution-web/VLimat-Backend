using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Features.Masters;

namespace VLimat.Eduz.Domain.Repositories
{
    public interface IMasterConfigRepository
    {
        Task<MasterConfig?> GetAsync(int Id, CancellationToken cancellationToken = default);
        Task<IEnumerable<MasterConfig?>> GetAllAsync(int academicId, string configKey, CancellationToken cancellationToken = default);
        Task<MasterConfig> AddAsync(MasterConfig entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(MasterConfig entity, CancellationToken cancellationToken = default);
    }
}
