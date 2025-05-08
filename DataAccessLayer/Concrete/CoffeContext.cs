using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EntityLayer.Concrete;

namespace DataAccessLayer.Concrete
{
    // IdentityDbContext<AppUser> => özelleştirilmiş kullanıcı sınıfın
    public class CoffeContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public CoffeContext(DbContextOptions<CoffeContext> options)
            : base(options)
        {
        }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<TokenInfo> TokenInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Burada gerekli ilişkileri ve constraints'leri belirleyebilirsiniz
        }
    }
}
