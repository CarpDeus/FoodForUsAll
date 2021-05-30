using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain;

namespace UI.Pages
{
    [AllowAnonymous]
    public class HomeModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
                return Redirect("/Index");
            else
                return Page();
        }
    }
}