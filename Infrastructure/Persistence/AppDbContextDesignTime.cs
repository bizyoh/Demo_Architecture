﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDbContextDesignTime : IDesignTimeDbContextFactory<AppDBContext>
    {
        public AppDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json")
                                .Build();

            var connectionStringSQL = configuration.GetConnectionString("DefaultConnection");
            var connectionStringPostgres = configuration["ConnectionStrings:PostgreSQLConnection"];

            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();

            if (configuration.GetValue<string>("DatabaseProvider").Equals("SQLServer"))
            {
                optionsBuilder.UseSqlServer(connectionStringSQL);
            }
            else
            {
                optionsBuilder.UseNpgsql(connectionStringPostgres);
            }

            return new AppDBContext(optionsBuilder.Options);
        }
    }
}
