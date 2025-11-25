using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Helper;
using DAL.Interfaces;
using Model;

namespace DAL
{
    public class BookCopiesRepository : IBookCopiesRepository
    {
        private readonly IDatabaseHelper _databaseHelper;

        public BookCopiesRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public bool AddBookCopies(BookCopies bookCopies)
        {
            try
            {
                string q = "insert into BookCopies (BookId, BookshelfId, Status) " +
                 "VALUES ('" + bookCopies.BookId + "', '" + bookCopies.BookshelfId + "', '" + bookCopies.Status + "')";
                _databaseHelper.ExecuteNoneQuery(q);
                return true;
            }
            catch (Exception ex) { return false; }

        }

        public bool DeleteBookCopies(int id)
        {
            try
            {
                string q = "delete from BookCopies where BookCopyId = '" + id + "'";
                _databaseHelper.ExecuteNoneQuery(q);
                return true;

            }
            catch
            {
                return false;
            }
        }

        public List<BookCopies> GetAllCopies()
        {
            string msg = string.Empty;
            string q = "select * from BookCopies";
            var dt = _databaseHelper.ExecuteQueryToDataTable(q, out msg);
            return dt.ConvertTo<BookCopies>().ToList();

        }

        public BookCopies GetBookCopies(int id)
        {
            string msg = string.Empty;
            string q = "select * from BookCopies  where BookCopyId = '" + id + "'";
            var dt = _databaseHelper.ExecuteQueryToDataTable(q, out msg);
            return dt.ConvertTo<BookCopies>().FirstOrDefault();
        }

        public bool UpdateBookCopies(BookCopies bookCopies)
        {

            try
            {
                string q = "update BookCopies set BookId = '" + bookCopies.BookId + "'," +
               "BookshelfId = '" + bookCopies.BookshelfId + "'," +
               "Status = '" + bookCopies.Status + "'" +
               "where BookCopyId = '" + bookCopies.BookCopyId + "'";
                _databaseHelper.ExecuteNoneQuery(q); return true;
            }
            catch { return false; }
        }
    }
}
