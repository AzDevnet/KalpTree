using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KalpTree.Models;
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
        //public IActionResult Index()
        //{
        //    return View();
        //}
        //const string SessionName = "_Name";
        //const string SessionEmail = "_Email";
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
            if (ModelState.IsValid)
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Only a test!");
                var request = webClient.DownloadString(kalpTreeAPI.LoginApiUrl + "?UserId=" + loginView.Email + "&Password=" + loginView.Password);
                //HttpWebResponse response=(HttpWebResponse) request.GetResponse();
                //Stream dataReader = response.GetResponseStream();
                //StreamReader reader = new StreamReader(request);
                //string responseReader = reader.ReadToEnd();
                List<UserDetails> jsondata = JsonConvert.DeserializeObject<List<UserDetails>>(request);

                //var results = new List<UserDetails>();
                //if (jsondata.items != null)
                //    foreach (var item in jsondata)
                //    {
                //        results.Add(new UserDetails
                //        {
                //            userfname = item.userfname,
                //            userlname = item.userlname,
                //            userlogonid = item.userlogonid,
                //        });
                //    }

                session.SetString("SessionName", jsondata[0].userfname +" "+ jsondata[0].userlname);
                session.SetString("SessionEmail", jsondata[0].userlogonid);
                string url = string.Format("/home/index");
                return Redirect(url);
            }

            return View(loginView);
        }
        public ActionResult LogOff()
        {
            session.Clear();
            return RedirectToAction("login", "account");
            
        }
    }
}
