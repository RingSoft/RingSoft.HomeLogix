using System.Collections.Generic;
using System.Linq;

namespace RingSoft.HomeLogix.DataAccess
{
    public interface IDbContext : DbLookup.IDbContext
    {
        bool SaveNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class, new();

        bool SaveEntity<TEntity>(TEntity entity, string message) where TEntity : class, new();

        bool DeleteEntity<TEntity>(TEntity entity, string message) where TEntity : class, new();

        bool AddNewNoCommitEntity<TEntity>(TEntity entity, string message) where TEntity : class, new();

        bool Commit(string message);

        void RemoveRange<TEntity>(IEnumerable<TEntity> listToRemove) where TEntity : class, new();

        void AddRange<TEntity>(List<TEntity> listToAdd) where TEntity : class, new();

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class, new();
    }
}
