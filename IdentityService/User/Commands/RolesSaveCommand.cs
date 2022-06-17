using Common;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class RolesSaveCommand : CommandRequest<OperationResult>
    {

        public RolesSaveModel SaveModel { get; set; }
    }

    class RolesSaveCommandHandler : IRequestHandler<RolesSaveCommand, OperationResult>
    {
        private readonly DatabaseContext databaseContext;
        private readonly UserManager<UserEntity> userManager;


        public RolesSaveCommandHandler(UserManager<UserEntity> userManager, DatabaseContext databaseContext)
        {
            this.userManager = userManager;
            this.databaseContext = databaseContext;
        }

        public async Task<OperationResult> Handle(RolesSaveCommand request, CancellationToken cancellationToken)
        {
            var entity = await userManager.Users.FirstOrDefaultAsync(x => x.Id == request. SaveModel.UserId, cancellationToken: cancellationToken);

            foreach (var role in request.SaveModel.Roles)
            {
                var roleEntity = await databaseContext.Roles.FirstOrDefaultAsync(x => x.Name.ToLower() == role.RoleName, cancellationToken: cancellationToken);
                var userRoleEntity = await databaseContext.UserRoles.FirstOrDefaultAsync(x => x.RoleId == roleEntity.Id && x.UserId == entity.Id, cancellationToken: cancellationToken);

                if (role.IsGrant == true)
                {
                    if (userRoleEntity == null) databaseContext.UserRoles.Add(new UserRoleEntity() { RoleId = roleEntity.Id, UserId = entity.Id });
                }
                else
                {
                    if (userRoleEntity != null) { databaseContext.UserRoles.Remove(userRoleEntity); }
                }
            }
            await databaseContext.SaveChangesAsync(cancellationToken);

            return OperationResult.BuildSuccessResult( "با موفقیت ذخیره گردید");
        }
    }
}
