using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        // In-memory veritabanı gibi davranacak statik liste
        public static List<ClassInformationModel> Classes { get; set; }
            = new List<ClassInformationModel>();

        // Form verileri bu property'e bind edilecek
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        public void OnGet()
        {
            // Sayfa yüklendiğinde tabloya Classes listesini yansıtır.
        }

        // Ekleme veya Güncelleme (Add / Update)
        public IActionResult OnPostAdd()
        {
            // ModelState geçerli değilse formu tekrar göster
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Eğer Edit modundan geliyorsak ve listede bu Id varsa önce eski kaydı silelim
            var existing = Classes.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                Classes.Remove(existing);
            }

            // Ardından yeni veya güncellenmiş nesneyi ekle
            Classes.Add(NewClass);

            // Formu temizle
            NewClass = new ClassInformationModel();

            // Sayfayı yenile
            return RedirectToPage();
        }

        // Silme (Delete)
        public IActionResult OnPostDelete(int id)
        {
            var classToDelete = Classes.FirstOrDefault(c => c.Id == id);
            if (classToDelete != null)
            {
                Classes.Remove(classToDelete);
            }

            // İşlemden sonra sayfayı yenile
            return RedirectToPage();
        }

        // Düzenleme (Edit)
        public IActionResult OnPostEdit(int id)
        {
            var classToEdit = Classes.FirstOrDefault(c => c.Id == id);
            if (classToEdit != null)
            {
                // Yeni bir nesne oluştururken constructor auto-increment yapar,
                // bu yüzden varolan Id'yi korumak için elle atıyoruz.
                NewClass = new ClassInformationModel
                {
                    Id = classToEdit.Id,
                    ClassName = classToEdit.ClassName,
                    StudentCount = classToEdit.StudentCount,
                    Description = classToEdit.Description
                };

                // Liste içinden bu öğeyi geçici olarak kaldır
                Classes.Remove(classToEdit);
            }

            // Form doldurulmuş şekilde sayfayı göster
            return Page();
        }
    }
}
