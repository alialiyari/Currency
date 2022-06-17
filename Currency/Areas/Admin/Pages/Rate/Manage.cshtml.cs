using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Admin.Rate
{
    public class ManagePageModel : PageModelAbstract
    {
        public CoreChannelClient coreChannelClient;

        public ManagePageModel(CoreChannelClient coreChannelClient)
        {
            this.coreChannelClient = coreChannelClient;
        }

        [BindProperty(SupportsGet = true)] public long? Id { get; set; }


        public void OnGet()
        {

        }


        public async Task<JsonResult> OnPostSaveAsync(global::Rate.SaveModel saveModel)
        {
            var result = await coreChannelClient.Create<global::Rate.IRateService>().Save(saveModel);

            return Json(result);
        }
    }
}