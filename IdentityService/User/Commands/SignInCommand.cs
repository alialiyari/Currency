using Common;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Userr
{
    public class SignInCommand : CommandRequest<ServiceDto<SignInResponse>>
    {
        public SignInRequest signInModel;
    }

    class SignInCommandHandler : IRequestHandler<SignInCommand, ServiceDto<SignInResponse>>
    {

        private readonly DatabaseContext dataBaseContext;
        private readonly FileServiceApi fileServiceApi;
        private readonly IConfiguration configuration;
        private readonly IMediator mediator;
        private readonly SignInManager<UserEntity> signInManager;

        public SignInCommandHandler(IMediator mediator, SignInManager<UserEntity> signInManager, DatabaseContext dataBaseContext, FileServiceApi fileServiceApi, IConfiguration configuration)
        {
            this.dataBaseContext = dataBaseContext;
            this.mediator = mediator;

            this.signInManager = signInManager;
            this.fileServiceApi = fileServiceApi;
            this.configuration = configuration;
        }

        

        public async Task<ServiceDto<SignInResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var user = await dataBaseContext.Users.AsNoTracking().Include(x => x.UserRoles).ThenInclude(x => x.Role)
                   .SingleOrDefaultAsync(x => x.UserName == request.signInModel.UserName, cancellationToken);

            if (user is null)
            {
                return new ServiceDto<SignInResponse>() { Status = 0, Message = "نام کاربری و یا کلمه عبور اشتباه است" };
            }


            var passCheckResult = await signInManager.CheckPasswordSignInAsync(user, request.signInModel.Password, lockoutOnFailure: false);
            if (passCheckResult.Succeeded == false) return new ServiceDto<SignInResponse>() { Status = 0, Message = "نام کاربری یا کلمه عبور اشتباه است" };



            if (user.VerifyStatus == VerifyStatusEnum.Requested)
                return new ServiceDto<SignInResponse>()
                {
                    Status = 0,
                    Message = "حساب کاربری شما تائید نشده است، لطفا با مدیر سیستم تماس بگیرید"
                };

            if (user.ActiveStatus == ActiveStatusEnum.InActive)
                return new ServiceDto<SignInResponse>()
                {
                    Status = 0,
                    Message = "حساب کاربری شما غیر فعال شده است، لطفا با مدیر سیستم تماس بگیرید"
                };

            if (user.PhoneNumberConfirmed == false)
                return new ServiceDto<SignInResponse>()
                {
                    Status = 0,
                    Message = @" کاربر گرامی: شماره همراه شما تایید نشده است."
                };


            await dataBaseContext.Database.ExecuteSqlRawAsync($"update [sec].[users] set lastLoginDate = getDate() where Id={user.Id}", cancellationToken);


            var signInResponse = new SignInResponse() { ExpireDate = DateTime.Now.AddDays(1) };
            if (request.signInModel.RememberMe) signInResponse.ExpireDate = DateTime.Now.AddYears(1);
            signInResponse.Token = await TokenGenerate(user, signInResponse.ExpireDate, cancellationToken);


            return new ServiceDto<SignInResponse>() { Data = signInResponse };
        }


        private async Task<string> TokenGenerate(UserEntity user, DateTime ExpireDate, CancellationToken cancellationToken)
        {
            var secretKey = Encoding.UTF8.GetBytes(configuration["JwtKey"]); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(configuration["JwtKey"][..16]); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await ClaimGenerate(user);

            var descriptor = new SecurityTokenDescriptor
            {
                //Issuer = "a.aliyari",
                //Audience = "a.aliyari",
                IssuedAt = DateTime.Now,
                //NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                Expires = ExpireDate,
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwt = tokenHandler.WriteToken(securityToken);

            return jwt;
        }

        private async Task<IEnumerable<Claim>> ClaimGenerate(UserEntity user)
        {
            ClaimsPrincipal result = new ClaimsPrincipal();
            if (user.Id == 0)
            {
                result = new ClaimsPrincipal();
            }
            else
            {
                result = await signInManager.ClaimsFactory.CreateAsync(user);
            }
            var claims = new List<Claim>(result.Claims)
            {
                new Claim("FullName", $"{user.FirstName } {user.LastName }"),
                new Claim("ImageUrl", fileServiceApi.FileUrlGenerate(user.ImageUrl))
            };

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    //claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));

                    string ObjectId = "";
                    if (string.IsNullOrEmpty(userRole.ObjectId) == false) ObjectId = userRole.ObjectId;
                    claims.Add(new Claim(userRole.Role.Name, ObjectId));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Public"));
                claims.Add(new Claim("Public", ""));
            }


            return claims;
        }
    }
}
