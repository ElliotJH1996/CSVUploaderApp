using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Application.Core.BookServices;
using Application.Core.Repositories;
using Application.Core.Models;

namespace Console_CSV_Uploader
{
    public class Upload
    {
      
        
        public static void SendFile(string fileName, IBookRepository _br, BookServices br)
        {
            try
            {

                using (var fs = File.OpenRead(fileName))
                {
                    string file = Path.GetFileName(fileName);
                    IFormFile csv = new FormFile(fs, 0, fs.Length, "csv", file);
                    br.InsertParsedBook(csv);
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
