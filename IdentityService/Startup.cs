using Common;
using Entities;
using Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.OpenApi.Models;


using System;
using System.Collections.Generic;
using System.Reflection;

namespace IdentityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddMagicOnion(option =>
            {
                option.GlobalFilters.Add(new MagicOnion.Server.MagicOnionServiceFilterDescriptor(new MagicOnionErrorHandling(), 0));
                option.SerializerOptions = MessagePack.MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
            });
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidateCommandBehavior<,>));

            services.Configure<EventLogSettings>(config =>
            {
                config.LogName = "Identity Service";
                config.SourceName = "Identity Service";
            });

            services.AddDbContextPool<DatabaseContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DatabaseContext"))
                .AddInterceptors(new DataBaseCommandInterceptor()));


            services.AddIdentityCore<UserEntity>(options =>
            {
                options.User.RequireUniqueEmail = false; options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false; options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false; options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddUserManager<UserManager<UserEntity>>().AddRoles<RoleEntity>().
            AddSignInManager().AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();
            services.AddAuthenticationJwtBearer(Configuration.GetSection("JwtKey").Get<string>());

            services.AddLocalization();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(s => s.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityService", Version = "v1" });
            });


            services.AddScoped(typeof(FileServiceApi));
            services.AddScoped(typeof(SmsChannelClient));

            //services.AddScoped(typeof(JWT.JWTHelper));
            //services.AddScoped(typeof(Identity.AuthenticationProvider));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler(env);

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityService v1"));
            }

            app.UseRouting();
            app.UseAuthentication();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMagicOnionService();
                endpoints.MapControllers();
            });
        }
    }
}
