using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using Application.Core.Services;

namespace CSV_File_Uploader
{
    public class IdentityAuthMW
	{
		private readonly RequestDelegate next;
		public IdentityAuthMW(RequestDelegate next)
		{
			this.next = next;
		}
		public async Task InvokeAsync(HttpContext context, UserServices us)
		{
			
			if (context.Request.Path.StartsWithSegments("/swagger"))
			{
				string authHeader = context.Request.Headers["Authorization"];
				if (authHeader != null && authHeader.StartsWith("Basic "))
				{
					var header = AuthenticationHeaderValue.Parse(authHeader);
					var headerBytes = Convert.FromBase64String(header.Parameter);
					var suppliedCredentials = Encoding.UTF8.GetString(headerBytes).Split(':');
					var username = suppliedCredentials[0];
					var password = suppliedCredentials[1];
					var userCheck = us.CheckUser(username, password);
					if (userCheck != 0)
					{
						await next.Invoke(context).ConfigureAwait(false);
						return;
					}

				}
				context.Response.Headers["WWW-Authenticate"] = "Basic";
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			}
			else
			{
				await next.Invoke(context).ConfigureAwait(false);
			}
		}

	}
}
