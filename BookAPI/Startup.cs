using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Data.Entity;
using System.Data;
using Microsoft.Data.SqlClient;
using Serilog;


namespace BookAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        string allowCors = "Access-Control-Allow-Origin";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(e =>
            {
                e.ClearProviders();
                e.AddSerilog(dispose: true);
            });
            services.AddScoped<IDbConnection>(e => new SqlConnection(Configuration.GetConnectionString("Default")));
            services.AddTransient<Application.Core.Repositories.IBookRepository, Application.Core.Repositories.BookRepository>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: allowCors,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:5002/api/Book",
                                                         "https://localhost:5001")
                                                        .AllowAnyHeader()
                                                        .AllowAnyMethod();
                                  });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookAPI", Version = "v1" });
            });

               }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookAPI v1"));
            } 
            app.UseHttpsRedirection();

            app.UseCors(allowCors);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
