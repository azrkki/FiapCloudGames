using FCG.Core.Entity;
using FCG.Core.Entity.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infrastructure.Repository.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseIdentityColumn();
            builder.Property(p => p.Name).HasColumnType("VARCHAR(100)");
            builder.Property(p => p.Email).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.Password)
                .HasColumnType("VARCHAR(255)")
                .IsRequired()
                .HasConversion(
                    password => password.Value,
                    hashedValue => Password.FromHashedValue(hashedValue));
        }
    }
}
