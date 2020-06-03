using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KalpTree.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KalpTree.Controllers
{
    public class AccountController : Controller
    {
        // GET: /<controller>/
        public readonly ISession session;
        private KalpTreeAPI kalpTreeAPI;
        public AccountController(IHttpContextAccessor httpContextAccessor,IOptions<KalpTreeAPI> options)
        {
            session = httpContextAccessor.HttpContext.Session;
            kalpTreeAPI = options.Value;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(Request.Cookies["SessionEmail"])))
            {
                if (ModelState.IsValid)
                {
                    WebClient webClient = new WebClient();
                    webClient.Headers.Add("user-agent", "Only a test!");
                    var request = webClient.DownloadString(kalpTreeAPI.LoginApiUrl + "?UserId=" + loginView.Email + "&Password=" + loginView.Password);

                    List<UserDetails> jsondata = JsonConvert.DeserializeObject<List<UserDetails>>(request);


                    session.SetString("SessionName", jsondata[0].userfname + " " + jsondata[0].userlname);
                    session.SetString("SessionEmail", jsondata[0].userlogonid);

                    Response.Cookies.Append("SessionEmail", jsondata[0].userlogonid,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(2),
                            IsEssential = true
                        });

                    Response.Cookies.Append("SessionEmail", "Jkumar@Gmail.com",
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(2),
                            IsEssential = true
                       });
                    Response.Cookies.Append("SessionName", jsondata[0].userfname + " " + jsondata[0].userlname,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(2),
                            IsEssential = true
                        });

                    Response.Cookies.Append("SessionName", "Jyoti Kumar",
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(2),
                            IsEssential = true
                        });
                    string url = string.Format("/home/index");
                    return Redirect(url);
                }
            }
            else
            {
                session.SetString("SessionName",Request.Cookies["SessionName"].ToString());
                session.SetString("SessionEmail", Request.Cookies["SessionEmail"].ToString());
            }
            return View(loginView);
        }
        public ActionResult LogOff()
        {
            session.Clear();
            Response.Cookies.Delete("SessionName");
            Response.Cookies.Delete("SessionEmail");
            return RedirectToAction("login", "account");
            
        }
        public void Configure(IApplicationBuilder applicationBuilder)
        {
            var options = new SessionOptions
            {
                IdleTimeout = TimeSpan.FromSeconds(5),
            };
            applicationBuilder.UseSession(options);
        }
    }
}
