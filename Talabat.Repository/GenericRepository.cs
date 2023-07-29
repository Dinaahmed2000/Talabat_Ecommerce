using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)  //ask clr for creating object from dbContext (implicitly)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if (typeof(T) == typeof(Product))
            //{
            //    return (IEnumerable<T>) await _dbContext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            //}
            //else
            //{
                return await _dbContext.Set<T>().ToListAsync();
            //}
        }
        public async Task<T> GetByIdAsync(int id)
        {
            // _dbContext.Set<T>().Where(x=>x.Id == id).Include(p => p.ProductBrand).Include(p => p.ProductType);  
            return await _dbContext.Set<T>().FindAsync(id);
        }
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await applySpecification(spec).ToListAsync();
        }

        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await applySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetByCountWithSpecAsync(ISpecification<T> spec)
        {
            return await applySpecification(spec).CountAsync();
        }
        private IQueryable<T> applySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task Add(T entity)
        {
           await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
             _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
