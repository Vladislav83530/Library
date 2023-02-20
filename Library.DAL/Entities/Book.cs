namespace Library.DAL.Entities
{
    internal class Book
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Cover { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }

        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
    }
}
