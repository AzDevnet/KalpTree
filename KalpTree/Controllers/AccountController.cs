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
            if (!string.IsNullOrWhiteSpace(Convert.ToString(Request.Cookies["SessionEmail"])))
            {
                string url = string.Format("/MyAccount/MyAccount");
                return Redirect(url);
            }
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginView)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Convert.ToString(Request.Cookies["SessionEmail"])))
                {
                    if (ModelState.IsValid)
                    {
                        session.SetString("SessionName", "Jyoti Kumar");
                        session.SetString("SessionEmail", "Jkumar@Gmail.com");
                        session.SetString("SessionUserType", "Farmer");

                        string url1 = string.Format("/MyAccount/MyAccount");
                        return Redirect(url1);

                        WebClient webClient = new WebClient();
                        webClient.Headers.Add("user-agent", "Only a test!");
                        var request = webClient.DownloadString(kalpTreeAPI.LoginApiUrl +  loginView.Email+"," + loginView.Password);

                        if (request.ToString().Contains("No User Found"))
                        {
                            ModelState.AddModelError(string.Empty, "The user name or password is incorrect");
                            return View(loginView);
                        }

                       UserDetails jsondata = JsonConvert.DeserializeObject<UserDetails>(request);


                        session.SetString("SessionName", jsondata.userfname + " " + jsondata.userlname);
                        session.SetString("SessionEmail", jsondata.userlogonid);
                        session.SetString("SessionUserType", jsondata.userrole);

                        Response.Cookies.Append("SessionEmail", jsondata.userlogonid,
                            new CookieOptions()
                            {
                                Expires = DateTime.Now.AddDays(2),
                                IsEssential = true
                            });

                        //Response.Cookies.Append("SessionEmail", "Jkumar@Gmail.com",
                        //    new CookieOptions()
                        //    {
                        //        Expires = DateTime.Now.AddDays(2),
                        //        IsEssential = true
                        //    });
                        Response.Cookies.Append("SessionName", jsondata.userfname + " " + jsondata.userlname,
                            new CookieOptions()
                            {
                                Expires = DateTime.Now.AddDays(2),
                                IsEssential = true
                            });

                        //Response.Cookies.Append("SessionName", "Jyoti Kumar",
                        //    new CookieOptions()
                        //    {
                        //        Expires = DateTime.Now.AddDays(2),
                        //        IsEssential = true
                        //    });
                        Response.Cookies.Append("SessionUserType", jsondata.userrole,
                            new CookieOptions()
                            {
                                Expires = DateTime.Now.AddDays(2),
                                IsEssential = true
                            });
                        string url = string.Format("/MyAccount/MyAccount");
                        return Redirect(url);
                    }
                }
                else
                {
                    session.SetString("SessionName", Request.Cookies["SessionName"].ToString());
                    session.SetString("SessionEmail", Request.Cookies["SessionEmail"].ToString());
                    session.SetString("SessionUserType", Request.Cookies["SessionUserType"].ToString());
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
