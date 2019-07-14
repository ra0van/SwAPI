using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Repository
{
    using SW.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// Interface IRepository
    /// </summary>
    /// <typeparam name="T"><see cref="SW.Repository.BaseEntity" />Base entity.</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets the entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns><see cref="SW.Repository.BaseEntity" />Base entity.</returns>
        T GetById(int id);

        /// <summary>
        /// Gets entities.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="size">The size of entities.</param>
        /// <returns>ICollection&lt; <see cref="SW.Repository.BaseEntity" /> &gt;.</returns>
        ICollection<T> GetEntities(int page = 1, int size = 10);

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        ICollection<T> GetAllEntities();
    }
}
