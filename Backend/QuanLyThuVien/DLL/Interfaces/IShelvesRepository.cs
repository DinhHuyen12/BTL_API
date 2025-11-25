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
        public List<Bookshelves> GetAllShelves();

        public bool AddBookshelves  (Bookshelves bookshelves);

        public Bookshelves GetBookshelves (string id); 
 
        public bool UpdateBookshelves(Bookshelves bookshelves);


        public bool DeleteBookshelves(int id); 

        public Bookshelves GetShelfById (int BookshelvesId); 

    }
}
