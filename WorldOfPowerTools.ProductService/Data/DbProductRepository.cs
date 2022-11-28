using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.ProductService.Models;

namespace WorldOfPowerTools.ProductService.Data
{
    public class DbProductRepository
    {
        protected readonly WoptProductServiceDb DbContext;
        protected readonly DbSet<Product> Set;
        protected virtual IQueryable<Product> Items => Set;

        public DbProductRepository(WoptProductServiceDb context)
        {
            DbContext = context;
            Set = context.Set<Product>();
        }

        public async Task<IEnumerable<Product>> GetAllAsync(int skip = 0, int? take = null)
        {
            int skipCount = skip < 0 ? 0 : skip;
            var items = Items.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await Items.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid> RemoveByIdAsync(Guid id)
        {
            var entity = await Set.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException("Ошибка удаления по Id: Запись не найдена");
            DbContext.Entry(entity).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync();
            return id;
        }

        public async Task<Product> SaveAsync(Product entity)
        {
            var storedEntity = await Set.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (storedEntity == null)
                await Set.AddAsync(entity);
            else
                Set.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Category category, int skip = 0, int? take = null)
        {
            var itemsByCategories = Items.Where(product => product.Category == category);
            int skipCount = skip < 0 ? 0 : skip;
            var items = itemsByCategories.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<Product>> SaveRangeAsync(IEnumerable<Product> products)
        {
            using var transaction = DbContext.Database.BeginTransaction();
            {
                try
                {
                    foreach (var product in products)
                        await SaveAsync(product);
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return products;
                }
                catch
                {
                    transaction.Rollback();
                    return Enumerable.Empty<Product>();
                }
            }
        }
    }
}