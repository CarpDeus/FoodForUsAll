using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Domain;

namespace UI.Pages
{
    [AllowAnonymous]
    public class HomeModel : PageModel
    {
        [Inject]
        IConfiguration Configuration { get; set; }

        public async Task<IActionResult> OnGet()
        {
            bool isDemoMode = bool.Parse(Configuration["AppSettings:IsDemoMode"]);

            if (User.Identity.IsAuthenticated || isDemoMode)
                return Redirect("/Index");
            else
                return Page();
        }
    }
}