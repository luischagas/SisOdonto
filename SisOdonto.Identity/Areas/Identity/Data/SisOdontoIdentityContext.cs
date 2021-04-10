using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SisOdonto.Identity.Areas.Identity.Data
{
    public class SisOdontoIdentityContext : IdentityDbContext<IdentityUser>
    {
        #region Constructors

        public SisOdontoIdentityContext(DbContextOptions<SisOdontoIdentityContext> options)
            : base(options)
        {
        }

        #endregion Constructors

        #region Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        #endregion Methods
    }
}