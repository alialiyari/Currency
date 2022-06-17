using Common;
using Entities;
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
    public class SaveCommand : CommandRequest<ServiceDto>
    {
        public long? UserId { get; set; }
        public SaveModel SaveModel { get; set; }

        public bool? AllowChangeUsername { get; set; }
    }

    class SaveCommandHandler : IRequestHandler<SaveCommand, ServiceDto>
    {

        private readonly FileServiceApi fileServiceApi;
        private readonly DatabaseContext dataBaseContext;
        private readonly UserManager<UserEntity> userManager;

        public SaveCommandHandler(DatabaseContext adminDbContext, UserManager<UserEntity> userManager, FileServiceApi fileServiceApi)
        {
            this.dataBaseContext = adminDbContext;
            this.userManager = userManager;
            this.fileServiceApi = fileServiceApi;
        }

        public async Task<ServiceDto> Handle(SaveCommand request, CancellationToken cancellationToken)
        {
            var mached = await dataBaseContext.Users
                .Where(x => x.Id == request.SaveModel.Id)
                .Where(x => x.Id == request.UserId || request.UserId == null)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (mached == null) return new ServiceDto() { Status = 0, Message = "پارامترهای ارسالی اشتباه می باشد" };


            if (request.AllowChangeUsername == true)
            {
                if (!string.IsNullOrEmpty(request.SaveModel.UserName) && mached.UserName != request.SaveModel.UserName)
                {
                    var usernameChangeResult = await userManager.SetUserNameAsync(mached, request.SaveModel.UserName);
                    if (usernameChangeResult.Succeeded == false)
                    {
                        //<br/>{usernameChangeResult.Errors}
                        return new ServiceDto() { Status = 0, Message = $"نام کاربری نتوانست تغییر کند" };
                    }
                }
            }

            if (!string.IsNullOrEmpty(request.SaveModel.Password))
            {
                var passwrodRemoveResult = await userManager.RemovePasswordAsync(mached);
                if (passwrodRemoveResult.Succeeded == false)
                {
                    return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند" };
                }

                var passwrodAddResult = await userManager.AddPasswordAsync(mached, request.SaveModel.Password);
                if (passwrodAddResult.Succeeded == false)
                {
                    return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند" };
                }
            }
            if (request.SaveModel.ActiveStatus == ActiveStatusEnum.InActive)
            {
                if (request.SaveModel.Mobile != mached.PhoneNumber) { mached.PhoneNumberConfirmed = false; }
            }
            else
            {
                mached.PhoneNumberConfirmed = request.SaveModel.MobileConfirmed;
            }


            mached.Use(x =>
            {
                x.FirstName = request.SaveModel.FirstName;
                x.LastName = request.SaveModel.LastName;

                x.PhoneNumber = request.SaveModel.Mobile;

                x.Address = request.SaveModel.Address;
                x.PostalCode = request.SaveModel.PostalCode;


                x.ActiveStatus = request.SaveModel.ActiveStatus;


                x.Email = request.SaveModel.Email;
                x.NormalizedEmail = request.SaveModel.Email;

                x.ShSh = request.SaveModel.ShSh;
                x.NationalCode = request.SaveModel.NationalCode;

                x.BirthDate = request.SaveModel.BirthDate;
            });

            if (request.SaveModel.VerifyStatus == VerifyStatusEnum.Verified)
            {
                if (mached.VerifyStatus != VerifyStatusEnum.Verified)
                {
                    mached.VerifyStatus = VerifyStatusEnum.Verified;
                }
            }
            else
            {
                mached.VerifyStatus = request.SaveModel.VerifyStatus;
            }


            mached.ImageUrl = await fileServiceApi.FileSave(request.SaveModel.ImageFile, mached.ImageUrl);
            mached.NationalCardUrl = await fileServiceApi.FileSave(request.SaveModel.NationalCardFile, mached.NationalCardUrl);

            await dataBaseContext.SaveChangesAsync(cancellationToken);
            return new ServiceDto() { Message = "با موفقیت ذخیره گردید" };
        }
    }
}