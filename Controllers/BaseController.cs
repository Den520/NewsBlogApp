using NewsBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsBlogApp.Controllers
{
    [HandleError(ExceptionType = typeof(Exception), View = "~/Views/Shared/Error.cshtml")]
    public abstract partial class BaseController : Controller
    {
        //Создаём список новостей, подключаемся к БД
        public static List<NewsModel> newsList = new List<NewsModel>();
        public static string connectionString = WebConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        public ActionResult NewsFeed()
        {
            //Проверка на авторизованного пользователя
            if (Membership.GetUser() != null & HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString() == "Home")
            {
                return RedirectToAction("NewsFeed", Roles.GetRolesForUser(Membership.GetUser().ToString())[0]);
            }

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

        [Authorize(Roles = "Admin, Newsman")]
        public ActionResult LogOut()
        {
            //выход из аккаунта
            FormsAuthentication.SignOut();
            return RedirectToAction("NewsFeed", "Home");
        }
    }
}