using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.CartService.Models;

namespace WorldOfPowerTools.CartService.Data
{
    public class DbCartLineRepository
    {
        protected readonly WoptCartServiceDb DbContext;
        protected readonly DbSet<CartLine> Set;
        protected virtual IQueryable<CartLine> Items => Set;

        public DbCartLineRepository(WoptCartServiceDb context)
        {
            DbContext = context;
            Set = context.Set<CartLine>();
        }

        public async Task<IEnumerable<CartLine>> GetAllAsync(int skip = 0, int? take = null)
        {
            int skipCount = skip < 0 ? 0 : skip;
            var items = Items.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<CartLine?> GetByIdAsync(Guid id)
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

        public async Task<CartLine> SaveAsync(CartLine entity)
        {
            var storedEntity = await Set.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (storedEntity == null)
                await Set.AddAsync(entity);
            else
                Set.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<CartLine>> GetByUserIdAsync(Guid userId)
        {
            return await Items.Where(line => line.UserId == userId).ToListAsync();
        }

        public async Task<int> RemoveByProductIdAsync(Guid productId)
        {
            var cartLines = Set.Where(x => x.ProductId == productId);
            return await RemoveRange(cartLines);
        }

        public async Task<int> RemoveByUserIdAsync(Guid userId)
        {
            var cartLines = Set.Where(x => x.UserId == userId);
            return await RemoveRange(cartLines);
        }

        private async Task<int> RemoveRange(IQueryable<CartLine> cartLines)
        {
            if (!cartLines.Any()) return 0;
            Set.RemoveRange(cartLines);
            await DbContext.SaveChangesAsync();
            return cartLines.Count();
        }
    }
}