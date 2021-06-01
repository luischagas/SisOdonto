using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SisOdonto.Infrastructure.CrossCutting.Services.Models;

namespace SisOdonto.App
{
    public partial class Startup
    {
        #region Methods

        private void ConfigureEmail(IServiceCollection services)
        {
            var emailSettings = new EmailSettings();

            Configuration.GetSection("EmailSettings").Bind(emailSettings);

            services.Configure<EmailSettings>(
                Configuration.GetSection("EmailSettings"));

            services.AddSingleton(ms =>
                ms.GetRequiredService<IOptions<EmailSettings>>().Value);
        }

        #endregion Methods
    }
}