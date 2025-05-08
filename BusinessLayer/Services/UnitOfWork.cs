using DataAccessLayer.Concrete;
using DataAccessLayer.Repository;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
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
