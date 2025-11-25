using baiapi1.DTO;
using baiapi1.Models;
using Microsoft.VisualBasic;

namespace baiapi1.DAL.Interfaces
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
