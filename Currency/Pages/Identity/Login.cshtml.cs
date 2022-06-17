using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Models;
using Userr;

namespace Identity
{
    public class LoginPageModel : PageModelAbstract
    {
        private readonly IConfiguration configuration;
        private readonly IdentityChannelClient identityChannelClient;

        public LoginPageModel(IConfiguration configuration, IdentityChannelClient identityChannelClient)
        {
            this.configuration = configuration;
            this.identityChannelClient = identityChannelClient;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync(SignInRequest model)
        {
            var res = await identityChannelClient.Create<IUserService>().SignIn(model);
            if (res.Status == 0) return Json(new ServiceDto { Status = 0, Message = res.Message, });


            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = res.Data.ExpireDate
            };

            authProperties.StoreTokens(new List<AuthenticationToken>{
                new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = res.Data.Token,}});


            var claimsIdentity = new ClaimsIdentity(JWTHelper.TokenClaimsGet(res.Data.Token, configuration["JwtKey"]), CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);



            var ReturnUrl = "/Admin/";
            var roles = claimsIdentity.Claims.Where(x => x.Type == claimsIdentity.RoleClaimType).Select(x => x.Value);

            return Json(new ServiceDto<object>() { Status = 1, Data = new { ReturnUrl, res.Data.Token }, Message = "با موفقیت شناسایی شدید" });
        }


        public async Task<IActionResult> OnPostRegisterAsync(string RregisterUserName, string Mobile)
        {
            var res = await identityChannelClient.Create<IUserService>().InsertSave(new InsertSaveModel()
            {
                ActiveStatus = ActiveStatusEnum.Active,
                Address = "",
                Email = $"{RregisterUserName}@Currency.com",
                FirstName = RregisterUserName,
                Mobile = Mobile,
                MobileConfirmed = true,
                Password = RregisterUserName,
                UserName = RregisterUserName,
                VerifyStatus = VerifyStatusEnum.Verified
            });
            if (res.Status == 0) return Json(new ServiceDto { Status = 0, Message = res.Message, });

            return Json(new ServiceDto<object>() { Status = 1, Message = "با موفقیت ثبت نام شدید" });
        }
    }
}
