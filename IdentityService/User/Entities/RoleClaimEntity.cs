using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class RoleClaimEntity : IdentityRoleClaim<long>
    {
        public  RoleEntity Role { get; set; }
    }

    public class RoleClaimEntityConfiguration : IEntityTypeConfiguration<RoleClaimEntity>
    {
        public void Configure(EntityTypeBuilder<RoleClaimEntity> entity)
        {
            // Primary key
            entity.HasKey(rc => rc.Id);

            // Maps to the AspNetRoleClaims table
            entity.ToTable("RoleClaims", "sec");
        }
    }
}
