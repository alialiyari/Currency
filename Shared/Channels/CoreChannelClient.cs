using MagicOnion.Client;
using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

public class CoreChannelClient
{

    public readonly GrpcChannel channel;
    private readonly IHttpContextAccessor httpContextAccessor;

    public CoreChannelClient(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
        channel = GrpcChannel.ForAddress(configuration["CoreGrpcServiceUrl"]);

        //channel = GrpcChannel.ForAddress(configuration["CoreGrpcServiceUrl"], new GrpcChannelOptions()
        //{
        //    MaxReceiveMessageSize = null,
        //    MaxSendMessageSize = null,
        //});
    }
    public T Create<T>() where T : MagicOnion.IService<T>
    {
        var client = MagicOnionClient.Create<T>(channel.CreateCallInvoker(),
          MessagePack.MessagePackSerializerOptions.Standard.WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance),
              new IClientFilter[] { new AppendHeaderFilter(httpContextAccessor) });
        return client;
    }

    public class AppendHeaderFilter : IClientFilter
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AppendHeaderFilter(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public async ValueTask<ResponseContext> SendAsync(RequestContext context, Func<RequestContext, ValueTask<ResponseContext>> next)
        {
            // add the common header(like authentication).
            Grpc.Core.Metadata header = context.CallOptions.Headers;
            await AddAuthenticationHeader(header);

            return await next(context);
        }

        public   async Task AddAuthenticationHeader(Grpc.Core.Metadata header)
        {
            if (httpContextAccessor is null) return;
            if (httpContextAccessor.HttpContext is null) return;

            if (httpContextAccessor.HttpContext.Items["RawToken"] is string token)
            {
                header.Add("Authorization", $"Bearer {token}");
                return;
            }

            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }
            token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (token is null) return;

            header.Add("Authorization", $"Bearer {token}");
        }
    }

}