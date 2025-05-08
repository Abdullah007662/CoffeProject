using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<UserActivity> UserActivities { get; }
        IRepository<TokenInfo> TokenInfos { get; }
        Task SaveChangesAsync();
    }
}


