using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public static class ServiceCollectionAuthExtensions
{
    public static AuthenticationBuilder AddAuthenticationJwtBearer(this IServiceCollection services, string key)
    {
        return services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = JWTHelper.TokenValidationParamegersGenerate(key);
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                    //logger.LogError("Authentication failed.", context.Exception);

                    //if (context.Exception != null) throw context.Exception;
                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    //var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<UserEntity>>();
                    //var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                    var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                    if (claimsIdentity.Claims?.Any() != true) context.Fail("This token has no claims.");

                    //var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                    //if (!securityStamp.HasValue())
                    //    context.Fail("This token has no secuirty stamp");

                    ////Find user and token from database and perform your custom validation
                    //var userId = claimsIdentity.GetUserId<int>();
                    //var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                    ////if (user.SecurityStamp != Guid.Parse(securityStamp))
                    ////    context.Fail("Token secuirty stamp is not valid.");

                    //var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                    //if (validatedUser == null) context.Fail("Token secuirty stamp is not valid.");

                    //if (!user.IsActive) context.Fail("User is not active.");

                    //await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                },
                OnMessageReceived = context => {

                    var a = context.Token;
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                    //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                    if (context.AuthenticateFailure != null) throw context.AuthenticateFailure;
                    throw new Exception("You are unauthorized to access this resource");
                    //return Task.CompletedTask;
                }
            };
        });


    }
}
