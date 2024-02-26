using Application.Core.Models;
using System.Data;

namespace Application.Core.Repositories
{
    public interface IBookRepository
    {
        public int BulkBookInsert(List<Book> books);
        public int InsertBook(Book book);

        public int InsertType(int refID,string type);

        public int InsertGenre(int refID,string genre);

        public int GetTypeID(string type);

        public int GetGenreID(string genre);

        public int GetLatestGenreID();

        public int GetLatestTypeID();

        public ICollection<Book> ShowAllBooks();
        
    }
}
