using System.ComponentModel.DataAnnotations;

namespace Library.BLL.DTOs
{
    public class BookRequestDTO
    {
        [Range(0, int.MaxValue)]    
        public int? Id { get; set; }
        public string Title { get; set; }
        [RegularExpression("^data:image\\/[a-zA-Z]+;base64,([^\\s]+)$", ErrorMessage = "Invalid image (base64)")]
        public string Cover { get; set; }
        public string Content { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
    }
}
