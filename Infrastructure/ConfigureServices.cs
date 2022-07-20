using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Files.Maps;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class ConfigureServices
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringPostgres = configuration["ConnectionStrings:PostgreSQLConnection"];
        if (configuration.GetValue<string>("DatabaseProvider").Equals("SQLServer"))
        {
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(connectionStringPostgres));
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
        services.AddScoped<SignInManager<User>>();
        //services.AddTransient<IUserService, UserService>();
        //services.AddTransient<IInvoiceService, InvoiceService>();
        //services.AddTransient<IRoleService, RoleService>();
        //services.AddTransient<IProductService, ProductService>();
        //services.AddTransient<ICategoryService, CategoryService>();
        services.AddHttpContextAccessor();
        return services;
    }
}
