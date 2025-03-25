using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        // Auto-increment için static sayaç
        private static int _nextId = 1;

        public ClassInformationModel()
        {
            // Her yeni nesne oluşturulduğunda Id otomatik artar.
            Id = _nextId++;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name gereklidir.")]
        public string ClassName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Student Count pozitif bir sayı olmalıdır.")]
        public int StudentCount { get; set; }

        public string Description { get; set; }
    }
}
