using Microsoft.Extensions.DependencyInjection;
using SisOdonto.Application.Interfaces;
using SisOdonto.Application.Services;
using SisOdonto.Domain.Interfaces.Notification;
using SisOdonto.Domain.Interfaces.Repositories;
using SisOdonto.Domain.Interfaces.Services;
using SisOdonto.Domain.Notification;
using SisOdonto.Domain.Shared;
using SisOdonto.Infrastructure.Context;
using SisOdonto.Infrastructure.CrossCutting.Services;
using SisOdonto.Infrastructure.Repositories;

namespace SisOdonto.Infrastructure.CrossCutting.IoC
{
    public static class NativeInjector
    {
        #region Methods

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<SisOdontoContext>();
            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IDentistRepository, DentistRepository>();
            services.AddScoped<IDentistService, DentistService>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }

        #endregion Methods
    }
}