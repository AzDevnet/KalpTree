using KalpTree.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PagedList;
using Microsoft.Extensions.Options;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KalpTree.Controllers
{
    public class SearchController : Controller
    {
        private GoogleSearchAPI googleSearchAPI;
        public SearchController(IOptions<GoogleSearchAPI> optionsAccessor)
        {
            googleSearchAPI = optionsAccessor.Value;
        }
        //[HttpGet]
        //public ActionResult Search([FromQuery]int? page, [FromQuery] string search)
        //{
        //    ViewBag.Search = search;
        //    if (string.IsNullOrWhiteSpace(search))
        //    {
        //        page = 1;
        //    }
        //        List<SearchViewModel> searches = new List<SearchViewModel>();
        //    SearchViewModel searchViewModel = new SearchViewModel();

        //    searchViewModel.Decsription = "In web application, displaying data in a gridview is a common requirement. Hence, we will walkthrough possible ways of designing grid view in ASP.NET MVC.";
        //    searchViewModel.Name = "Jyoti Kumar";
        //    searchViewModel.PhoneNumber = "9123456789";
        //    searchViewModel.userType = @"/images/F.gif";
        //    searches.Add(searchViewModel);

        //    SearchViewModel searchViewModelE = new SearchViewModel();

        //    searchViewModelE.Decsription = "In web application, displaying data in a gridview is a common requirement. Hence, we will walkthrough possible ways of designing grid view in ASP.NET MVC.";
        //    searchViewModelE.Name = "Subhashish Nandi";
        //    searchViewModelE.PhoneNumber = "9123456789";
        //    searchViewModelE.userType = @"/images/E.gif";
        //    searches.Add(searchViewModelE);

        //    SearchViewModel searchViewModelA = new SearchViewModel();

        //    searchViewModelA.Decsription = "In web application, displaying data in a gridview is a common requirement. Hence, we will walkthrough possible ways of designing grid view in ASP.NET MVC.";
        //    searchViewModelA.Name = "Dibya Ganguly";
        //    searchViewModelA.PhoneNumber = "9123456789";
        //    searchViewModelA.userType = @"/images/a.gif";
        //    searches.Add(searchViewModelA);

        //    SearchViewModel searchViewModelB = new SearchViewModel();

        //    searchViewModelB.Decsription = "In web application, displaying data in a gridview is a common requirement. Hence, we will walkthrough possible ways of designing grid view in ASP.NET MVC.";
        //    searchViewModelB.Name = "Ranjit Jha";
        //    searchViewModelB.PhoneNumber = "9123456789";
        //    searchViewModelB.userType = @"/images/B.gif";
        //    searches.Add(searchViewModelB);
        //    //return View(searches);
        //    int pageSize = 3;
        //    int pageNumber = (page ?? 1);
        //    return View(searches.ToPagedList(pageNumber, pageSize));
        //}

        [HttpGet]
        public ActionResult Search([FromQuery] string search)
        {
            ViewBag.Search = search;
            WebClient webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");
            var request = webClient.DownloadString(googleSearchAPI.url + "?key=" + googleSearchAPI.Key + "&cx=" + googleSearchAPI.CX + "&q=" + search+ "&alt=json");
            //HttpWebResponse response=(HttpWebResponse) request.GetResponse();
            //Stream dataReader = response.GetResponseStream();
            //StreamReader reader = new StreamReader(request);
            //string responseReader = reader.ReadToEnd();
            dynamic jsondata = JsonConvert.DeserializeObject(request);

            var results = new List<Result>();
            if (jsondata.items != null)
                foreach (var item in jsondata.items)
                {
                    results.Add(new Result
                    {
                        Title = item.title,
                        Link = item.link,
                        Snippet = item.snippet,
                    });
                }

            return View(results.ToList());
        }
    }
}