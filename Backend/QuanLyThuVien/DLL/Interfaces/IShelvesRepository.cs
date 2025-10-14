using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace DAL.Interfaces
{
    public interface IShelvesRepository
    {
        public List<Bookshelves> GetAllCopies();

        public bool AddBookshelves  (Bookshelves bookshelves); //C -- Tạo
        // đầu vào: bookshelves
        // Đầu ra: thông báo them thành công, true, xong cmnr 
        public Bookshelves GetBookshelves (int id); //R -- Tìm kiếm
        // đầu vào: id 
        // Đầu ra: Bookshelves
        public bool UpdateBookshelves(Bookshelves bookshelves); //U -- Cập nhật
        // đầu vào: Bookshelves
        // Đầu ra: sửa thánh coong 

        public bool DeleteBookshelves(int id); //D -- Xoá
        // đầu vào: id 
        // Đầu ra: xoá thành công 
    }
}
