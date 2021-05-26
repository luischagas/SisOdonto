using Microsoft.Extensions.DependencyInjection;
using SisOdonto.Infrastructure.Context;

namespace SisOdonto.Infrastructure.CrossCutting.IoC
{
    public static class NativeInjector
    {
        #region Methods

        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<SisOdontoContext>();

            return services;
        }

        #endregion Methods
    }
}