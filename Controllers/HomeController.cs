using NewsBlogApp.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsBlogApp.Controllers
{
    public class HomeController : BaseController
    {
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