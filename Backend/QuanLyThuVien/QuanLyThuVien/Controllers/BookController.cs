using DAL.Interfaces;
using QuanLyThuVien.DTO;
using Models;
using Microsoft.AspNetCore.Mvc;

namespace QuanLyThuVien.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //localhost/api/book/get-a
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public BookController(IBookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }
        [HttpGet("get-all")]
        public List<book> Get()
        {
            return _bookRepo.GetAll();
        }
        [HttpGet("get-all/{id}")]
        public book GetById(int id)
        {
            return _bookRepo.GetById(id);
        }
        [HttpGet("get-by-cate")]
        public List<book> GetByCate([FromQuery] string theLoai)
        {
            return _bookRepo.GetByCate(theLoai);
        }
        [HttpGet("get-by-name")]
        public book ten([FromQuery] string sach)
        {
            return _bookRepo.ten(sach);
        }
        [HttpGet("get-by-year")]
        public List<book> tensach([FromQuery] int nam)
        {
            return _bookRepo.tensach(nam);
        }
        // Mượn sách
        [HttpPost("borrow")]
        public IActionResult BorrowBook([FromBody] BorrowBookDTO dto)
        {
            try
            {
                var result = _bookRepo.BorrowBook(dto);

                if (result)
                    return Ok(new { message = "Mượn sách thành công" });
                else
                    return BadRequest(new { message = "Mượn sách thất bại" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }
        [HttpGet("get-loan")]
        public List<LoanDTO> LoanUser([FromQuery] int userId)
        {
            return _bookRepo.GetLoan(userId);
        }
    }
}
