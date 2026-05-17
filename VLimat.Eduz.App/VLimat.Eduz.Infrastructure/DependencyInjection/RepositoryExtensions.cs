using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VLimat.Eduz.Domain.Repositories;
using VLimat.Eduz.Domain.Security;
using VLimat.Eduz.Infrastructure.Features.MasterConfigs.Repositories;
using VLimat.Eduz.Infrastructure.Persistence;
using VLimat.Eduz.Infrastructure.Security;

namespace VLimat.Eduz.Infrastructure.DependencyInjection
{
    public static class InfrastructureExtensions
    {

        public static IServiceCollection AddInfrastructureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(connectionString));
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string is null or empty");
            }
            services.AddScoped<IUserContext, UserContext>();
            // Register domain-level current user abstraction with infra implementation
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IEntityMasterConfigRepository, EfMasterConfigRepository>();        
            // register implementation in Startup / Program.cs (in composition root project)
            services.AddScoped<VLimat.Eduz.Domain.Features.Masters.Services.IMasterConfigService, VLimat.Eduz.Infrastructure.Features.MasterConfigs.Services.MasterConfigService>();
            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();
            services.AddScoped<IMasterConfigRepository, MasterConfigRepository>();
            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return new DapperDbContext(config, "DapperConnection");
            });

            services.AddScoped<IDapperUnitOfWork, DapperUnitOfWork>();
           
            return services;
        }
    }
}