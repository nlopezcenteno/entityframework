using NSubstitute;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace UnitTesting.Tests
{
    class EntityFrameworkTestHelpers
    {
        /// <summary>
        ///     Mocks a DBSet and initialize it with data, using EntityFramework.Testing.NSubstitute
        ///     "https://github.com/scott-xu/EntityFramework.Testing#entityframeworktestingnsubstitute--"
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static DbSet<TEntity> MockDbSet<TEntity>(ICollection<TEntity> data = null) where TEntity : class
        {
            var dbSet = Substitute.For<DbSet<TEntity>, IQueryable<TEntity>, IDbAsyncEnumerable<TEntity>>();
            if (data != null)
            {
                dbSet.SetupData(data);
            }
            return dbSet;
        }
    }
}
