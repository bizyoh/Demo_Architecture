using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    public DbSet<User> Users { get; }
    public DbSet<Role> Roles { get; }
    public DbSet<Product> Products { get;  }
    public DbSet<Category> Categories { get; }
    public DbSet<Invoice> Invoices { get;  }
    public DbSet<InvoiceDetail> InvoiceDetails { get; }
 //   Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
