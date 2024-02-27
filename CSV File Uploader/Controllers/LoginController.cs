using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Net.Http.Json;
using CSV_File_Uploader.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace CSV_File_Uploader.Controllers
{
    public class LoginController : Controller
	{
		private readonly ILogger<LoginController> _logger;

		public LoginController(ILogger<LoginController> logger)
		{
			_logger = logger;
		}
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(IFormCollection formData)
		{
			try
			{
				User user = new User();
				string username = formData["username"];
				var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(formData["password"]);
				string password = Convert.ToBase64String(plainTextBytes);
				user.Username = username;
				user.Password = password;


				string apiUrl = "https://localhost:44316/api/User";
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(apiUrl);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

					var data = await client.PostAsJsonAsync(apiUrl, user);
					if (data.StatusCode == System.Net.HttpStatusCode.OK)
					{
						return RedirectToAction("Index", "Home");
					}
					else
					{
						return View("Index", user);
					}

				}

			}
			catch (Exception ex)
			{
                _logger.LogError(ex, "Login Index POST");
                return View("Login");
			}


		}
	}
}
