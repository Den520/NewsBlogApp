using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace NewsBlogApp.Infrastructure
{
    public class AnotherActionFilterAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Проверка на авторизованного пользователя
            if (Membership.GetUser() != null & context.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString() == "Home")
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "action", "NewsFeed" },
                    { "controller", Roles.GetRolesForUser(Membership.GetUser().ToString())[0] }
                });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}