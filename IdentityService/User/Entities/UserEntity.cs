using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class UserEntity : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ShSh { get; set; }

        public string NationalCode { get; set; }
        public string NationalCardUrl { get; set; }



        public string ImageUrl { get; set; }
        public long? IntroducerUserId { get; set; }
        public UserEntity IntroducerUser { get; set; }


        public ActiveStatusEnum ActiveStatus { get; set; }
        public VerifyStatusEnum VerifyStatus { get; set; }
        
        public string FatherName { get; set; }
        public GenderEnum? Gender { get; set; }



        public string Address { get; set; }
        public string PostalCode { get; set; }


        public DateTime? BirthDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? LastLoginDate { get; set; }



        public virtual ICollection<UserClaimEntity> Claims { get; set; }
        public virtual ICollection<UserLoginEntity> Logins { get; set; }
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; }
    }

    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> entity)
        {

            entity.HasKey(u => u.Id);
            // Indexes for "normalized" username and email, to allow efficient lookups
            entity.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex").IsUnique();
            entity.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();

            entity.ToTable("Users", "sec");

            // A concurrency token for use with the optimistic concurrency checking
            entity.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            entity.Property(u => u.UserName).HasMaxLength(256);
            entity.Property(u => u.NormalizedUserName).HasMaxLength(256);
            entity.Property(u => u.Email).HasMaxLength(256);
            entity.Property(u => u.NormalizedEmail).HasMaxLength(256);


            entity.HasMany(x => x.Claims).WithOne(x => x.User).HasForeignKey(uc => uc.UserId).IsRequired();
            entity.HasMany(x => x.Logins).WithOne(x => x.User).HasForeignKey(ul => ul.UserId).IsRequired();
            entity.HasMany(x => x.UserRoles).WithOne(x => x.User).HasForeignKey(ur => ur.UserId).IsRequired();
        }
    }
}
