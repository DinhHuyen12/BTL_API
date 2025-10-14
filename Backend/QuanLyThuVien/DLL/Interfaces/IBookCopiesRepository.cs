using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL.Interfaces
{
    public interface IBookCopiesRepository
    {
        public List<BookCopies> GetAllCopies();

        public bool AddBookCopies(BookCopies bookCopies); //C -- Tạo
      
        public BookCopies GetBookCopies(int id); //R -- Tìm kiếm
       
        public bool UpdateBookCopies(BookCopies bookCopies); //U -- Cập nhật
       
        public bool DeleteBookCopies( int id); //D -- Xoá
     

      
    }
}
