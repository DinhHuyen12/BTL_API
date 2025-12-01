using QuanLyThuVien.DTO;
using Models;
using Microsoft.VisualBasic;

namespace DAL.Interfaces
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
