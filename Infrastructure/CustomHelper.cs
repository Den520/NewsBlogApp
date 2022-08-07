using NewsBlogApp.Models;
using System;
using System.Web.Mvc;

namespace NewsBlogApp.Infrastructure
{
    public static class CustomHelper
    {
        public static MvcHtmlString CreateNewsBlock(this HtmlHelper html, NewsModel news)
        {
            TagBuilder outerDiv = new TagBuilder("div");
            outerDiv.AddCssClass("news-block");


            TagBuilder innerDiv = new TagBuilder("div");
            innerDiv.AddCssClass("news-article");

            TagBuilder h1 = new TagBuilder("h1");
            h1.InnerHtml += news.Article;

            TagBuilder img = new TagBuilder("img");
            img.MergeAttribute("src", $"/Resources/NewsCovers/{news.Id}.png?{new Random().Next()}");

            innerDiv.InnerHtml += h1.ToString() + img;


            TagBuilder p_Content = new TagBuilder("p");
            p_Content.AddCssClass("multiline-content");
            p_Content.InnerHtml += news.Content;


            TagBuilder footer = new TagBuilder("footer");

            TagBuilder hr = new TagBuilder("hr");

            TagBuilder p_Author = new TagBuilder("p");
            p_Author.InnerHtml += "Автор: " + news.Author;

            TagBuilder p_DateOfPublication = new TagBuilder("p");
            p_DateOfPublication.InnerHtml += "Дата публикации: " + news.DateOfPublication;

            footer.InnerHtml += hr.ToString() + p_Author.ToString() + p_DateOfPublication.ToString();


            outerDiv.InnerHtml += innerDiv.ToString() + p_Content + footer;
            return new MvcHtmlString(outerDiv.ToString());
        }
    }
}