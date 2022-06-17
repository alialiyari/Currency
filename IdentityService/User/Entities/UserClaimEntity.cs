using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class UserClaimEntity : IdentityUserClaim<long>
    {
        public  UserEntity User { get; set; }
    }


    public class UserClaimEntityConfiguration : IEntityTypeConfiguration<UserClaimEntity>
    {
        public void Configure(EntityTypeBuilder<UserClaimEntity> entity)
        {
            entity.ToTable("UserClaims", "sec");
            // Primary key
            entity.HasKey(uc => uc.Id);
        }
    }

}
