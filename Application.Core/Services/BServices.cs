using Application.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace Application.Core.BookServices
{
    public class BServices
    {
        private readonly IBookRepository _br;
        private readonly ILogger<BServices> _logger;

        public BServices(IBookRepository br, ILogger<BServices> logger)
        {
            _br = br;
            _logger = logger;
        }

        public int InsertParsedBook(IFormFile csv)
        {
            try
            {
                var books = CSVParser.ParseCSV(csv);
                int insertedBooks = _br.BulkBookInsert(books);

                return insertedBooks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "InsertParsedBook");
                throw;
            }

        }
        
        public string GetAllBooks() 
        {
            try
            {
                var allBooks = _br.ShowAllBooks();
                string json = JsonConvert.SerializeObject(allBooks);

                return json;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllBooks");
                throw;
            }
          
        }

    }
}
