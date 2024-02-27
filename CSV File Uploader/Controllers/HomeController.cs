using CSV_File_Uploader.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;


namespace CSV_File_Uploader.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            var origin = Request.Headers["referer"].ToString();
            if (!origin.Contains("localhost"))
            {
                return Unauthorized();
            }
            return View();
        }
        public IActionResult ShowAllBooks()
        {
            var origin = Request.Headers["referer"].ToString();
            if (!origin.Contains("localhost"))
            {
                return Unauthorized();
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
