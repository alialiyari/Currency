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
    public class SmsPassCommand : CommandRequest<ServiceDto>
    {
        public string Mobile { get; set; }
        public string SmsCode { get; set; }
        public string NationalCode { get; set; }
    }

    class SmsPassCommandHandler : IRequestHandler<SmsPassCommand, ServiceDto>
    {

        private readonly UserManager<UserEntity> userManager;
        private readonly DatabaseContext dataBaseContext;
        private readonly SmsChannelClient smsChannelClient;

        public SmsPassCommandHandler(UserManager<UserEntity> userManager, DatabaseContext dataBaseContext, SmsChannelClient smsChannelClient)
        {
            this.userManager = userManager;
            this.dataBaseContext = dataBaseContext;
            this.smsChannelClient = smsChannelClient;
        }

        public async Task<ServiceDto> Handle(SmsPassCommand request, CancellationToken cancellationToken)
        {

            var user = await dataBaseContext.Users.AsNoTracking().Include(x => x.Claims)
                .Where(x => x.NationalCode == request.NationalCode).FirstOrDefaultAsync(cancellationToken);

            if (user == null) return new ServiceDto() { Message = "پارامترهای ورودی اشتباه می باشد" };

            if (user.PhoneNumber != request.Mobile) return new ServiceDto() { Message = "پارامترهای ورودی اشتباه می باشد" };
            if (user.ActiveStatus == ActiveStatusEnum.InActive) return new ServiceDto() { Message = "پارامترهای ورودی اشتباه می باشد" };

            var claim = user.Claims.Where(x => x.ClaimType == "PasswordRecoveryKey").FirstOrDefault();
            if (claim == null) return new ServiceDto() { Message = "پارامترهای ورودی اشتباه می باشد /c" };

            if (claim.ClaimValue != request.SmsCode) return new ServiceDto() { Message = "کد ارسال شده به همراهتان را اشتباه وارد کرده اید" };



            dataBaseContext.UserClaims.Remove(claim);
            await dataBaseContext.SaveChangesAsync(cancellationToken);



            var password = CreateRandomPassword();



            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded == false) return new ServiceDto()
            {
                Status = 0,
                Message = $"{ result.Errors.FirstOrDefault()?.Description} { result.Errors.FirstOrDefault()?.Description }"
            };

            //var removePasswordResult = await userManager.RemovePasswordAsync(new UserEntity() { Id = user.Id, UserName = user.UserName });
            //if (removePasswordResult.Succeeded == false) return new ServiceDto()
            //{
            //    Status = 0,
            //    Message = $"{ removePasswordResult.Errors.FirstOrDefault()?.Description} { removePasswordResult.Errors.FirstOrDefault()?.Description }"
            //};
            //var setPasswordResult = await userManager.AddPasswordAsync(new UserEntity() { Id = user.Id, UserName = user.UserName }, password);
            //if (setPasswordResult.Succeeded == false) return new ServiceDto()
            //{
            //    Status = 0,
            //    Message = $"{ setPasswordResult.Errors.FirstOrDefault()?.Description} { setPasswordResult.Errors.FirstOrDefault()?.Description }"
            //};




            _ = smsChannelClient.Create<Sms.ISmsService>().Send(user.PhoneNumber, $"کلمه عبور جدید:{password}\nبقال نت");
            return new ServiceDto() { Message = $"اطلاعات ورود شما به شماره همراه تان پیامک گردید" };
        }

        private static string CreateRandomPassword(int length = 6)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "0123456789";
            //string validChars = "abcdefghijklmnopqrstuvwxyz0123456789";
            //string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            var random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

    }
}
