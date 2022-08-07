using NewsBlogApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NewsBlogApp.Controllers
{
    public class AnotherController : BaseController
    {
        const int pageSize = 2;  //кол-во статей за одну "подгрузку"

        [HttpGet]
        public ActionResult NewsFeed(List<NewsModel> anotherNewsList, int? page)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ItemsPartial", GetItemsPage(anotherNewsList, (int)page));
            }
            return View(GetItemsPage(anotherNewsList, 0));
        }

        [HttpPost]
        public ActionResult NewsFeed(List<NewsModel> anotherNewsList)
        {
            return View(GetItemsPage(anotherNewsList, 0));
        }

        private List<NewsModel> GetItemsPage(List<NewsModel> anotherNewsList, int page)
        {
            var itemsToSkip = page * pageSize;

            return anotherNewsList.Skip(itemsToSkip).
                Take(pageSize).ToList();
        }

        [HttpGet]
        public ActionResult GetCities()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetCities(CityModel[] cities)
        {
            if (ModelState.IsValid) { return View(cities); }
            return View();
        }
    }
}