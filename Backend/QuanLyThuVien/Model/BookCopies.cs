using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BookCopies
    {
        public int BookCopyId { get; set; }
        public int BookId { get; set; }
        public int BookshelfId { get; set; }
        public string Status { get; set; }
    }
}
