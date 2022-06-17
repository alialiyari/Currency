using Common;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class RoleRemoveCommand : CommandRequest<OperationResult>
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? RoleId { get; set; }
    }

    class RoleRemoveCommandHandler : IRequestHandler<RoleRemoveCommand, OperationResult>
    {

        private readonly DatabaseContext adminDbContext;


        public RoleRemoveCommandHandler(DatabaseContext adminDbContext)
        {
            this.adminDbContext = adminDbContext;
        }

        public async Task<OperationResult> Handle(RoleRemoveCommand request, CancellationToken cancellationToken)
        {
            UserRoleEntity finded = null;

            if (request.Id.HasValue)
                finded = await adminDbContext.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (finded == null)
                finded = await adminDbContext.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId, cancellationToken: cancellationToken);


            if (finded == null) return OperationResult.BuildFailure("پارامترهای ورودی اشتباه می باشد");

            adminDbContext.UserRoles.Remove(finded);
            await adminDbContext.SaveChangesAsync(cancellationToken);
            return OperationResult.BuildSuccessResult( "با موفقیت حذف گردید");
        }
    }
}
