namespace baiapi1.Models
{
    public class book
    {
        public int BookId { get; set; } 

        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public int PublicationYear { get; set; }
        public string Image {  get; set; }
    }
}
