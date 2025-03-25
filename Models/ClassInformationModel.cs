using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        // static sayaç, her yeni kayıt eklendiğinde artacak
        private static int _nextId = 1;

        public ClassInformationModel()
        {
            // Constructor boş. ID, OnPostAdd()'de verilecek.
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name gereklidir.")]
        public string ClassName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Student Count pozitif bir sayı olmalıdır.")]
        public int StudentCount { get; set; }

        public string Description { get; set; } = string.Empty;

        // Yeni kayıt eklenirken ID atamak için çağrılacak yardımcı metot
        public static int GetNextId()
        {
            return _nextId++;
        }
    }
}
