using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class RoleEntity : IdentityRole<long>
    {
        public string Title { get; set; }


        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
        public virtual ICollection<RoleClaimEntity> RoleClaims { get; set; }



    }


    public class RoleEntityConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> entity)
        {
            entity.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            entity.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

            // Maps to the AspNetRoles table
            entity.ToTable("Roles", "sec");

            // A concurrency token for use with the optimistic concurrency checking
            entity.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            entity.Property(u => u.Name).HasMaxLength(256);
            entity.Property(u => u.NormalizedName).HasMaxLength(256);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            //entity.HasMany<UserRoleEntity>().WithOne(x=> x.Role).HasForeignKey(ur => ur.RoleId).IsRequired();
            entity.HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(ur => ur.RoleId).IsRequired();

            // Each Role can have many associated RoleClaims
            //entity.HasMany<RoleClaimEntity>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
            entity.HasMany(x => x.RoleClaims).WithOne(x => x.Role).HasForeignKey(rc => rc.RoleId).IsRequired();
        }
    }
}
