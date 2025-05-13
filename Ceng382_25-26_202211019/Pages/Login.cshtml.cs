using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MyRazorApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IWebHostEnvironment _env;
        public LoginModel(IWebHostEnvironment env) => _env = env;

        [BindProperty]
        public InputModel Input { get; set; } = new();
        public class InputModel
        {
            [Required] public string Username { get; set; } = string.Empty;
            [Required][DataType(DataType.Password)] public string Password { get; set; } = string.Empty;
        }

        public IActionResult OnGet()
        {
            if (IsLoggedIn()) return RedirectToPage("/Index");
            return Page();
        }

        public IActionResult OnPost()
        {
            var path  = Path.Combine(_env.WebRootPath, "data", "users.json");
            var users = JsonSerializer.Deserialize<List<User>>(System.IO.File.ReadAllText(path)) ?? new();
            var user  = users.FirstOrDefault(u =>
                u.Username == Input.Username &&
                u.Password == Input.Password &&
                u.IsActive);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or password is incorrect.");
                return Page();
            }

            // Token & Session
            var token = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

            // Cookies
            var opts = new CookieOptions
            {
                Expires    = DateTimeOffset.Now.AddMinutes(30),
                HttpOnly   = true,
                Secure     = true,
                SameSite   = SameSiteMode.Strict
            };
            Response.Cookies.Append("username", user.Username, opts);
            Response.Cookies.Append("token", token, opts);
            Response.Cookies.Append("session_id", HttpContext.Session.Id, opts);

            return RedirectToPage("/Index");
        }

        private bool IsLoggedIn()
        {
            var usr = HttpContext.Session.GetString("username");
            var tok = HttpContext.Session.GetString("token");
            var sid = HttpContext.Session.Id;
            return usr != null
                && Request.Cookies["username"] == usr
                && Request.Cookies["token"]    == tok
                && Request.Cookies["session_id"] == sid;
        }
    }
}
