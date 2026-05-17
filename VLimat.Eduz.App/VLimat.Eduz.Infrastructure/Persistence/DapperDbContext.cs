using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace VLimat.Eduz.Infrastructure.Persistence
{
    /// <summary>
    /// Lightweight helper to create open IDbConnection instances for Dapper.
    /// Register in DI as a singleton or transient and call CreateConnection / CreateOpenConnectionAsync when needed.
    /// Uses a connection string name (default: "DefaultConnection") from IConfiguration.
    /// </summary>
    public sealed class DapperDbContext : IDisposable
    {
        private readonly string _connectionString;
        private bool _disposed;

        public DapperDbContext(IConfiguration configuration, string connectionStringName)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _connectionString = configuration.GetConnectionString(connectionStringName)
                ?? throw new InvalidOperationException($"Connection string '{connectionStringName}' not found.");
        }

        /// <summary>
        /// Creates a new closed IDbConnection. Caller is responsible for disposing the returned connection.
        /// </summary>
        public IDbConnection CreateConnection()
        {
            EnsureNotDisposed();
            return new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Creates a new open IDbConnection asynchronously. Caller is responsible for disposing the returned connection.
        /// </summary>
        public async Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken = default)
        {
            EnsureNotDisposed();
            var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync(cancellationToken).ConfigureAwait(false);
            return conn;
        }

        private void EnsureNotDisposed()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(DapperDbContext));
        }

        public void Dispose()
        {
            _disposed = true;
            // Nothing to dispose here because connections returned to callers must be disposed by callers.
        }
    }
}
