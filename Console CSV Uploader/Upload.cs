using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Application.Core.BookServices;
using Application.Core.Repositories;

namespace Console_CSV_Uploader
{
    public class Upload
    {
        
        
        public static void SendFile(string fileName, IBookRepository _br)
        {
            try
            {

                using (var fs = File.OpenRead(fileName))
                {


                    string file = Path.GetFileName(fileName);
                    IFormFile csv = new FormFile(fs, 0, fs.Length, "csv", file);
                    List<Application.Core.Models.Book> books = CSVParser.FormatCSVtoTable(csv);

                    var addBooks = _br.BulkBookInsert(books);

                    Program.uploaded = true;


                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception(e.Message);
            }

        }
    }
}
