namespace Library.BLL.DTOs
{
    public class BookDetailsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public string Rating { get; set; }
        public IEnumerable<ReviewDTO> Reviews { get; set; }
    }
}
