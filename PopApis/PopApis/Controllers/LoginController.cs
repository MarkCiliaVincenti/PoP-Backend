using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopLibrary;

namespace PopApis.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly HttpClient httpClient;

        public LoginController()
        {
            this.httpClient = new HttpClient();
        }

        public object UTF8 { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Login()
        {
            var userName = Request.Form["Input.UserName"];
            var password = Request.Form["Input.Password"];
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                httpClient.BaseAddress = new Uri($"{this.Request.Scheme}://{this.Request.Host}");
                var encodedUser = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userName}:{password}"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedUser}");
                var response = await httpClient.GetAsync("/api/Authentication").ConfigureAwait(false);
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToAction("Index", "Home");
                    //throw new UnauthorizedAccessException();
                }
                response.EnsureSuccessStatusCode();
                HttpContext.Session.SetString("UserName", userName);
                

                HttpContext.Session.SetString("UserRole", "Admin");
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            // clear session context
            HttpContext.Session.SetString("UserName", "");
            HttpContext.Session.SetString("UserRole", "");
            return RedirectToAction("Index", "Home");
        }
    }
}
