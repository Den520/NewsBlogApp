using NewsBlogApp.Models;
using System.Drawing;
using System.Web;
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
        public ActionResult EditNews(NewsModel model, HttpPostedFileBase fileCover)
        {
            if (ModelState.IsValid & fileCover != null)
            {
                using (NewsContext db = new NewsContext())
                {
                    NewsModel oldModel = db.News.Find(model.Id);
                    oldModel.Article = model.Article.Trim();
                    oldModel.Content = model.Content.Trim();
                    db.SaveChanges();
                }
                Bitmap bitmap = new Bitmap(Image.FromStream(fileCover.InputStream));
                bitmap = new Bitmap(bitmap, bitmap.Width * 180 / bitmap.Height, 180);
                bitmap.Save(Server.MapPath($"~/Resources/NewsCovers/{model.Id}.png"));

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
            System.IO.File.Delete(Server.MapPath($"~/Resources/NewsCovers/{Id}.png"));
            return RedirectToAction("NewsFeed");
        }
    }
}