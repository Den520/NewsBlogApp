using NewsBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsBlogApp.Controllers
{
    public class NewsContext : DbContext
    {
        public DbSet<NewsModel> News { get; set; }

        public NewsContext() : base("NewsDBConnectionString")
        { }
    }

    [HandleError(ExceptionType = typeof(Exception), View = "~/Views/Shared/Error.cshtml")]
    public abstract partial class BaseController : Controller
    {
        public static List<NewsModel> newsList;  //Создаём список новостей

        public ActionResult NewsFeed()
        {
            //Проверка на авторизованного пользователя
            if (Membership.GetUser() != null & HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString() == "Home")
            {
                return RedirectToAction("NewsFeed", Roles.GetRolesForUser(Membership.GetUser().ToString())[0]);
            }

            //Заполняем список новостей
            using (NewsContext db = new NewsContext())
            {
                newsList = db.News.ToList();
                newsList.Reverse();
            }

            return View();
        }

        [Authorize(Roles = "Admin, Newsman")]
        public ActionResult AddNews()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Newsman")]
        [HttpPost]
        public ActionResult AddNews(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                using (NewsContext db = new NewsContext())
                {
                    model.Article = model.Article.Trim();
                    model.Content = model.Content.Trim();
                    model.Author = Membership.GetUser().ToString();
                    model.DateOfPublication = DateTime.Now;
                    db.News.Add(model);
                    db.SaveChanges();
                }
                return RedirectToAction("NewsFeed");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Newsman")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("NewsFeed", "Home");
        }
    }
}