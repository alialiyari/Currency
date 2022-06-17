using Common;
using Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class RolesViewQuery : CommandRequest<OperationResult<RolesViewModel>>
    {
        public long Id { get; set; } 
    }

    class RolesViewQuerywHandler : IRequestHandler<RolesViewQuery, OperationResult<RolesViewModel>>
    {

        private readonly DatabaseContext dbContext;


        public RolesViewQuerywHandler(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OperationResult<RolesViewModel>> Handle(RolesViewQuery request, CancellationToken cancellationToken)
        {
            var finded = await dbContext.UserRoles.AsNoTracking().Where(x => x.UserId == request.Id)
                .Select(x => x.Role.Name).ToListAsync(cancellationToken: cancellationToken);
             

            return OperationResult<RolesViewModel>.BuildSuccessResult(new RolesViewModel() { Roles = finded });
        }
    }
}
