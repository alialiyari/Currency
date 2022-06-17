using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication; 

namespace Identity
{
    public class LogoutPageModel : PageModelAbstract
    {
        public LogoutPageModel()
        {
           
        }
        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync();
        }
  
    }
}
