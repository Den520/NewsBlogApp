using NewsBlogApp.Infrastructure;
using NewsBlogApp.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NewsBlogApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        void Application_Error(object sender, EventArgs e)
        {
            Response.Write("<b>Возникла ошибка: </b><hr/>");
            Response.Write(Server.GetLastError().Message.ToString() + "<hr/>" + Server.GetLastError().ToString());
            Server.ClearError();
        }

        void Application_BeginRequest()
        {
            try
            {
                if (Context.Request.Url.Segments[1] == "Another/" || Context.Request.Url.Segments[1] == "Another")
                {
                    ModelBinders.Binders.Add(typeof(List<NewsModel>), new NewsModelBinder());
                }
            }
            catch
            {
                return;
            }
        }

        void Application_EndRequest()
        {
            try
            {
                if (Context.Request.Url.Segments[1] == "Another/" || Context.Request.Url.Segments[1] == "Another")
                {
                    ModelBinders.Binders.Remove(typeof(List<NewsModel>));
                }
            }
            catch
            {
                return;
            }
        }
    }
}
