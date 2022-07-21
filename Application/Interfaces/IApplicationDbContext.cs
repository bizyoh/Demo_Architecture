using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Role> Roles { get; }
    public DbSet<Product> Products { get;  }
    public DbSet<Category> Categories { get; }
    public DbSet<Invoice> Invoices { get;  }
    public DbSet<InvoiceDetail> InvoiceDetails { get; }
    public DatabaseFacade Database { get; }
    public int SaveChanges();
    public  EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
