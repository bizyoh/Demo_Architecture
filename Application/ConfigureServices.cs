﻿using Application.Interfaces;
using Application.Interfaces.CategoryService;
using Application.Interfaces.InvoiceService;
using Application.Interfaces.ProductService;
using Application.Interfaces.RoleService;
using Application.Interfaces.UserService;
using Application.Services;
using Domain.Entities;
using Infrastructure.Files.Maps;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class ConfigureServices
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpContextAccessor();
        services.AddAutoMapper(typeof(RoleMapper));
        services.AddAutoMapper(typeof(UserMapper));
        services.AddAutoMapper(typeof(InvoiceMapper));
        services.AddAutoMapper(typeof(CategoryMapper));
        services.AddAutoMapper(typeof(ProductMapper));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<SignInManager<User>>();
        services.AddTransient<IInvoiceService, InvoiceService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IServerService, ServerService>();
        services.AddHttpContextAccessor();
        return services;
    }
}
