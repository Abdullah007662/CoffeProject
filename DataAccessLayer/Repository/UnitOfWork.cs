using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeContext _context;
        private IRepository<UserActivity> _userActivities;
        private IRepository<TokenInfo> _tokenInfos;

        public UnitOfWork(CoffeContext context)
        {
            _context = context;
        }

        public IRepository<UserActivity> UserActivities =>
            _userActivities ??= new Repository<UserActivity>(_context);

        public IRepository<TokenInfo> TokenInfos =>
            _tokenInfos ??= new Repository<TokenInfo>(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
