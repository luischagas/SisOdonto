using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SisOdonto.Infrastructure.Context
{
    public class SisOdontoIdentityContext : IdentityDbContext
    {
        #region Constructors

        public SisOdontoIdentityContext(DbContextOptions<SisOdontoIdentityContext> options)
            : base(options)
        {
        }

        #endregion Constructors
    }
}