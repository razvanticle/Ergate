namespace Ergate.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    ///     A unit of work that allows to modify and save entities in the database
    /// </summary>
    public interface IUnitOfWork : IRepository, IDisposable
    {
        /// <summary>
        ///     Saves the changes that were done on the entities on the current unit of work
        /// </summary>
        void SaveChanges();

        /// <summary>
        ///     Saves the changes that were done on the entities on the current unit of work
        /// </summary>
        Task SaveChangesAsync();

        /// <summary>
        ///     Adds to the current unit of work a new entity of type T
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">The entity to be added</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        ///     Deletes from the current unit of work an entity of type T
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">The entity to be deleted</param>
        void Delete<T>(T entity) where T : class;


        /// <summary>
        ///     Begins a TransactionScope with specified isolation level
        /// </summary>
        void BeginTransactionScope(SimplifiedIsolationLevel isolationLevel);

        /// <summary>
        /// Gets the <see cref="IEntityEntry{T}"/> for the given entity
        /// </summary>
        /// <exception cref="ArgumentNullException"><see cref="ArgumentNullException"/> is thrown when <param name="entity"> is <c>null</c></param></exception>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">the entity instance</param>
        /// <returns>and <see cref="IEntityEntry{T}"/>instance for the given entity</returns>
        IEntityEntry<T> GetEntityEntry<T>(T entity)
            where T : class;

        /// <summary>
        /// Returns all the entity entries tracked by this context
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEntityEntry> GetEntityEntries();
    }
}