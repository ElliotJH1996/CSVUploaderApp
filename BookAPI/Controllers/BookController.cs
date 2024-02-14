using Application.Core.Repositories;
using Application.Core.BookServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IDbConnection _db;
        private readonly IBookRepository _br;
        private readonly ILogger<BookController> _log;
        public BookController(IDbConnection db, IBookRepository br, ILogger<BookController> log)
        {
            _db = db;
            _br = br;
            _log = log;
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
                List<Application.Core.Models.Book> books = CSVParser.FormatCSVtoTable(csv);

                var addBooks = _br.BulkBookInsert(books);

                _log.LogInformation(+addBooks + " Books have been added to the DB");
                return Ok(+addBooks + " Books have been successfully uploaded");


            }
            catch (Exception e)
            {

                _log.LogError(e, e.Message);
                return BadRequest("Error: " + e);
            }


        }

        [HttpGet]
        public IActionResult ShowAllBooks()
        {
            try
            {
                var ShowAllBooks = _br.ShowAllBooks();
                string json = JsonConvert.SerializeObject(ShowAllBooks);
                return Ok(json);
            }
            catch (Exception e)
            {
                _log.LogError(e, e.Message);
                return BadRequest("Error: " + e);

            }
        }

    }
}
