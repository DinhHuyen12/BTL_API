using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace QuanLyThuVien.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookCopiesController : ControllerBase
    {
        private readonly IBookCopiesBusiness _bus;
        public BookCopiesController(IBookCopiesBusiness bus) 
        {
            _bus = bus;
        }
        [HttpGet]
        public List<BookCopies> GetAll()
        {
            return _bus.GetAllCopies();
        }
        [HttpGet("{id}")]
        public BookCopies Get(int id)
        {
            return _bus.GetBookCopies(id);
        }
        [HttpDelete("remove")]
        public bool Delete([FromQuery] int id)
        {
            return (_bus.DeleteBookCopies(id));
        }
        [HttpPut("update")]
        public bool Update([FromBody] BookCopies bookCopies)
        {
            return _bus.UpdateBookCopies(bookCopies);
        }
        [HttpPost("add")]
        public bool Add([FromBody] BookCopies bookCopies)
        {
            return _bus.AddBookCopies(bookCopies);
        }
    }
}
