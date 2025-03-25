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

            // Eğer Edit modundan geliyorsak, bu Id'ye sahip kaydı sil
            var existing = _storage.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                // Yani update yapıyoruz
                _storage.Remove(existing);
            }
            else
            {
                // Gerçekten yeni bir kayıt ekleniyor
                NewClass.Id = ClassInformationModel.GetNextId();
            }

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
                // Var olan ID'yi koruyoruz (update için).
                NewClass.Id = toEdit.Id;
                NewClass.ClassName = toEdit.ClassName;
                NewClass.StudentCount = toEdit.StudentCount;
                NewClass.Description = toEdit.Description;

                // Liste dışına alıyoruz ki OnPostAdd()'te "update" yapabilsin
                _storage.Remove(toEdit);
            }

            // Tabloda verileri görebilmek için
            Classes = _storage;
            return Page();
        }
    }
}
