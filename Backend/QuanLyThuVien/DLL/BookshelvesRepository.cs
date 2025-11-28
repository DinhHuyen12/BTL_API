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
    public class BookshelvesRepository : IShelvesRepository
    {
        private readonly IDatabaseHelper _data;
        public BookshelvesRepository(IDatabaseHelper data)
        {
            _data = data;
        }

     

        public bool AddBookshelves(Bookshelves bookshelves)
        {
            try
            {
                string q = "INSERT INTO Bookshelves (ShelfCode, Location)" +
                "VALUES ('" + bookshelves.ShelfCode+ "', '" + bookshelves.Location+ "')";
                _data.ExecuteNoneQuery(q);
                return true;
            }   
            catch (Exception ex) { return false; }
        }

        public bool DeleteBookshelves(int id)
        {
            try
            {
                string q = "delete from Bookshelves where BookshelfId = '" + id + "'";
                _data.ExecuteNoneQuery(q);
                return true;

            }
            catch
            {
                return false;
            }
        }


        public List<Bookshelves> GetAllShelves()
        {
            string msg = string.Empty;
            string q = "select * from Bookshelves";
            var dt = _data.ExecuteQueryToDataTable(q, out msg);
            return dt.ConvertTo<Bookshelves>().ToList();
        }

        public Bookshelves GetBookshelves(string id)
        {
            string msg = string.Empty;
            string q = "select * from Bookshelves where ShelfCode = '" + id + "'";
            var dt = _data.ExecuteQueryToDataTable(q, out msg);
            return dt.ConvertTo<Bookshelves>().FirstOrDefault();

        }

        public bool UpdateBookshelves(Bookshelves bookshelves)
        {
            try
            {
                string q = "UPDATE Bookshelves SET " +
                           "ShelfCode = '" + bookshelves.ShelfCode + "', " +
                           "Location = '" + bookshelves.Location + "' " +
                           "WHERE BookshelfId = '" + bookshelves.BookshelfId + "'";
                _data.ExecuteNoneQuery(q);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Bookshelves GetShelfById(int BookshelvesId )
        {
            string msg = string.Empty;
            string q = "select * from Bookshelves where BookshelfId  = '" + BookshelvesId + "'";
            var dt = _data.ExecuteQueryToDataTable(q, out msg);
            return dt.ConvertTo<Bookshelves>().FirstOrDefault();

        }
    }
}
