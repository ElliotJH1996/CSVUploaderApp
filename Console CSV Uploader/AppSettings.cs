using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_CSV_Uploader
{
    public class AppSettings
    {
        public static string uri = "https://localhost:5002/api/Book/";
        public static string GetFilePath()
        {
            
            string folderName = "CSV Uploader Files";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), folderName);;
            
            if (!Directory.Exists(filePath))
            {
                try
                {
                    Directory.CreateDirectory(filePath);
                    Program.created = true;
                }
                catch (Exception)
                {

                    throw new Exception("Unable to create directory for files");
                }

            }
          
            

            return filePath;
        }
    }
}
