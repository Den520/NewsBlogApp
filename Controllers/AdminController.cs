using NewsBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsBlogApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //Создаём список новостей, подключаемся к БД
        public static List<NewsModel> newsList = new List<NewsModel>();
        static string connectionString = WebConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        public ActionResult NewsFeed()
        {
            //Очищаем список новостей и синхронизируемся с БД, затем заполняем список новостей
            newsList.Clear();
            DataSet newsDataset = new DataSet();
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                SqlDataAdapter newsAdapter = new SqlDataAdapter("SELECT * FROM NewsTable", connect);
                newsAdapter.Fill(newsDataset, "NewsTable");
            }
            foreach (DataRow row in newsDataset.Tables["NewsTable"].Rows)
            {
                newsList.Add(new NewsModel()
                {
                    Id = (int)row["ID"],
                    Article = row["Заголовок"].ToString(),
                    Content = row["Содержание"].ToString(),
                    Author = row["Автор"].ToString(),
                    DateOfPublication = row["ДатаПубликации"].ToString()
                });
            }
            newsList.Reverse();
            return View();
        }

        public ActionResult AddNews()  //создание новости (валидация)
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddNews(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    connect.Open();
                    SqlCommand addNews = new SqlCommand(String.Format("INSERT INTO NewsTable (Заголовок, Содержание, Автор, ДатаПубликации) VALUES ('{0}', '{1}', '{2}', GETDATE())", model.Article.Trim(), model.Content.Trim(), Membership.GetUser()), connect);
                    addNews.ExecuteNonQuery();
                }

                return RedirectToAction("NewsFeed");
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            //выход из аккаунта
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

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

        /*public ActionResult NewsRegister()
        {
            //реестр новостей (краткая новостная лента, без содержимого новостей)
            return View();
        }*/
    }
}