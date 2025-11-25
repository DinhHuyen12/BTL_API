using System.Net;
using System.Reflection.Metadata;
using baiapi1.DAL.Interfaces;
using baiapi1.DTO;
using baiapi1.Models;
using DAL;
using DAL.Helper;
using Microsoft.VisualBasic;

namespace baiapi1.DAL
{
    public class BookRepository : IBookRepository
    {
        private readonly IDatabaseHelper _dbHelper;

        public BookRepository(IDatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;

        }
        public List<book> GetAll()
        {
            string msg = string.Empty;
            var dt = _dbHelper.ExecuteQueryToDataTable("select * from Books", out msg);
            return dt.ConvertTo<book>().ToList();
        }
        public book GetById(int id)
        {
            string msg = string.Empty;
            var dt = _dbHelper.ExecuteQueryToDataTable("select * from Books where BookId = '" + id + "'", out msg);
            return dt.ConvertTo<book>().FirstOrDefault();
        }
        public List<book> GetByCate(string TheLoai)
        {
            string msg = string.Empty;
            var dt = _dbHelper.ExecuteQueryToDataTable("select * from Books where Category =N'" + TheLoai + "'", out msg);
            return dt.ConvertTo<book>().ToList();
        }
        public book ten(string sach)
        {
            string msg = string.Empty;
            var dt = _dbHelper.ExecuteQueryToDataTable("select * from Books where Title =N'" + sach + "'", out msg);
            return dt.ConvertTo<book>().FirstOrDefault();
        }
         public List<book> tensach(int nam)
        {
            string msg = string.Empty;
            var dt = _dbHelper.ExecuteQueryToDataTable("select * from Books where PublicationYear =N'" + nam + "'", out msg);
            return dt.ConvertTo<book>().ToList();
        }

        public bool BorrowBook(BorrowBookDTO dto)
        {
            try
            {
                string msgError = "";
                var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_BorrowBook",
                    "@ReaderId", dto.ReaderId,
                    "@BookId", dto.BookId,
                    "@BorrowDate", dto.BorrowDate,
                    "@DueDate", dto.DueDate
                );

                if (!string.IsNullOrEmpty(msgError))
                {
                  
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
             
                return false;
            }
        }

        public List<LoanDTO> GetLoan(int id)
        {

            string msgError = "";
            var dt = _dbHelper.ExecuteSProcedureReturnDataTable(out msgError, "sp_GetBorrowedBooksByReader", "@ReaderId", id);

            return dt.ConvertTo<LoanDTO>().ToList();
        }
    }
}