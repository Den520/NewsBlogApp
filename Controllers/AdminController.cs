using NewsBlogApp.Models;
using System;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace NewsBlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public ActionResult EditNews(int Id)  //редактирование и удаление новости (валидация)
        {
            return View(newsList.Find(news => news.Id == Id));
        }

        [HttpPost]
        public ActionResult EditNews(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    SqlCommand editNews = new SqlCommand(String.Format("UPDATE NewsTable SET Заголовок = '{0}', Содержание = '{1}' WHERE ID = {2}", model.Article.Trim(), model.Content.Trim(), model.Id), connect);
                    editNews.ExecuteNonQuery();
                }
                return RedirectToAction("NewsFeed");
            }

            return View(model);
        }

        public ActionResult DeleteNews(int Id)  //удаление новости
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                SqlCommand deleteNews = new SqlCommand(String.Format("DELETE FROM NewsTable WHERE ID = {0}", Id), connect);
                deleteNews.ExecuteNonQuery();
            }
            return RedirectToAction("NewsFeed");
        }
    }
}