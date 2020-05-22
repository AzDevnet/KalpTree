using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KalpTree.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace KalpTree.Controllers
{
    public class HomeController : Controller
    {
        public readonly ISession session;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult Index()
        {
            if (!string.IsNullOrWhiteSpace(Convert.ToString(Request.Cookies["SessionEmail"])))
            {
                session.SetString("SessionName", Request.Cookies["SessionName"].ToString());
                session.SetString("SessionEmail", Request.Cookies["SessionEmail"].ToString());
            }

            return View();
        }

        public IActionResult Privacy()
        {
            
                return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string Search)
        {
            //string url = string.Format("/search/search?page={0}&search={1}",1,Search);
            string url = string.Format("/search/Search?search={0}", Search);
            return Redirect(url);
            //return RedirectToAction("search", new RouteValueDictionary( new { controller = "Search", action = "Search", page = 1 }));
        }
    }
}
