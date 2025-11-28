using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using Model;

namespace BLL
{
    public class BookshelvesBusiness: IBookshelvesBusiness
    {
        private readonly IShelvesRepository _repo;
        public BookshelvesBusiness(IShelvesRepository repo)
        {
            _repo = repo;
        }
        public bool AddBookshelves(Bookshelves bookshelves)
        {
            try
            {
                _repo.AddBookshelves(bookshelves );
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool DeleteBookshelves(int id)
        {
            try { _repo.DeleteBookshelves(id); return true; }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Bookshelves> GetAllShelves()
        {
           return _repo.GetAllShelves();
        }

        public Bookshelves GetBookshelves(int id)
        {
            return _repo.GetBookshelves(id);
        }

        public Bookshelves GetShelfById(int BookshelvesId)
        {
            return _repo.GetShelfById(BookshelvesId);

        }

        public bool UpdateBookshelves(Bookshelves bookshelves)
        {
            try
            {
                _repo.UpdateBookshelves(bookshelves);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        
    }
}
