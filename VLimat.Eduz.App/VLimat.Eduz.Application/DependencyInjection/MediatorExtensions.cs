
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VLimat.Eduz.Application.Features.MasterConfig.Handlers;
using VLimat.Eduz.Application.Features.MasterConfig.Providers;

namespace VLimat.Eduz.Application.DependencyInjection
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddApplicationMediator(this IServiceCollection services)
        {
            // Registers MediatR handlers found in this assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient<IProviderMasterConfigFactory, ProviderMasterConfigFactory>();

            // If you want to wire your pipeline behaviors from the Application / Infrastructure assemblies,
            // register them here (the types already exist in the project):
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Common.Behaviors.ApplicationExceptionBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Infrastructure.Common.Behaviors.InfrastructureExceptionBehavior<,>));

            return services;
        }
    }
}