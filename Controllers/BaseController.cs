using NewsBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
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
        public ActionResult AddNews(NewsModel model, HttpPostedFileBase fileCover)
        {
            if (ModelState.IsValid & fileCover != null)
            {
                //Добавление новости в БД
                using (NewsContext db = new NewsContext())
                {
                    model.Article = model.Article.Trim();
                    model.Content = model.Content.Trim();
                    model.Author = Membership.GetUser().ToString();
                    model.DateOfPublication = DateTime.Now;
                    model.Id = db.News.Add(model).Id;
                    db.SaveChanges();
                }

                //Сохранение обложки в хранилище
                Bitmap bitmap = new Bitmap(Image.FromStream(fileCover.InputStream));
                bitmap = new Bitmap(bitmap, bitmap.Width * 180 / bitmap.Height, 180);
                bitmap.Save(Server.MapPath($"~/Resources/NewsCovers/{model.Id}.png"));

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