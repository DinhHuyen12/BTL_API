using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace QuanLyThuVien.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookshelvesController : Controller
    {
        private readonly IBookshelvesBusiness _bus;
        public BookshelvesController(IBookshelvesBusiness bus) 
        {
            _bus = bus;
        }
        [HttpGet]
        public List<Bookshelves> GetAll()
        {
            return _bus.GetAllShelves();
        }
        [HttpGet("{id}")]
        public Bookshelves Get(int id)
        {
            return _bus.GetBookshelves(id);
        }
        [HttpDelete("remove")]
        public bool Delete([FromQuery] int id)
        {
            return (_bus.DeleteBookshelves(id));
        }
        [HttpPut("update")]
        public bool Update([FromBody] Bookshelves bookshelves)
        {
            return _bus.UpdateBookshelves(bookshelves);
        }
        [HttpPost("add")]
        public bool Add([FromBody] Bookshelves bookshelves)
        {
            return _bus.AddBookshelves(bookshelves);
        }

        [HttpGet("get-by-id/{id}")]
        public Bookshelves GetShelfById(int id)
        {
            return _bus.GetShelfById(id);
        }
    }
}
