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
    public class BookCopiesBusiness : IBookCopiesBusiness
    {
        private readonly IBookCopiesRepository _repo;

        public BookCopiesBusiness(IBookCopiesRepository repo)
        {
            _repo = repo;
        }

        public bool AddBookCopies(BookCopies bookCopies)
        {
            try
            {
                _repo.AddBookCopies(bookCopies);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteBookCopies(int id)
        {
            try { _repo.DeleteBookCopies(id); return true; }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<BookCopies> GetAllCopies()
        {
            return _repo.GetAllCopies();
        }

        public BookCopies GetBookCopies(int id)
        {
            return _repo.GetBookCopies(id);
        }

        public bool UpdateBookCopies(BookCopies bookCopies)
        {
            try
            {
                _repo.UpdateBookCopies(bookCopies);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
