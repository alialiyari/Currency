using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities
{
    public class UserRoleEntity : IdentityUserRole<long>
    {
        public long Id { get; set; }
        public string ObjectId { get; set; }


        public UserEntity User { get; set; }
        public  RoleEntity Role { get; set; }
    }


    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> entity)
        {
            entity.HasKey(u => u.Id);
            entity.ToTable("UserRoles", "sec");
        }
    }
}
