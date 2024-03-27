using Application.Core.Models;
using Application.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
namespace Application.Core.BookServices
{
    public class BookServices
    {
        private readonly IBookRepository _br;
        private readonly ILogger<BookServices> _logger;

        public BookServices(IBookRepository br, ILogger<BookServices> logger)
        {
            _br = br;
            _logger = logger;
        }

        public int InsertParsedBook(IFormFile csv)
        {
            try
            {
				int insertedBooks = 0;
                var books = CSVParser.ParseCSV(csv);
				foreach (var book in books)
				{
					book.TypeID = GetTypeID(book.Type);
					book.GenreID = GetGenreID(book.Genre);
					_br.InsertBook(book);
					insertedBooks++;
				}
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
		public int GetTypeID(string type)
		{
			int typeId = _br.GetTypeID(type.ToLower());
			if (typeId == 0)
			{
				int newTypeID = _br.GetLatestTypeID() + 1;
				var insertType = _br.InsertType(newTypeID, type);
				int TypeID = _br.GetTypeID(type.ToLower());
				typeId = TypeID;
			}
			else
			{
				int refID = _br.GetLatestTypeID();
				typeId = refID;
			}

			return typeId;
		}

		public int GetGenreID(string genre)
		{
			int genreId = _br.GetGenreID(genre.ToLower());
			if (genreId == 0)
			{
				int newGenreID = _br.GetLatestGenreID() + 1;
				if (genre != "")
				{
					var insertGenre = _br.InsertGenre(newGenreID, genre);
				}
			}
			if (genre != "")
			{
				genreId = _br.GetGenreID(genre);
			}
			else
			{
				genreId = -1;
			}

			return genreId;
		}
	}
}
