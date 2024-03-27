using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using Application.Core.Models;

namespace Application.Core.BookServices
{
    public class CSVParser
    {
		public static List<Book> ParseCSV(IFormFile csv)
        {
            var importedBooks = new List<Book>();
            string regexFormat = "((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))";
            try
            {
                using (var stream = new StreamReader(csv.OpenReadStream()))
                {
                    while (!stream.EndOfStream)
                    {
                        var csvData = stream.ReadToEnd();
                        string[] streamData = csvData.Split("\r\n");
                        foreach (var books in streamData.Skip(1))
                        {
                            MatchCollection bookData = new Regex(regexFormat).Matches(books);

                            importedBooks.Add(new Book
                            {
                                Title = bookData[0].ToString().Trim(),
                                ISBN10 = bookData[1].ToString().Trim(),
                                Pages = bookData[2].ToString().Trim(),
                                Type = bookData[3].ToString().Trim(),
                                Genre = bookData[4].ToString().Trim(),
                                Authors = bookData[5].ToString().Trim(),
                                Price = bookData[6].ToString().Trim(),
                                PublishDate = bookData[7].ToString().Trim(),

                            });
                        }
                    }
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return importedBooks;
        }



    }
}
