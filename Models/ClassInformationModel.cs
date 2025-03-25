using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        // static sayaç, her yeni kayıt eklendiğinde artacak
        private static int _nextId = 1;

        // Artık constructor'da _nextId++ YOK!
        public ClassInformationModel()
        {
            // Boş constructor
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name gereklidir.")]
        public string ClassName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Student Count pozitif bir sayı olmalıdır.")]
        public int StudentCount { get; set; }

        public string Description { get; set; } = string.Empty;

        // Her yeni eklemede ID atayacak yardımcı metot
        public static int GetNextId()
        {
            return _nextId++;
        }
    }
}
