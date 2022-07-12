using NewsBlogApp.Models;
using System.Web.Mvc;

namespace NewsBlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public ActionResult EditNews(int Id)  //редактирование и удаление новости (валидация)
        {
            using (NewsContext db = new NewsContext()) { return View(db.News.Find(Id)); }
        }

        [HttpPost]
        public ActionResult EditNews(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                using (NewsContext db = new NewsContext())
                {
                    NewsModel oldModel = db.News.Find(model.Id);
                    oldModel.Article = model.Article.Trim();
                    oldModel.Content = model.Content.Trim();
                    db.SaveChanges();
                }
                return RedirectToAction("NewsFeed");
            }
            return View(model);
        }

        public ActionResult DeleteNews(int Id)  //удаление новости
        {
            using (NewsContext db = new NewsContext())
            {
                db.News.Remove(db.News.Find(Id));
                db.SaveChanges();
            }
            return RedirectToAction("NewsFeed");
        }
    }
}