using Common;
using Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class InsertCommand : CommandRequest<ServiceDto<long?>>
    {
        public InsertSaveModel SaveModel { get; set; }
    }

    internal class CreateCommandHandler : IRequestHandler<InsertCommand, ServiceDto<long?>>
    {

        private readonly FileServiceApi fileServiceApi;
        private readonly DatabaseContext dataBaseContext;
        private readonly UserManager<UserEntity> userManager;

        public CreateCommandHandler(UserManager<UserEntity> userManager, DatabaseContext dataBaseContext, FileServiceApi fileServiceApi)
        {
            this.userManager = userManager;
            this.fileServiceApi = fileServiceApi;
            this.dataBaseContext = dataBaseContext;
        }

        public async Task<ServiceDto<long?>> Handle(InsertCommand request, CancellationToken cancellationToken)
        {
            var beforeData = await dataBaseContext.Users.AsNoTracking()
                .Where(
                       x => x.UserName == request.SaveModel.UserName
                    || x.PhoneNumber == request.SaveModel.Mobile
                    || x.Id == request.SaveModel.IntroducerUserId
                ).ToListAsync(cancellationToken);


            var nationalCodeMachtedUser = beforeData.FirstOrDefault(x => x.UserName == request.SaveModel.UserName);

            if (nationalCodeMachtedUser != null)
            {

                return new ServiceDto<long?>() { Status = 0, Message = "کد ملی (نام کاربری) تکراری می باشد" };
                //if (nationalCodeMachtedUser.PhoneNumber == request.SaveModel.Mobile)
                //{
                //    return new ServiceDto<long?>() { Data = nationalCodeMachtedUser.Id, Message = "کد ملی (نام کاربری) تکراری می باشد" };
                //}
                //else
                //{
                //    return new ServiceDto<long?>() { Status = 0, Message = "کد ملی (نام کاربری) تکراری می باشد" };
                //}
            }


            if (!string.IsNullOrEmpty(request.SaveModel.Mobile))
            {
                if (beforeData.Where(x => x.PhoneNumber == request.SaveModel.Mobile).Any())
                {
                    return new ServiceDto<long?>() { Status = 0, Message = "شماره موبایل وارده تکراری می باشد" };
                }
            }

            if (request.SaveModel.IntroducerUserId.HasValue)
            {
                if (beforeData.Where(x => x.Id == request.SaveModel.IntroducerUserId).Any() == false)
                {
                    return new ServiceDto<long?>() { Status = 0, Message = "شخصی با این کد معرف پیدا نشد" };
                }
            }

            var user = new UserEntity
            {
                UserName = request.SaveModel.UserName,
                FirstName = request.SaveModel.FirstName,
                LastName = request.SaveModel.LastName,

                NationalCode = request.SaveModel.NationalCode,

                Email = request.SaveModel.Email,
                BirthDate = request.SaveModel.BirthDate,
                ShSh = request.SaveModel.ShSh,

                PhoneNumber = request.SaveModel.Mobile,
                PhoneNumberConfirmed = request.SaveModel.MobileConfirmed,

                ActiveStatus = request.SaveModel.ActiveStatus,
                VerifyStatus = request.SaveModel.VerifyStatus,
                RegisterDate = DateTime.Now,

                Address = request.SaveModel.Address,
                PostalCode = request.SaveModel.PostalCode,

                IntroducerUserId = request.SaveModel.IntroducerUserId,
            };


            var addUserResult = await userManager.CreateAsync(user, request.SaveModel.Password);
            if (addUserResult.Succeeded == false)
            {
                return new ServiceDto<long?>() { Status = 0, Message = string.Join(", ", addUserResult.Errors) };

            }

            user.ImageUrl = await fileServiceApi.FileSave(request.SaveModel.ImageFile);
            user.NationalCardUrl = await fileServiceApi.FileSave(request.SaveModel.NationalCardFile);

            //await userManager.AddToRoleAsync(user, request.SaveModel.Role.ToString());
            if (request.SaveModel.RoleId.HasValue)
                dataBaseContext.UserRoles.Add(new UserRoleEntity()
                {
                    RoleId = request.SaveModel.RoleId.Value,
                    UserId = user.Id,
                    ObjectId = request.SaveModel.RoleObjectId
                });

            await dataBaseContext.SaveChangesAsync(cancellationToken);
            return new ServiceDto<long?>() { Data = user.Id, Message = "با موفقیت ذخیره گردید" };
        }
    }
}
