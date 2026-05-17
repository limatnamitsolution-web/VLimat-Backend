using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VLimat.Eduz.Domain.Features.Masters;
using VLimat.Eduz.Domain.Features.Masters.Services;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Infrastructure.Persistence;
using System.Data;

namespace VLimat.Eduz.Infrastructure.Features.MasterConfigs.Services
{
    public class MasterConfigService : IMasterConfigService
    {
        private readonly IDapperUnitOfWork _uow;
        private readonly IMasterConfigRepository _repository;

        public MasterConfigService(IDapperUnitOfWork uow, IMasterConfigRepository repository)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<MasterConfig> AddAsync(MasterConfig entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var startedLocal = false;
            try
            {
                if (_uow.Transaction == null)
                {
                    await _uow.BeginAsync(true, null, cancellationToken).ConfigureAwait(false);
                    startedLocal = true;
                }

                var created = await _repository.AddAsync(entity, cancellationToken).ConfigureAwait(false);

                if (startedLocal)
                {
                    await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                }

                return created;
            }
            catch
            {
                if (startedLocal)
                {
                    try { await _uow.RollbackAsync(cancellationToken).ConfigureAwait(false); } catch { }
                }
                throw;
            }
        }

        public async Task UpdateAsync(MasterConfig entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var startedLocal = false;
            try
            {
                if (_uow.Transaction == null)
                {
                    await _uow.BeginAsync(true, null, cancellationToken).ConfigureAwait(false);
                    startedLocal = true;
                }

                await _repository.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

                if (startedLocal)
                {
                    await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            catch
            {
                if (startedLocal)
                {
                    try { await _uow.RollbackAsync(cancellationToken).ConfigureAwait(false); } catch { }
                }
                throw;
            }
        }

        public async Task<IEnumerable<MasterConfig?>> GetAllAsync(int academicId, string configuration, CancellationToken cancellationToken = default)
        {
            var startedLocal = false;
            try
            {
                // ensure a connection is available. If an outer scope already started the UoW, Transaction!=null or Connection.State==Open.
                //if (_uow.Connection == null || _uow.Connection.State != ConnectionState.Open)
                if (!_uow.HasOpenConnection)
                {
                    // open connection only (no DB transaction) for read-only work
                    await _uow.BeginAsync(false, null, cancellationToken).ConfigureAwait(false);
                    startedLocal = true;
                }

                // repository should detect active connection and use _uow.Connection
                var results = await _repository.GetAllAsync(academicId, configuration, cancellationToken).ConfigureAwait(false);

                if (startedLocal)
                {
                    // CommitAsync will just close the connection when there is no transaction
                    await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                }

                return results;
            }
            catch
            {
                if (startedLocal)
                {
                    try { await _uow.RollbackAsync(cancellationToken).ConfigureAwait(false); } catch { }
                }
                throw;
            }
        }

        public async Task<MasterConfig?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var startedLocal = false;
            try
            {
                // Ensure a connection is available. If an outer scope already started the UoW, Transaction!=null or Connection.State==Open.
                // if (_uow.Connection == null || _uow.Connection.State != ConnectionState.Open)
                if (!_uow.HasOpenConnection)
                {
                    // Open connection only (no DB transaction) for read-only work
                    await _uow.BeginAsync(false, null, cancellationToken).ConfigureAwait(false);
                    startedLocal = true;
                }

                // Repository should detect active connection and use _uow.Connection
                var result = await _repository.GetAsync(id, cancellationToken).ConfigureAwait(false);

                if (startedLocal)
                {
                    // CommitAsync will just close the connection when there is no transaction
                    await _uow.CommitAsync(cancellationToken).ConfigureAwait(false);
                }

                return result;
            }
            catch
            {
                if (startedLocal)
                {
                    try { await _uow.RollbackAsync(cancellationToken).ConfigureAwait(false); } catch { }
                }
                throw;
            }
        }


        // Example composition (multiple repo calls in one transaction):
        // public async Task AddBothAsync(MasterConfig a, AnotherEntity b, CancellationToken cancellationToken = default)
        // {
        //     await _uow.BeginAsync(cancellationToken);
        //     try
        //     {
        //         await _repository.AddAsync(a, cancellationToken);
        //         await _otherRepository.AddAsync(b, cancellationToken); // both use same _uow.Connection/_uow.Transaction
        //         await _uow.CommitAsync(cancellationToken);
        //     }
        //     catch
        //     {
        //         await _uow.RollbackAsync(cancellationToken);
        //         throw;
        //     }
        // }
    }
}