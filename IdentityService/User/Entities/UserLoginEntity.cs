using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class UserLoginEntity : IdentityUserLogin<long>
    {
        public  UserEntity User { get; set; }
    }


    public class UserLoginEntityConfiguration : IEntityTypeConfiguration<UserLoginEntity>
    {
        public void Configure(EntityTypeBuilder<UserLoginEntity> entity)
        {
            entity.HasKey(l => new {
                l.LoginProvider,
                l.ProviderKey
            });

            // Limit the size of the composite key columns due to common DB restrictions
            entity.Property(l => l.LoginProvider).HasMaxLength(128);
            entity.Property(l => l.ProviderKey).HasMaxLength(128);

            // Maps to the AspNetUserLogins table
            entity.ToTable("UserLogins", "sec");
        }
    }

  
}
