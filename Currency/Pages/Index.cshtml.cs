using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Portal.Pages
{
    public class IndexModel : PageModelAbstract
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            OnGetCartable();
        }

        public void OnGetCartable()
        {
            var ReturnUrl = "/Admin";
            if (User.HasRole("Admin"))
            {
                ReturnUrl = "/Admin";
            }

            Response.Redirect(ReturnUrl);
        }
    }
}