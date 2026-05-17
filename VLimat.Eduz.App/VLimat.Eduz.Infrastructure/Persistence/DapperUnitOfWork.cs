using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace VLimat.Eduz.Infrastructure.Persistence
{
    /// <summary>
    /// Simple Dapper unit-of-work: opens a connection, optionally begins a transaction and exposes commit/rollback.
    /// Disposal will clean up connection and transaction.
    /// </summary>
    public sealed class DapperUnitOfWork : IDapperUnitOfWork
    {
        private readonly DapperDbContext _db;
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;

        public DapperUnitOfWork(DapperDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        public IDbConnection Connection => _connection ?? throw new InvalidOperationException("Call BeginAsync before using Connection. ");
        public IDbTransaction? Transaction => _transaction;
        public bool HasOpenConnection => _connection != null && _connection.State == ConnectionState.Open;

        /// <summary>
        /// Open connection and optionally begin transaction.
        /// If beginTransaction==false a connection is opened but no transaction is created.
        /// </summary>
        public async Task BeginAsync(bool beginTransaction = true, IsolationLevel? isolationLevel = null, CancellationToken cancellationToken = default)
        {
            if (_connection != null)
                throw new InvalidOperationException("UnitOfWork already started for this instance.");

            _connection = await _db.CreateOpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            if (beginTransaction)
            {
                _transaction = isolationLevel.HasValue
                    ? _connection.BeginTransaction(isolationLevel.Value)
                    : _connection.BeginTransaction();
            }
            else
            {
                _transaction = null;
            }
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            // If a transaction exists commit it; then always dispose connection.
            if (_transaction != null)
            {
                try
                {
                    _transaction.Commit();
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                    _connection?.Close();
                    _connection?.Dispose();
                    _connection = null;
                }
            }
            else
            {
                // No transaction: just close and dispose connection (read-only usage)
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }

            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Rollback();
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                    _connection?.Close();
                    _connection?.Dispose();
                    _connection = null;
                }
            }
            else
            {
                // No transaction — ensure connection is closed if opened
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed) return;
            try
            {
                if (_transaction != null)
                {
                    try { _transaction.Rollback(); } catch { /* swallow */ }
                    _transaction.Dispose();
                    _transaction = null;
                }
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
