using System;
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
    }
}
