using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System.Threading;
using System.Threading.Tasks;

public abstract class RestApiAbstract
{
    public string ApiAddress = string.Empty;
    readonly IHttpContextAccessor HttpContextAccessor;
    public RestApiAbstract(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;
    }

    public virtual async Task<string> Get(string uriResource)
    {
        var req = new RestRequest(uriResource, Method.Get);
        req.AddHeader("Accept", "application/json");
        req.AddHeader("Content-Type", "application/json; charset=utf-8");
        req.AddHeader("Accept-Language", Thread.CurrentThread.CurrentUICulture.Name);

        req.Timeout = 300000; // 5 minute


        await AddAuthenticationHeader(req);

        var client = new RestClient(ApiAddress);

        var response = await client.ExecuteAsync(req);
        if (response.IsSuccessful) return response.Content;


        if (response.ErrorException != null) throw response.ErrorException;
        throw new System.Exception(response.StatusCode.ToString());
    }

    public virtual async Task<T> Get<T>(string uriResource)
    {
        string response = await Get(uriResource);
        return JsonConvert.DeserializeObject<T>(response);
    }

    public virtual async Task<string> Post(string uriResource, object data = null, Method method = Method.Post)
    {
        //var req = new RestRequest(uriResource, method, DataFormat.Json);
        var req = new RestRequest(uriResource, method);
        req.RequestFormat = DataFormat.Json;

        req.AddHeader("Accept", "application/json");
        req.AddHeader("Content-Type", "application/json; charset=utf-8");
        req.AddHeader("Accept-Language", Thread.CurrentThread.CurrentUICulture.Name);
        req.AddJsonBody(data);

        await AddAuthenticationHeader(req);

        var client = new RestClient(ApiAddress);

        var response = await client.ExecuteAsync(req);
        if (response.IsSuccessful) return response.Content;

        if (response.ErrorException != null) throw response.ErrorException;
        throw new System.Exception($"url: { ApiAddress}{uriResource} error: {response.StatusCode}");
    }

    public virtual async Task<T> Post<T>(string uriResource, object data = null, Method method = Method.Post)
    {
        string response = await Post(uriResource, data, method);
        return JsonConvert.DeserializeObject<T>(response);
    }

    public virtual async Task AddAuthenticationHeader(RestRequest request)
    {
        if (HttpContextAccessor is null) return;

        if (HttpContextAccessor.HttpContext.Items["RawToken"] is string token)
        {
            request.AddHeader("Authorization", $"Bearer {token}");
            return;
        }

        if (!HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return;
        }
        token = await HttpContextAccessor.HttpContext.GetTokenAsync("access_token");
        if (token is null)
        {
            return;
        }
        request.AddHeader("Authorization", $"Bearer {token}");
    }
}
