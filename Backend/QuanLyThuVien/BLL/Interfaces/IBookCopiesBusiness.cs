using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BLL.Interfaces
{
    public interface IBookCopiesBusiness
    {
        public List<BookCopies> GetAllCopies();

        public bool AddBookCopies(BookCopies bookCopies); 

        public BookCopies GetBookCopies(int id);

        public bool UpdateBookCopies(BookCopies bookCopies);

        public bool DeleteBookCopies(int id); 
    }
}
