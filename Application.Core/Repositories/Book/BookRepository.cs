using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;
using Application.Core.Models;
namespace Application.Core.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ILogger<BookRepository> _log;
        private readonly IDbConnection _db;
        public BookRepository(IDbConnection db, ILogger<BookRepository> log)
        {
            _db = db;
            _log = log;
        }

        public int InsertBook(Book book)
        {
			int recCount;
			try
			{
				int typeId = GetTypeID(book.Type.ToLower());
				if (typeId == 0)
				{
					int newTypeID = GetLatestTypeID() + 1;
					var insertType = InsertType(newTypeID, book.Type);
                    book.TypeID = GetTypeID(book.Type.ToLower());
                }
				else
				{
					int refID = GetLatestTypeID();
                    book.TypeID = refID;
                }
              
                int genreId = GetGenreID(book.Genre.ToLower());
				if (genreId == 0)
				{
					int newGenreID = GetLatestGenreID() + 1;
					if (book.Genre != "")
					{
						var insertGenre = InsertGenre(newGenreID, book.Genre);
					}
				}
				if (book.Genre != "")
				{
					book.GenreID = GetGenreID(book.Genre);
				}
				else
				{
					book.GenreID = -1;
				}
				var storedProcedureName = "sp_InsertBook";
				var result = _db.Execute(storedProcedureName, book, commandType: CommandType.StoredProcedure);
				recCount = result;
			}
			catch (Exception e)
			{
				_log.LogError(e,"InsertBook");
				throw;
			}

			return 1;
		}

        public int GetTypeID(string type)
        {

            try
            {
                var storedProcedureName = "sp_GetTypeID";
                var result = _db.QuerySingleOrDefault<int>(storedProcedureName, new { type }, commandType: CommandType.StoredProcedure);

                return result;

            }
            catch (Exception e)
            {
                _log.LogError(e,"GetTypeID");
                throw;
            }



        }

        public int InsertType(int refID, string type)
        {
            int id = 0;
            try
            {
             
                var storedProcedureName = "sp_InsertType";


                var result = _db.Execute(storedProcedureName, new { refID, type }, commandType: CommandType.StoredProcedure);

                id = result;

            }
            catch (Exception e)
            {
                _log.LogError(e, "InsertType");
                throw;
            }


            return id;
        }
        public int GetGenreID(string genre)
        {

            try
            {
                var storedProcedureName = "sp_GetGenreID";

                var result = _db.QuerySingleOrDefault<int>(storedProcedureName, new { genre }, commandType: CommandType.StoredProcedure);

                return result;

            }
            catch (Exception e)
            {
                _log.LogError(e, "GetGenreID");
                throw;
            }

        }

        public int GetLatestTypeID()
        {

            try
            {
                var storedProcedureName = "sp_GetLatestTypeID";

                var result = _db.QuerySingleOrDefault<int>(storedProcedureName, commandType: CommandType.StoredProcedure);

                return result;

            }
            catch (Exception e)
            {
                _log.LogError(e, "GetLatestTypeID");
                throw;
            }

        }
        public int InsertGenre(int refID, string genre)
        {
            int id = 0;

            try
            {
                var storedProcedureName = "sp_InsertGenre";

                var result = _db.Execute(storedProcedureName, new { refID, genre }, commandType: CommandType.StoredProcedure);

                id = result;

            }
            catch (Exception e)
            {
                _log.LogError(e, "InsertGenre");
                throw;
            }


            return id;
        }
        public int GetLatestGenreID()
        {

            try
            {
                var storedProcedureName = "sp_GetLatestGenreID";

                var result = _db.QuerySingleOrDefault<int>(storedProcedureName, commandType: CommandType.StoredProcedure);

                return result;

            }
            catch (Exception e)
            {
                _log.LogError(e, "GetLatestGenreID");
                throw;
            }

        }

        public int BulkBookInsert(List<Book> books)
        {
            int recCount = 0;
            foreach (Book book in books)
            {
                try
                {
                    var result = InsertBook(book);
                    recCount++;

                }
                catch (Exception e)
                {
                    _log.LogError(e, "BulkBookInsert");
                    throw;

                }
            }

            return recCount;
        }

        public ICollection<Book> ShowAllBooks()
        {
            try
            {
                ICollection<Book> books = new List<Book>();
                var storedProcedureName = "sp_ShowAllBooks";
                var result = _db.Query<Book>(storedProcedureName, commandType: CommandType.StoredProcedure);
               
                foreach (var book in result)
                {
                    books.Add(new Book
                    {
                        Title = book.Title,
                        ISBN10 = book.ISBN10,
                        Pages = book.Pages,
                        Type = book.Type,
                        Genre = book.Genre,
                        Authors = book.Authors,
                        Price = book.Price,
                        PublishDate = book.PublishDate,
                    });
                }
                return books;
            }
            catch (Exception e)
            {
                _log.LogError(e,"ShowAllBooks");
                throw;
            }


        }

    }
}


