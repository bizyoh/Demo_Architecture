using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configurations
{
    public class UserConfiguration: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1).ValueGeneratedOnAdd();
            builder.Property(x => x.UserName).HasMaxLength(15);
            builder.Property(x => x.FirstName).HasMaxLength(15);
            builder.Property(x => x.LastName).HasMaxLength(15);
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.Property(x => x.PhoneNumber).HasMaxLength(15);
            builder.Property(x => x.RefreshToken).IsRequired(false);
        }
    }
}
