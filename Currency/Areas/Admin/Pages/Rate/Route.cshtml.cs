using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Admin.Rate
{
    public class RoutePageModel : PageModelAbstract
    {
        public CoreChannelClient coreChannelClient;

        public RoutePageModel(CoreChannelClient coreChannelClient)
        {
            this.coreChannelClient = coreChannelClient;
        }
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostFind(string from, string to)
        {
            var result = await coreChannelClient.Create<global::Rate.IRateService>().Find(from, to);
            return Partial("RouteResult", new ServiceDto<List<global::Rate.NodeModel>, string>()
            {
                D2 = to,
                D1 = result.Data,
                Status = result.Status,
                Message = result.Message,
            });
        }

    }
}
