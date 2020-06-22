using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalpTree.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KalpTree.Controllers
{
    public class MyAccountController : Controller
    {
        private KalpTreeAPI kalpTreeAPI;
        public readonly ISession session;
        public MyAccountController(IHttpContextAccessor httpContextAccessor, IOptions<KalpTreeAPI> options)
        {
            session = httpContextAccessor.HttpContext.Session;
            kalpTreeAPI = options.Value;
        }
        // GET: /<controller>/
        public IActionResult MyAccount()
        {
            if (string.IsNullOrEmpty( session.GetString("SessionEmail")))
            {
                return RedirectToAction("login", "account");
            }
            return View();
        }
        //to do
        [HttpPost]
        public string AddFarmProduct([FromForm] AddFarmProductViewModel addFarmProduct) {
            string result;
            try
            {
              
                if (ModelState.IsValid)
                {
                    var fileName = System.IO.Path.GetFileName(addFarmProduct.productImage.FileName);
                    ViewBag.FarmProduct = "1";
                    result = "Successfully added";
                    System.Threading.Thread.Sleep(1000);
                }
                else
                    result = "Something went wrong";
            }
            catch (Exception ex)
            {
                result = ex.Message;
              //  throw;
            }
            return result;
        }
    }
}
