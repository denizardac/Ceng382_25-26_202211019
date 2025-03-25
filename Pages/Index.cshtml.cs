using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        // In-memory verilerin saklandığı static alan
        private static List<ClassInformationModel> _storage = new List<ClassInformationModel>();

        // Form verileri
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        // Tabloda göstereceğimiz veriler
        public List<ClassInformationModel> Classes { get; set; } = new List<ClassInformationModel>();

        public void OnGet()
        {
            // Sayfa yüklendiğinde tabloya _storage içeriğini ver
            Classes = _storage;
        }

        public IActionResult OnPostAdd()
        {
            if (!ModelState.IsValid)
            {
                // Validasyon hataları varsa tabloyu tekrar doldur
                Classes = _storage;
                return Page();
            }

            // Edit modundan gelmişse varolan kaydı sil (Id aynıysa update)
            var existing = _storage.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                _storage.Remove(existing);
            }
            else
            {
                // Yepyeni bir kayıt ekleniyorsa
                NewClass.Id = ClassInformationModel.GetNextId();
            }

            // Listeye ekle
            _storage.Add(NewClass);

            // Formu temizle
            NewClass = new ClassInformationModel();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var toDelete = _storage.FirstOrDefault(c => c.Id == id);
            if (toDelete != null)
            {
                _storage.Remove(toDelete);
            }
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id)
        {
            var toEdit = _storage.FirstOrDefault(c => c.Id == id);
            if (toEdit != null)
            {
                // Var olan ID'yi koru (update için)
                NewClass.Id = toEdit.Id;
                NewClass.ClassName = toEdit.ClassName;
                NewClass.StudentCount = toEdit.StudentCount;
                NewClass.Description = toEdit.Description;

                // Güncellenecek öğeyi geçici olarak listeden çıkar
                _storage.Remove(toEdit);
            }

            // Tabloda verileri görebilmek için
            Classes = _storage;
            return Page();
        }
    }
}
