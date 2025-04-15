namespace MyRazorApp.Models
{
    // Tabloda gösterilecek bilgileri tutar; ID tabloda görüntülenmez ancak CRUD işlemleri için kullanılır.
    public class ClassInformationTable
    {
        public int Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int StudentCount { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
