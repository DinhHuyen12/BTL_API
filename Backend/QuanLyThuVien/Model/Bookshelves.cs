namespace Model
{
    public class Bookshelves
    {
        public int BookshelfId { get; set; }    // ID duy nhất
        public string ShelfCode { get; set; }   // Mã kệ, ví dụ: "A01"
        public string Location { get; set; }    // Vị trí kệ, ví dụ: "Tầng 1, Kệ B"
    }
}
