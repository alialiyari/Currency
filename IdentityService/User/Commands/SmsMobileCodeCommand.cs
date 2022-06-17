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
    public class SmsMobileCodeCommand : CommandRequest<ServiceDto>
    {
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
    }

    class SmsMobileCodeCommandHandler : IRequestHandler<SmsMobileCodeCommand, ServiceDto>
    {

        private readonly DatabaseContext dataBaseContext;
        private readonly SmsChannelClient smsChannelClient;

        public SmsMobileCodeCommandHandler(DatabaseContext dataBaseContext, SmsChannelClient smsChannelClient)
        {
            this.dataBaseContext = dataBaseContext;
            this.smsChannelClient = smsChannelClient;
        }

        public async Task<ServiceDto> Handle(SmsMobileCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await dataBaseContext.Users.AsNoTracking().Include(x => x.Claims).AsNoTracking()
                .Where(x => x.NationalCode == request.NationalCode).FirstOrDefaultAsync(cancellationToken);

            if (user == null) return new ServiceDto() { Status = 0, Message = "پارامترهای ورودی اشتباه می باشد" };

            if (user.PhoneNumber != request.Mobile) return new ServiceDto() { Status = 0, Message = "پارامترهای ورودی اشتباه می باشد" };
            if (user.ActiveStatus == ActiveStatusEnum.InActive) return new ServiceDto() { Status = 0, Message = "حساب کاربری شما غیر فعال می باشد" };



            var claim = user.Claims.Where(x => x.ClaimType == "PasswordRecoveryKey").FirstOrDefault();
            if (claim == null)
            {
                var SmsMobileCode = CreateRandomPassword();
                dataBaseContext.UserClaims.Add(new UserClaimEntity() { UserId = user.Id, ClaimType = "PasswordRecoveryKey", ClaimValue = SmsMobileCode });
                await dataBaseContext.SaveChangesAsync(cancellationToken);

                _ = smsChannelClient.Create<Sms.ISmsService>().Send(user.PhoneNumber, $"کد تغییر: {SmsMobileCode} \nبقال نت");
                return new ServiceDto() { Message = $"کد تغییر به شماره همراه تان پیامک گردید" };
            }

            _ = smsChannelClient.Create<Sms.ISmsService>().Send(user.PhoneNumber, $"کد تغییر: {claim.ClaimValue} \nبقال نت");
            return new ServiceDto() { Message = $"کد تغییر به شماره همراه تان دوباره پیامک گردید" };
        }

        private static string CreateRandomPassword(int length = 6)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "0123456789";
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
