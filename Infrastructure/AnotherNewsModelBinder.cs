using NewsBlogApp.Controllers;
using NewsBlogApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace NewsBlogApp.Infrastructure
{
    public class NewsModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Получаем поставщик значений
            var valueProvider = bindingContext.ValueProvider;

            // Получаем значение фильтра
            string filterValue;
            try
            {
                filterValue = valueProvider.GetValue("filterValue").AttemptedValue.ToString();
            }
            catch
            {
                filterValue = "Все";
            }
            
            controllerContext.Controller.ViewBag.FilterValue = filterValue;

            using (NewsContext db = new NewsContext())
            {
                List<NewsModel> newsList = db.News.ToList();
                if (filterValue == "Все")
                {
                    newsList.Reverse();
                    return newsList;
                }
                for (int i = 0; i < newsList.Count; i++)
                {
                    if (Roles.GetRolesForUser(newsList[i].Author)[0] != filterValue)
                    {
                        newsList.Remove(newsList[i]);
                        i--;
                    }
                }
                newsList.Reverse();
                return newsList;
            }
        }
    }
}