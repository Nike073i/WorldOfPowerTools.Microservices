using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.UserService.Models;

namespace WorldOfPowerTools.UserService.Data
{
    public class DbUserRepository
    {
        protected readonly WoptUserServiceDb DbContext;
        protected readonly DbSet<User> Set;
        protected virtual IQueryable<User> Items => Set;

        public DbUserRepository(WoptUserServiceDb context)
        {
            DbContext = context;
            Set = context.Set<User>();
        }

        public async Task<IEnumerable<User>> GetAllAsync(int skip = 0, int? take = null)
        {
            int skipCount = skip < 0 ? 0 : skip;
            var items = Items.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(Guid id)
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

        public async Task<User> SaveAsync(User entity)
        {
            var storedEntity = await Set.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (storedEntity == null)
                await Set.AddAsync(entity);
            else
                Set.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await Items.FirstOrDefaultAsync(user => user.Login == login);
        }
    }
}