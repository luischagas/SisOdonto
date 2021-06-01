using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SisOdonto.Infrastructure.Context;
using SisOdonto.Infrastructure.CrossCutting.Configurations;
using SisOdonto.Infrastructure.CrossCutting.Extensions.Configurations;
using SisOdonto.Infrastructure.CrossCutting.Identity;
using SisOdonto.Infrastructure.CrossCutting.IoC;
using SisOdonto.Infrastructure.CrossCutting.Services.Models;

namespace SisOdonto.App
{
    public partial class Startup
    {
        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Constructors

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfiguration(Configuration);

            services.AddDbContext<SisOdontoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            ConfigureEmail(services);

            services.AddMvcConfiguration();

            services.ResolveDependencies();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseGlobalizationConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }

        private void ConfigureEmailService(IServiceCollection services)
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