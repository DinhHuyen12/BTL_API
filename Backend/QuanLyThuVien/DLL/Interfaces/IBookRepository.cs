using QuanLyThuVien.DTO;
using QuanLyThuVien.Models;
using Microsoft.VisualBasic;

namespace QuanLyThuVien.DAL.Interfaces
{
    public interface IBookRepository
    {
        List<book> GetAll();
        book GetById(int id);
        
        List<book> GetByCate(string TheLoai);

        book ten(string sach);
        List<book> tensach(int nam);

        bool BorrowBook(BorrowBookDTO dto);

        List<LoanDTO> GetLoan(int id);
    }
}
