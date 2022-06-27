using NewsBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsBlogApp.Controllers
{
    public class HomeController : Controller
    {
        //Создаём список новостей, подключаемся к БД
        public static List<NewsModel> newsList = new List<NewsModel>();
        static string connectionString = WebConfigurationManager.ConnectionStrings["NewsDBConnectionString"].ConnectionString;

        public ActionResult NewsFeed()
        {
            //Проверка на авторизованного пользователя
            if (Membership.GetUser() != null)
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

        public ActionResult Login()
        {
            //страница авторизации (валидация) 
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(LogInModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToAction("NewsFeed", Roles.GetRolesForUser(model.UserName)[0]);
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)  //регистрация пользователя (с ролью репортёра)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, passwordQuestion: null, passwordAnswer: null, isApproved: true, providerUserKey: null, status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    Roles.AddUserToRole(model.UserName, "Newsman");
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("NewsFeed", "Newsman");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при регистрации");
                }
            }

            return View(model);
        }
    }
}