using System.Data;
using Application.Core.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Console_CSV_Uploader
{
    class Program
    {
        public static bool created = false;
        public static bool uploaded = false;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Start();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(GetConfiguredServices);

        public static void GetConfiguredServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddHostedService<CSVBackgroundServices>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IDbConnection>(e => new SqlConnection("Data Source=localhost;Initial Catalog=BookDB;Integrated Security=True;Trust Server Certificate=True;"));

        }


        public static void MainApplication(IBookRepository br)
        {
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("--------------------------- Book Uploader -------------------------");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine(SystemMessages.checkFolder);
            try
            {
                string filePath = AppSettings.GetFilePath();
                if (created == true)
                {
                    Console.WriteLine(SystemMessages.folderFound);
                    Console.WriteLine(SystemMessages.placeFileInFolder);
                    Console.WriteLine("Folder Path: " + filePath);
                    while (Console.ReadKey().Key != ConsoleKey.Enter)
                    {

                    }
                    ConsoleKey checkKey = Console.ReadKey(true).Key;
                    while (checkKey == ConsoleKey.Enter && Directory.GetFiles(filePath, "*.csv") == null)
                    {

                        Console.WriteLine(SystemMessages.noCSVFound);

                    }
                }
                else
                {

                    Console.WriteLine(SystemMessages.folderExists);


                }
                Console.WriteLine(SystemMessages.insertRecords);
                Console.WriteLine(SystemMessages.insertConfirmation);
                while (!Console.KeyAvailable)
                {

                }
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.Y)
                {
                    int fileCount = Directory.GetFiles(filePath).Count();
                    if (fileCount == 0)
                    {
                        Console.WriteLine(SystemMessages.noCSVFound);
                        Console.WriteLine(SystemMessages.closeApplication);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                    
                        var files = Directory.GetFiles(filePath);
                        int count = 0;
                        foreach (var file in files)
                        {
                            string fileType = Path.GetExtension(file);
                            if (!fileType.Equals(".csv"))
                            {
                                Console.WriteLine(SystemMessages.wrongFileType);
                                Console.WriteLine(SystemMessages.closeApplication);
                                Console.ReadLine();
                                Environment.Exit(0);
                            }
                            int t = files.Length;
                            string fileName = Path.GetFileName(file);
                            Console.WriteLine("Uploading records from file: " + fileName);

                            Upload.SendFile(file, br);

                            if (uploaded == true)
                            {
                                count++;
                                Console.WriteLine("All records from " + fileName + " have been successfully uploaded!");
                                if (files.Length == count)
                                {
                                    Console.WriteLine(SystemMessages.allFilesUploaded);
                                    Console.WriteLine(SystemMessages.closeApplication);
                                    Console.ReadLine();
                                    Environment.Exit(0);
                                }
                            }
                            else
                            {
                                Console.WriteLine(SystemMessages.errorMessage);
                                Console.ReadLine();
                                Environment.Exit(0);

                            }
                        }
                }
                if (key == ConsoleKey.N)
                {
                    Console.WriteLine(SystemMessages.uploadCancelled);
                    Console.WriteLine(SystemMessages.closeApplication);
                    Console.ReadLine();
                    Environment.Exit(0);


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
