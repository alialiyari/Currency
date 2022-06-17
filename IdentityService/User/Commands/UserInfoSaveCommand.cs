//using Common;
//using Entities;
//using Extensions;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Models;
//
//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Userr
//{
//    public class UserInfoSaveCommand : CommandRequest<ServiceDto>
//    {
//        public long? UserId { get; set; }
//        public bool AllowChangeUsername { get; set; }


//        public SaveModel SaveModel { get; set; }
//    }

//    class UserInfoSaveCommandHandler : IRequestHandler<UserInfoSaveCommand, ServiceDto>
//    {

//        private readonly FileServiceApi fileServiceApi;
//        private readonly DataBaseContext adminDbContext;
//        private readonly UserManager<UserEntity> userManager;

//        public UserInfoSaveCommandHandler(DataBaseContext adminDbContext, UserManager<UserEntity> userManager, FileServiceApi fileServiceApi)
//        {
//            this.adminDbContext = adminDbContext;
//            this.userManager = userManager;
//            this.fileServiceApi = fileServiceApi;
//        }

//        public async Task<ServiceDto> Handle(UserInfoSaveCommand request, CancellationToken cancellationToken)
//        {
//            if (request.UserId != request.SaveModel.Id) { return new ServiceDto() { Status = 0, Message = "پارامترهای ارسالی اشتباه می باشد" }; }


//            var finded = await adminDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.SaveModel.Id, cancellationToken: cancellationToken);
//            if (finded == null) return new ServiceDto() { Status = 0, Message = "پارامترهای ارسالی اشتباه می باشد" };




//            // change username
//            if (request.AllowChangeUsername == true)
//            {
//                if (!string.IsNullOrEmpty(request.SaveModel.UserName) && finded.UserName != request.SaveModel.UserName)
//                {
//                    var usernameChangeResult = await userManager.SetUserNameAsync(finded, request.SaveModel.UserName);
//                    if (usernameChangeResult.Succeeded == false)
//                    {
//                        //<br/>{usernameChangeResult.Errors}
//                        return new ServiceDto() { Status = 0, Message = $"نام کاربری نتوانست تغییر کند" };
//                    }
//                }
//            }
           



//            if (!string.IsNullOrEmpty(request.SaveModel.Password))
//            {
//                var passwrodRemoveResult = await userManager.RemovePasswordAsync(finded);
//                if (passwrodRemoveResult.Succeeded == false)
//                {
//                    return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند" };
//                }

//                var passwrodAddResult = await userManager.AddPasswordAsync(finded, request.SaveModel.Password);
//                if (passwrodAddResult.Succeeded == false)
//                {
//                    return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند" };
//                }
//            }


//            finded.Use(x =>
//            {
//                x.FirstName = request.SaveModel.FirstName;
//                x.LastName = request.SaveModel.LastName;


//                x.ShSh = request.SaveModel.ShSh;
//                x.BirthDate = request.SaveModel.BirthDate;

//                x.PhoneNumber = request.SaveModel.Mobile;
//                x.PhoneNumberConfirmed = request.SaveModel.MobileConfirmed;

//                x.Address = request.SaveModel.Address;
//                x.PostalCode = request.SaveModel.PostalCode;


//                x.ActiveStatus = request.SaveModel.ActiveStatus;


//                x.Email = request.SaveModel.Email;
//                x.NormalizedEmail = request.SaveModel.Email;

              
//                x.NationalCode = request.SaveModel.NationalCode; 
//            });

//            if (request.SaveModel.VerifyStatus == VerifyStatusEnum.Verified)
//            {
//                if (finded.VerifyStatus != VerifyStatusEnum.Verified)
//                {
//                    finded.VerifyDate = DateTime.Now;
//                    finded.VerifierUserId = request.UserId;
//                    finded.VerifyStatus = VerifyStatusEnum.Verified;
//                }
//            }
//            else
//            {
//                finded.VerifyStatus = request.SaveModel.VerifyStatus;
//            }
            

//            finded.ImageUrl = await fileServiceApi.FileSave(request.SaveModel.ImageFile, finded.ImageUrl);
//            finded.NationalCardUrl = await fileServiceApi.FileSave(request.SaveModel.NationalCardFile, finded.NationalCardUrl);

//            await adminDbContext.SaveChangesAsync(cancellationToken);
//            return new ServiceDto() { Message = "با موفقیت ذخیره گردید" };
//        }
//    }
//}
