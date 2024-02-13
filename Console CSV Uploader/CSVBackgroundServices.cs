using Application.Core.Repositories;
using Microsoft.Extensions.Hosting;


namespace Console_CSV_Uploader
{
    public class CSVBackgroundServices : BackgroundService
    {
        public readonly IBookRepository _br;
        

        public CSVBackgroundServices(IBookRepository br)
        {
            _br = br;
            
        }
        public override Task StartAsync(CancellationToken stoppingToken)
        {
            Program.MainApplication(_br);
            return base.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
           
            return base.StopAsync(stoppingToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            return Task.CompletedTask;
        }
    }
}
