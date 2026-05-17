using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Features.Masters;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Infrastructure.Persistence;
using VLimat.Eduz.Domain.Security;

namespace VLimat.Eduz.Infrastructure.Features.MasterConfigs.Repositories
{
    public class MasterConfigRepository : IMasterConfigRepository
    {
        private readonly IDapperUnitOfWork _uow;
        private readonly ICurrentUser _currentUser;

        public MasterConfigRepository( IDapperUnitOfWork uow, ICurrentUser currentUser)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        }

        public async Task<MasterConfig?> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            const string sp = "mst.usp_vklmt_MasterConfigs_GetById";

            try
            {
                // Use the unit-of-work connection. Caller is responsible for calling BeginAsync() before using the connection.
                return await _uow.Connection.QueryFirstOrDefaultAsync<MasterConfig>(
                    new CommandDefinition(sp, new { Id }, _uow.Transaction, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));
            }
            catch (InvalidOperationException ex) when (ex.Message?.Contains("BeginAsync") == true)
            {
                throw new InvalidOperationException("Unit of work not started. Call IDapperUnitOfWork.BeginAsync() before performing read operations.", ex);
            }
        }

        public async Task<IEnumerable<MasterConfig?>> GetAllAsync(int academicId, string configuration, CancellationToken cancellationToken = default)
        {
            const string sp = "mst.usp_vklmt_MasterConfigs_GetAll";

            var academic = academicId != default ? academicId : _currentUser.AcademicId;

            try
            {
                // Use the unit-of-work connection. Transaction may be null for read-only calls.
                var results = await _uow.Connection.QueryAsync<MasterConfig>(
                    new CommandDefinition(sp, new { AcademicId = academic, Configuration = configuration }, _uow.Transaction, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken));
                return results.Cast<MasterConfig?>().ToList();
            }
            catch (InvalidOperationException ex) when (ex.Message?.Contains("BeginAsync") == true)
            {
                throw new InvalidOperationException("Unit of work not started. Call IDapperUnitOfWork.BeginAsync() before performing read operations.", ex);
            }
        }

        public async Task<MasterConfig> AddAsync(MasterConfig entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            const string sp = "mst.usp_vklmt_MasterConfigs_Insert";

            // Enforce caller started UoW for writes
            if (_uow.Transaction == null)
                throw new InvalidOperationException("Unit of work not started. Call IDapperUnitOfWork.BeginAsync() before performing write operations.");

            var id = await _uow.Connection.QuerySingleAsync<int>(
                new CommandDefinition(sp, new
                {
                    AcademicId = _currentUser.AcademicId,
                    Configuration = entity.Configuration,
                    ConfigKey = entity.ConfigKey,
                    ConfigValue = entity.ConfigValue,
                    Description = entity.Description,
                    SortOrder = entity.SortOrder,
                    AC_Yr = entity.AC_Yr,
                    IsActive = entity.IsActive,
                    CreatedDate = entity.CreatedDate,
                    CreatedBy = entity.CreatedBy,
                    ModifiedDate = entity.ModifiedDate,
                    ModifiedBy = entity.ModifiedBy
                }, _uow.Transaction, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);

            entity.Id = id;
            return entity;
        }

        public async Task UpdateAsync(MasterConfig entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            const string sp = "mst.usp_vklmt_MasterConfigs_Update";

            if (_uow.Transaction == null)
                throw new InvalidOperationException("Unit of work not started. Call IDapperUnitOfWork.BeginAsync() before performing write operations.");

            await _uow.Connection.ExecuteAsync(
                new CommandDefinition(sp, new
                {
                    Id = entity.Id,
                    AcademicId = _currentUser.AcademicId,
                    Configuration = entity.Configuration,
                    ConfigKey = entity.ConfigKey,
                    ConfigValue = entity.ConfigValue,
                    Description = entity.Description,
                    SortOrder = entity.SortOrder,
                    AC_Yr = entity.AC_Yr,
                    IsActive = entity.IsActive,
                    ModifiedDate = entity.ModifiedDate,
                    ModifiedBy = entity.ModifiedBy
                }, _uow.Transaction, commandType: CommandType.StoredProcedure, cancellationToken: cancellationToken)).ConfigureAwait(false);
        }
    }
}