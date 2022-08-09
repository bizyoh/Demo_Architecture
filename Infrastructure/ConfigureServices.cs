using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Infrastructure.Files.Maps;
using Infrastructure.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class ConfigureServices
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
     
        var connectionStringPostgres = configuration["ConnectionStrings:PostgreSQLConnection"];
        //if(serverSetting.DatabaseProvider.Equals("SQLServer"))
        if (configuration.GetValue<string>("DatabaseProvider").Equals("SQLServer"))
        {
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]));
        }
        else
        {
            services.AddDbContext<AppDBContext>(options => options.UseNpgsql(connectionStringPostgres));
        }

        services.AddHttpContextAccessor();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDBContext>());
        services.AddIdentityCore<User>().AddRoles<Role>().AddEntityFrameworkStores<AppDBContext>();
        services.AddAutoMapper(typeof(RoleMapper));
        services.AddAutoMapper(typeof(UserMapper));
        services.AddHttpContextAccessor();
        return services;
    }
}
