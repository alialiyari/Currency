using Entities;
using Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

public partial class DatabaseContext : IdentityDbContext<UserEntity, RoleEntity, long, UserClaimEntity, UserRoleEntity, UserLoginEntity, RoleClaimEntity, UserTokenEntity>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }
     

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }



    public override int SaveChanges()
    {
        this.CleanString();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.CleanString();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        this.CleanString();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.CleanString();
        return base.SaveChangesAsync(cancellationToken);
    }
}


public class DataBaseCommandInterceptor : DbCommandInterceptor
{

    public DataBaseCommandInterceptor()
    {

    }
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        //command.CommandText = command.CommandText
        //       .Replace("[sec]", $"[{IdentityDBName}].[sec]")
        //       ;
        return base.ReaderExecuting(command, eventData, result);
    }
    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        // command.CommandText = command.CommandText.Replace("[sec]", $"[{IdentityDBName}].[sec]");
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }
}