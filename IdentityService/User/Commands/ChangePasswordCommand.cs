using Common;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Userr
{
    public class ChangePasswordCommand : CommandRequest<ServiceDto>
    {
        public long UserId { get; set; }
        public bool IsResetPassword { get; set; } = false;

        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ServiceDto>
    {

        private readonly DatabaseContext dataBaseContext;
        private readonly UserManager<UserEntity> userManager;
        private readonly SignInManager<UserEntity> signInManager;

        public ChangePasswordCommandHandler(DatabaseContext adminDbContext, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
        {
            this.dataBaseContext = adminDbContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<ServiceDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await dataBaseContext.Users
               .Where(x => x.Id == request.UserId)
               .SingleOrDefaultAsync(cancellationToken: cancellationToken);

            if (user is null) return new ServiceDto() { Status = 0, Message = "پارامترهای ورودی اشتباه است" };


            if (request.IsResetPassword == false)
            {
                var passCheckResult = await signInManager.CheckPasswordSignInAsync(user, request.CurrentPassword, lockoutOnFailure: false);
                if (passCheckResult.Succeeded == false) return new ServiceDto() { Status = 0, Message = "کلمه عبور جاری شما اشتباه می باشد" };
            }

            var passwrodRemoveResult = await userManager.RemovePasswordAsync(user);
            if (passwrodRemoveResult.Succeeded == false) return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند 1" };

            var passwrodAddResult = await userManager.AddPasswordAsync(user, request.NewPassword);
            if (passwrodAddResult.Succeeded == false) return new ServiceDto() { Status = 0, Message = $"کلمه عبور نتوانست تغییر کند 2" };

            return new ServiceDto() { Message = "با موفقیت کلمه عبور تغییر کرد" };
        }
    }
}