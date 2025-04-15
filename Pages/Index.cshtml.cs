using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyRazorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        // Uygulama çalıştığı sürece bellekte kalacak liste
        private static List<ClassInformationModel> _storage = new List<ClassInformationModel>();

        // Form ile gönderilen yeni kayıt bilgileri (Add veya Edit)
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new ClassInformationModel();

        // Filtre için query parametresi; GET üzerinden alınır.
        [BindProperty(SupportsGet = true)]
        public string? Filter { get; set; }

        // Sayfalama için query parametresi; GET üzerinden alınır.
        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        // Sayfa başına gösterilecek kayıt sayısı
        public int PageSize { get; set; } = 10;

        // Toplam sayfa sayısı
        public int TotalPages { get; set; }

        // Filtrelenip, sıralanıp sayfalanmış kayıtları bu listede tutacağız
        public List<ClassInformationTable> FilteredClasses { get; set; } = new List<ClassInformationTable>();

        // Tüm kayıtlar (CRUD için)
        public List<ClassInformationModel> Classes { get; set; } = new List<ClassInformationModel>();

        public void OnGet()
        {
            // Eğer hiç eklenmemişse sadece 1 kere 100 kayıt oluşturur:
            if (_storage.Count == 0)
            {
                for (int i = 1; i <= 100; i++)
                {
                    _storage.Add(new ClassInformationModel
                    {
                        Id = ClassInformationModel.GetNextId(), // 1 den 100'e ID atanır
                        ClassName = $"Sample Class {i}",
                        StudentCount = i * 10,
                        Description = $"Description for Class {i}"
                    });
                }
            }

            // Filtreleme
            var query = _storage.AsQueryable();
            if (!string.IsNullOrWhiteSpace(Filter))
            {
                query = query.Where(x => x.ClassName.Contains(Filter, StringComparison.OrdinalIgnoreCase));
            }

            // Sıralama: ID büyükten küçüğe -> En yeni kayıt en üstte
            query = query.OrderByDescending(x => x.Id);

            // Sayfalama
            int totalRecords = query.Count(); 
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            if (Page < 1)
                Page = 1;
            if (Page > TotalPages && TotalPages > 0)
                Page = TotalPages;

            var pagedData = query
                .Skip((Page - 1) * PageSize)
                .Take(PageSize);

            // ClassInformationModel -> ClassInformationTable dönüştürme
            FilteredClasses = pagedData
                .Select(x => new ClassInformationTable
                {
                    Id = x.Id,
                    ClassName = x.ClassName,
                    StudentCount = x.StudentCount,
                    Description = x.Description
                })
                .ToList();

            // CRUD işlemleri için orijinal liste de elimizin altında dursun
            Classes = _storage;
        }

        public IActionResult OnPostAdd()
        {
            // Model validasyonu (zorunlu alanlar, range vs.)
            if (!ModelState.IsValid)
            {
                OnGet();  // Tabloyu tekrar hazırlıyoruz
                return Page();
            }

            // Edit modundaysa eski kaydı silip yenisini ekleriz
            var existing = _storage.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existing != null)
            {
                _storage.Remove(existing);
            }
            else
            {
                // Yeni kayıt ekleniyorsa ID verelim
                NewClass.Id = ClassInformationModel.GetNextId();
            }

            _storage.Add(NewClass);

            // Formu temizle
            NewClass = new ClassInformationModel();

            // Yeni kayıt en büyük ID ile eklendi, ID’ye göre Descending sıralama yapıldığı için
            // Page=1’de gözükecek. Filtreyi koruyorsanız, eklenen kaydın ClassName’i fitreye
            // uygun değilse tablodan gizli kalabilir. Bunu istemiyorsanız Filter="" yapabilirsiniz.
            return RedirectToPage("/Index", new { Page = 1, Filter = this.Filter });
        }

        public IActionResult OnPostDelete(int id)
        {
            var toDelete = _storage.FirstOrDefault(c => c.Id == id);
            if (toDelete != null)
            {
                _storage.Remove(toDelete);
            }
            // Aynı sayfada, aynı filtreyle kalalım
            return RedirectToPage("/Index", new { Page, Filter });
        }

        public IActionResult OnPostEdit(int id)
        {
            // Kayıt form alanlarına yüklensin
            var toEdit = _storage.FirstOrDefault(c => c.Id == id);
            if (toEdit != null)
            {
                NewClass.Id = toEdit.Id;
                NewClass.ClassName = toEdit.ClassName;
                NewClass.StudentCount = toEdit.StudentCount;
                NewClass.Description = toEdit.Description;

                // Kayıt geçici olarak listeden çıkarılır.
                _storage.Remove(toEdit);
            }

            // Listeyi güncel sekilde doldurup formda veriyi gösterelim
            OnGet();
            return Page();
        }
    }
}
