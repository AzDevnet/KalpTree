using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KalpTree.Models;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KalpTree.Controllers
{
    public class SignUpController : Controller
    {
        
        // GET: /<controller>/
        private KalpTreeAPI kalpTreeAPI;
        public readonly ISession session;
        public SignUpController(IHttpContextAccessor httpContextAccessor, IOptions<KalpTreeAPI> options)
        {
            session = httpContextAccessor.HttpContext.Session;
            kalpTreeAPI = options.Value;
        }
        [HttpGet]
        public IActionResult SignUp([FromQuery] string userType)
        {
            ViewBag.UserType = userType;
            return View();
        }

        [HttpPost]
        public IActionResult SignUp([FromForm] SignUpViewModel signUpViewModel,[FromQuery] string userType)
        {
            try
            {
                ViewBag.UserType = signUpViewModel.userrole;
                //ViewBag.UserType = userType;
                //session.SetString("SessionName", signUpViewModel.userfname + " " + signUpViewModel.userlname);
                //session.SetString("SessionEmail", signUpViewModel.userlogonid);
                //session.SetString("SessionUserType", signUpViewModel.userrole);
                //string url1 = string.Format("/MyAccount/MyAccount");
                //return Redirect(url1);

                if (ModelState.IsValid)
                {
                    if (Captcha.ValidateCaptchaCode(signUpViewModel.CaptchaCode, HttpContext))
                    {
                        HttpClient webClient = new HttpClient();

                        string contentString = JsonConvert.SerializeObject(signUpViewModel, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                        var buffer = System.Text.Encoding.UTF8.GetBytes(contentString);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        Task<HttpResponseMessage> request = webClient.PostAsync(kalpTreeAPI.LoginApiUrl, byteContent);
                        request.Wait();
                        if (request.Result.IsSuccessStatusCode)
                        {
                            session.SetString("SessionName", signUpViewModel.userfname+" "+ signUpViewModel.userlname);
                            session.SetString("SessionEmail", signUpViewModel.userlogonid);
                            session.SetString("SessionUserType", signUpViewModel.userrole);
                            string url = string.Format("/MyAccount/MyAccount");
                            return Redirect(url);
                            // ViewBag.SuccessCode = "1";
                            // ModelState.Clear();
                        }
                        else
                            ViewBag.SuccessCode = "0";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Captch");
                        
                    }
                }
                
            }
            catch (Exception)
            {

                throw;
            }
                return View();
        }
        [HttpGet]
        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }
    }
}
