using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using MyRazorApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        private static List<ClassInformationModel> _storage = new();

        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public int TotalPages { get; set; }
        public List<ClassInformationTable> FilteredClasses { get; set; } = new();
        public List<string> SelectedColumns { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!IsLoggedIn())
                return RedirectToPage("/Login");

            if (_storage.Count == 0)
            {
                for (int i = 1; i <= 100; i++)
                {
                    _storage.Add(new ClassInformationModel
                    {
                        Id = ClassInformationModel.GetNextId(),
                        ClassName = $"Sample Class {i}",
                        StudentCount = i * 10,
                        Description = $"Description for Class {i}"
                    });
                }
            }

            var query = _storage.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Filter))
                query = query.Where(x =>
                    x.ClassName.Contains(Filter, StringComparison.OrdinalIgnoreCase));
            query = query.OrderByDescending(x => x.Id);

            TotalPages = (int)Math.Ceiling(query.Count() / (double)PageSize);
            Page = Math.Clamp(Page, 1, Math.Max(TotalPages, 1));

            FilteredClasses = query
                .Skip((Page - 1) * PageSize)
                .Take(PageSize)
                .Select(x => new ClassInformationTable
                {
                    Id = x.Id,
                    ClassName = x.ClassName,
                    StudentCount = x.StudentCount,
                    Description = x.Description
                })
                .ToList();

            return Page();
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            var exist = _storage.FirstOrDefault(c => c.Id == NewClass.Id);
            if (exist != null)
                _storage.Remove(exist);
            else
                NewClass.Id = ClassInformationModel.GetNextId();

            _storage.Add(NewClass);
            NewClass = new();

            // ← Redirect back to /Index with current paging/filter parameters
            return RedirectToPage("/Index", new { Page, PageSize, Filter });
        }

        public IActionResult OnPostEdit(int id)
        {
            var e = _storage.FirstOrDefault(c => c.Id == id);
            if (e != null)
            {
                NewClass = new ClassInformationModel
                {
                    Id = e.Id,
                    ClassName = e.ClassName,
                    StudentCount = e.StudentCount,
                    Description = e.Description
                };
                _storage.Remove(e);
            }
            OnGet();
            return Page();
        }

        public IActionResult OnPostDelete(int id)
        {
            var d = _storage.FirstOrDefault(c => c.Id == id);
            if (d != null)
                _storage.Remove(d);

            // ← Redirect back to /Index with current paging/filter parameters
            return RedirectToPage("/Index", new { Page, PageSize, Filter });
        }

        public FileResult OnPostExportAll()
        {
            var json = Utils.Instance.ExportToJson(_storage, SelectedColumns);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "AllData.json");
        }

        public FileResult OnPostExportFiltered()
        {
            var json = Utils.Instance.ExportToJson(FilteredClasses, SelectedColumns);
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", "FilteredData.json");
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
