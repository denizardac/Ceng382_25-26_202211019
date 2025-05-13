using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyRazorApp.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear();
            foreach (var key in new[] { "username", "token", "session_id" })
                Response.Cookies.Delete(key);
            return RedirectToPage("/Login");
        }
    }
}
