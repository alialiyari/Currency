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
    public class RoleSetCommand : CommandRequest<OperationResult<long?>>
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? RoleId { get; set; }


        public string RoleName { get; set; }
    }

    class RoleSetCommandHandler : IRequestHandler<RoleSetCommand, OperationResult<long?>>
    {

        private readonly DatabaseContext adminDbContext;


        public RoleSetCommandHandler( DatabaseContext adminDbContext)
        {
            this.adminDbContext = adminDbContext;
        }

        public async Task<OperationResult<long?>> Handle(RoleSetCommand request, CancellationToken cancellationToken)
        {
            UserRoleEntity finded = null;
            if (request.RoleId.HasValue == false)
                request.RoleId = (await adminDbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Name == request.RoleName, cancellationToken: cancellationToken)).Id;



            if (request.Id.HasValue)
                finded = await adminDbContext.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            if (finded == null)
                finded = await adminDbContext.UserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId, cancellationToken: cancellationToken);


            if (finded != null) return OperationResult<long?>.BuildSuccessResult(finded.Id, "با موفقیت ذخیره گردید");

            var result = adminDbContext.UserRoles.Add(new UserRoleEntity()
            {
                RoleId = request.RoleId.Value,
                UserId = request.UserId.Value,
            });

            
            await adminDbContext.SaveChangesAsync(cancellationToken);
            return OperationResult<long?>.BuildSuccessResult(result.Entity.Id, "با موفقیت ذخیره گردید");
        }
    }
}
