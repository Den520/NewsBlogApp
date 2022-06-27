using System.Web.Mvc;

namespace NewsBlogApp.Controllers
{
    [Authorize(Roles = "Newsman")]
    public class NewsmanController : BaseController
    {

    }
}