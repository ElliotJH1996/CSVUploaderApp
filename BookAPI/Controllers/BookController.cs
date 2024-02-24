using Application.Core.BookServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _log;
        private readonly BServices _bs;
        public BookController(ILogger<BookController> log, BServices bs)
        {
            _log = log;
            _bs = bs;
        }

        [HttpPost]
        public IActionResult Upload([FromForm] IFormFile csv)
        {
            try
            {
                if (!Path.GetExtension(csv.FileName).Equals(".csv"))
                {
                    return BadRequest("This uploader uses .CSV files only, please try again!");
                }

                var addBooks = _bs.InsertParsedBook(csv);
                _log.LogInformation(+addBooks + " Books have been added to the DB");
                return Ok(+addBooks + " Books have been successfully uploaded");


            }
            catch (Exception e)
            {

                _log.LogError(e, e.Message);
                return BadRequest("Error: " + e.Message);
            }


        }

        [HttpGet]
        public IActionResult ShowAllBooks()
        {
            try
            {
                string json = _bs.GetAllBooks();
                return Ok(json);
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                return BadRequest("Error: " + e.Message);

            }
        }

    }
}
