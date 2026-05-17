using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace VLimat.Eduz.Infrastructure.Persistence
{
    /// <summary>
    /// Unit-of-work abstraction for Dapper-based work (connection + optional transaction).
    /// Call <see cref="BeginAsync"/> before using <see cref="Connection"/> / <see cref="Transaction"/>.
    /// </summary>
    public interface IDapperUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction? Transaction { get; }
        bool HasOpenConnection { get; } // <-- new

        /// <summary>
        /// Open the connection and optionally start a transaction.
        /// - beginTransaction = true (default) opens a transaction (for writes).
        /// - beginTransaction = false opens a connection but no transaction (useful for batched reads).
        /// </summary>
        Task BeginAsync(bool beginTransaction = true, IsolationLevel? isolationLevel = null, CancellationToken cancellationToken = default);

        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
