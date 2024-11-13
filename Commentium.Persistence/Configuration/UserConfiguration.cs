using Commentium.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentium.Persistence.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(u => u.UserName)
                .HasMaxLength(50);

            builder.Property(e => e.Email)
                .HasMaxLength(60);

            builder.HasIndex(e => e.Email)
                .IsUnique();
        }
    }
}
