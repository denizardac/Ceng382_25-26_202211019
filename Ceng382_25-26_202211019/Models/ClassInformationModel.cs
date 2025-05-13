using System.ComponentModel.DataAnnotations;

namespace MyRazorApp.Models
{
    public class ClassInformationModel
    {
        private static int _nextId = 1;
        public int Id { get; set; }

        [Required(ErrorMessage = "Class Name gereklidir.")]
        public string ClassName { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Student Count pozitif bir sayı olmalıdır.")]
        public int StudentCount { get; set; }

        public string Description { get; set; } = string.Empty;

        public static int GetNextId() => _nextId++;
    }
}
