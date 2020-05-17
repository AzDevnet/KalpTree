using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalpTree.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
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
                session.SetString("SessionName", "Ranjit Jha");
                session.SetString("SessionEmail", loginView.Email);
                session.ToString();
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
