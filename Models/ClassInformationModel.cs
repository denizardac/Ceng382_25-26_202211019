using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        // Her yeni kayıt için artan statik sayaç
        private static int _nextId = 1;

        public ClassInformationModel()
        {
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name gereklidir.")]
        public string ClassName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Student Count pozitif bir sayı olmalıdır.")]
        public int StudentCount { get; set; }

        public string Description { get; set; } = string.Empty;

        // Yeni kayıt eklenirken ID atamak için yardımcı metot
        public static int GetNextId()
        {
            return _nextId++;
        }
    }
}
