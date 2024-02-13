using Dapper;
using System;
using System.Data;
using Serilog;
using Microsoft.Extensions.Logging;
using System.Data.Common;
using System.Reflection;

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

        public int InsertBook(Models.Book book)
        {
            int recCount;
            try
            {
                if (book.Pages.Equals("null"))
                {
                    book.Pages = null;
                }

                if (book.Genre.Equals("null"))
                {
                    book.Genre = null;
                }

                if (book.Type.Equals("null"))
                {
                    book.Type = null;
                }

                int typeId = GetTypeID(book.Type);
                if (typeId == 0)
                {
                    int newTypeID = GetLatestTypeID() + 1;
                    var insertType = InsertType(newTypeID, book.Type);
                }
                else
                {
                    int refID = GetLatestTypeID();
                }
                int genreId = GetGenreID(book.Genre);
                if (genreId == 0)
                {
                    int newGenreID = GetLatestGenreID() + 1;
                    if (book.Genre != null)
                    {
                        var insertGenre = InsertGenre(newGenreID, book.Genre);
                    }
                }
                if (book.Genre != null)
                {
                    book.GenreID = GetGenreID(book.Genre);
                }
                else
                {
                    book.GenreID = -1;
                }
                book.TypeID = GetTypeID(book.Type);
                var storedProcedureName = "sp_InsertBook";
                var result = _db.Execute(storedProcedureName, book, commandType: CommandType.StoredProcedure);
                recCount = result;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
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
                _log.LogError(e.Message);
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
                _log.LogError(e.Message);
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
                _log.LogError(e.Message);
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
                _log.LogError(e.Message);
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
                _log.LogError(e.Message);
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
                _log.LogError(e.Message);
                throw;
            }

        }

        public int BulkBookInsert(List<Models.Book> books)
        {
            int recCount = 0;
            foreach (Models.Book book in books)
            {
                try
                {
                    var result = InsertBook(book);
                    recCount++;

                }
                catch (Exception e)
                {
                    _log.LogError(e.Message);
                    throw;

                }
            }

            return recCount;
        }

        public DataTable ShowAllBooks()
        {
            try
            {
                DataTable r = new DataTable();
                r.Columns.Add("Id");
                r.Columns.Add("Title");
                r.Columns.Add("ISBN-10");
                r.Columns.Add("Pages");
                r.Columns.Add("Type");
                r.Columns.Add("Genre");
                r.Columns.Add("Authors");
                r.Columns.Add("Price");
                r.Columns.Add("Publish Date");

                var storedProcedureName = "sp_ShowAllBooks";

                var result = _db.Query(storedProcedureName, commandType: CommandType.StoredProcedure);
                List<string> items = new List<string>();
                foreach (var book in result)
                {

                    var value = book as IDictionary<string, object>;
                    var pages = "";
                    var type = "";
                    var genre = "";
                    var id = value["Id"].ToString();
                    var title = value["Title"].ToString();
                    var isbn10 = value["ISBN-10"].ToString();
                    if (value["Pages"] == null)
                    {
                        pages = "NULL";
                    }
                    else
                    {
                        pages = value["Pages"].ToString();
                    }
                    if (value["Type"] == null)
                    {
                        type = "NULL";
                    }
                    else
                    {
                        type = value["Type"].ToString();
                    }
                    if (value["Genre"] == null)
                    {
                        genre = "NULL";
                    }
                    else
                    {
                        genre = value["Genre"].ToString();
                    }
                    var authors = value["Authors"].ToString();
                    var price = value["Price"].ToString();
                    var publishDate = Convert.ToDateTime(value["Publish Date"]).ToString("dd-MMM-yy");

                    items.Add(id);
                    items.Add(title);
                    items.Add(isbn10);
                    items.Add(pages);
                    items.Add(type);
                    items.Add(genre);
                    items.Add(authors);
                    items.Add(price);
                    items.Add(publishDate);

                    int count = items.Count();
                    DataRow row = r.NewRow();
                    row.ItemArray = items.Skip(count - 9).Take(9).ToArray();
                    r.Rows.Add(row);


                }
                return r;
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
                throw;
            }


        }

    }
}


