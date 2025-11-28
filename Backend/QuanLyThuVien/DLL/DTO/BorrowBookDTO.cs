namespace QuanLyThuVien.DTO
{
    public class BorrowBookDTO
    {
        public int ReaderId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}
