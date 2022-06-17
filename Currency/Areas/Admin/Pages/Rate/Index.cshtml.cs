using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Admin.Rate
{
    public class IndexPageModel : PageModelAbstract
    {
        public CoreChannelClient coreChannelClient;

        public IndexPageModel(CoreChannelClient coreChannelClient)
        {
            this.coreChannelClient = coreChannelClient;
        }
        public void OnGet()
        {
        }
        public async Task<ActionResult> OnPostGVResponse(GridViewDTO gridViewDTO)
        {
            var result = await coreChannelClient.Create<global::Rate.IRateService>().List();

            gridViewDTO.Rows = result.Data.Select(x => new GridViewRow() { Data = x }).ToList();
            gridViewDTO.Count = result.Data.Count;

            return ViewComponent("GridView", gridViewDTO);
        }

        //public async Task<JsonResult> OnPostDeleteAsync(long Id)
        //{
        //    var result = await coreChannelClient.Create<global::Rate.IRateService>().Delete(Id);
        //    return Json(result);
        //}

    }
}
