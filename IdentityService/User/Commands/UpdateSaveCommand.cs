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
//    public class UpdateSaveCommand : CommandRequest<ServiceDto>
//    {
//        public long? CurrentUserId { get; set; }
//        public SaveModel SaveModel { get; set; }
//    }

//    class UpdateSaveCommandHandler : IRequestHandler<UpdateSaveCommand, ServiceDto>
//    {

//        private readonly DataBaseContext adminDbContext;
//        private readonly UserManager<UserEntity> userManager;
//        private readonly FileServiceApi fileServiceApi;

//        public UpdateSaveCommandHandler(DataBaseContext adminDbContext, UserManager<UserEntity> userManager, FileServiceApi fileServiceApi)
//        {
//            this.adminDbContext = adminDbContext;
//            this.userManager = userManager;
//            this.fileServiceApi = fileServiceApi;
//        }

//        public async Task<ServiceDto> Handle(UpdateSaveCommand request, CancellationToken cancellationToken)
//        {
//            var finded = await adminDbContext.Users.FirstOrDefaultAsync((System.Linq.Expressions.Expression<Func<UserEntity, bool>>)(x => x.Id == request.SaveModel.Id), cancellationToken: cancellationToken);
//            if (finded == null) return new ServiceDto() { Status = 0, Message = "پارامترهای ارسالی اشتباه می باشد" };

//            if (!string.IsNullOrEmpty(request.SaveModel.UserName) && finded.UserName != request.SaveModel.UserName)
//            {
//                var usernameChangeResult = await userManager.SetUserNameAsync((UserEntity)finded, (string)request.SaveModel.UserName);
//                if (usernameChangeResult.Succeeded == false)
//                {
//                    //<br/>{usernameChangeResult.Errors}
//                    return new ServiceDto() { Status = 0, Message = $"نام کاربری نتوانست تغییر کند" };
//                }
//            }
//            if (!string.IsNullOrEmpty((string)request.SaveModel.Password))
//            {
//                var passwrodRemoveResult = await userManager.RemovePasswordAsync((UserEntity)finded);
//                if (passwrodRemoveResult.Succeeded == false)
//                {
//                    return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند" };
//                }

//                var passwrodAddResult = await userManager.AddPasswordAsync((UserEntity)finded, (string)request.SaveModel.Password);
//                if (passwrodAddResult.Succeeded == false)
//                {
//                    return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند" };
//                }
//            }


//            finded.Use(x =>
//            {
//                x.FirstName = request.SaveModel.FirstName;
//                x.LastName = request.SaveModel.LastName;

//                x.PhoneNumber = request.SaveModel.Mobile;
//                x.PhoneNumberConfirmed = request.SaveModel.MobileConfirmed;

//                x.Address = request.SaveModel.Address;
//                x.PostalCode = request.SaveModel.PostalCode;


//                x.ActiveStatus = request.SaveModel.ActiveStatus;


//                x.Email = request.SaveModel.Email;
//                x.NormalizedEmail = request.SaveModel.Email;

//                x.IntroducerUserId = request.SaveModel.IntroducerUserId;
//                x.NationalCode = request.SaveModel.NationalCode;
//            });


//            if (finded.VerifyStatus != VerifyStatusEnum.Verified && request.SaveModel.VerifyStatus == VerifyStatusEnum.Verified)
//            {
//                finded.VerifyDate = DateTime.Now;
//                finded.VerifierUserId = request.CurrentUserId;
//                finded.VerifyStatus = VerifyStatusEnum.Verified;
//            }

//            finded.ImageUrl = await fileServiceApi.FileSave((FileDto)request.SaveModel.ImageFile, (string)finded.ImageUrl);
//            finded.NationalCardUrl = await fileServiceApi.FileSave((FileDto)request.SaveModel.NationalCardFile, (string)finded.NationalCardUrl);

//            await adminDbContext.SaveChangesAsync(cancellationToken);
//            return new ServiceDto() { Message = "با موفقیت ذخیره گردید" };
//        }
//    }
//}
