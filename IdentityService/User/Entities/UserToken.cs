using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class UserTokenEntity : IdentityUserToken<long>
    {
        public  UserEntity User { get; set; }
    }

    public class UserTokenEntityConfiguration : IEntityTypeConfiguration<UserTokenEntity>
    {
        public void Configure(EntityTypeBuilder<UserTokenEntity> entity)
        {
            // Composite primary key consisting of the UserId, LoginProvider and Name
            entity.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            // Limit the size of the composite key columns due to common DB restrictions
            //b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
            //b.Property(t => t.Name).HasMaxLength(maxKeyLength);

            // Maps to the AspNetUserTokens table
            entity.ToTable("UserTokens", "sec");
        }
    }
}
