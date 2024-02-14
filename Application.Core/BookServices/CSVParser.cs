using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text.RegularExpressions;
namespace Application.Core.BookServices
{
    public class CSVParser
    {
        public static List<Models.Book> FormatCSVtoTable(IFormFile csv)
        {
            List<string> bookItem = new List<string>();
            DataTable dto = new DataTable();
            var importedBooks = new List<Application.Core.Models.Book>();
            string regexFormat = "((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))";
            try
            {
                using (var stream = new StreamReader(csv.OpenReadStream()))
                {
                    while (!stream.EndOfStream)
                    {
                        var csvData = stream.ReadToEnd();
                        string[] streamData = csvData.Split("\r\n");
                        foreach (var books in streamData)
                        {
                            MatchCollection bookData = new Regex(regexFormat).Matches(books);

                            foreach (var item in bookData)
                            {
                                if (item.ToString().Length == 0)
                                {
                                    string fmtItem = "null";
                                    bookItem.Add(fmtItem);
                                }
                                else
                                {
                                    bookItem.Add(item.ToString());
                                }
                            }

                        }

                    }

                }

                var headers = bookItem.Take(8).ToList();
                var data = bookItem.Skip(8).ToList();

                foreach (var item in headers)
                {
                    dto.Columns.Add(item);

                }

                for (int i = 8; i < data.Count + 8; i += 8)
                {
                    var row = bookItem.Skip(i).Take(8).ToList();
                    var book = new Application.Core.Models.Book();

                    book.Title = row[0].Trim();
                    book.ISBN10 = row[1].Trim();
                    book.Pages = row[2].Trim();
                    book.Type = row[3].Trim();
                    book.Genre = row[4].Trim();
                    book.Authors = row[5].Trim();
                    book.Price = row[6].Trim();
                    book.PublishDate = row[7].Trim();

                    importedBooks.Add(book);

                    dto.Rows.Add(book.Title, book.ISBN10, book.Pages, book.Type, book.Genre, book.Authors, book.Price, book.PublishDate);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
           
          
            return importedBooks;
        }         


       
    }
}
