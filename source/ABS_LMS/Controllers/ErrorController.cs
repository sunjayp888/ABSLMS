using System.Web.Mvc;


namespace ABS_LMS.Controllers
{
 
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}