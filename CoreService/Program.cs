
global using Common;
global using Models;

global using MediatR;
global using Mapster;
global using Microsoft.EntityFrameworkCore;

global using MagicOnion;
global using MagicOnion.Server;

using System.Reflection;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;

builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = null;
    options.MaxSendMessageSize = null;
});

services.AddMagicOnion(option =>
{
    option.IsReturnExceptionStackTraceInErrorDetail = true;
    option.GlobalFilters.Add(new MagicOnion.Server.MagicOnionServiceFilterDescriptor(new MagicOnionErrorHandling(), 0));
    option.SerializerOptions = MessagePack.MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
});
services.AddMemoryCache();
services.AddHttpContextAccessor();
services.AddMediatR(typeof(DatabaseContext).GetTypeInfo().Assembly);


services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


IConfiguration configuration = builder.Configuration;

services.AddDbContext<DatabaseContext>(x => x.UseSqlServer(configuration.GetConnectionString("DatabaseContext")).AddInterceptors(new DataBaseCommandInterceptor()));

services.AddAuthenticationJwtBearer(configuration.GetSection("JwtKey").Value);


services.AddScoped<FileServiceApi>();



builder.WebHost
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    }).UseKestrel(options =>
    {

        options.ListenLocalhost(4019, o => o.Protocols = HttpProtocols.Http2);
        options.ListenLocalhost(4009, o => o.Protocols = HttpProtocols.Http1);
    });

var app = builder.Build();
app.UseCustomExceptionHandler(app.Environment);



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();


//app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapMagicOnionService();
    endpoints.MapControllers();

});


app.Run();
