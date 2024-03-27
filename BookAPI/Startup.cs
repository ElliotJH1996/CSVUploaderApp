using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Serilog;
using CSV_File_Uploader;
using Application.Core.BookServices;
using Application.Core.Repositories;
using Application.Core.Repositories.Users;
using Application.Core.Services;


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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(e =>
            {
                e.ClearProviders();
                e.AddSerilog(dispose: true);
            });
            services.AddScoped<IDbConnection>(e => new SqlConnection(Configuration.GetConnectionString("Default")));
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IUsers, Users>();
            services.AddScoped<BookServices>();
            services.AddScoped<UserServices>();
			services.AddCors(options =>
            {
                options.AddPolicy(name: allowCors,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://localhost:44396")
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
				app.UseMiddleware<IdentityAuthMW>();
				app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookAPI v1"));
            } 
            app.UseHttpsRedirection();

            app.UseMiddleware<IdentityAuthMW>();

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
