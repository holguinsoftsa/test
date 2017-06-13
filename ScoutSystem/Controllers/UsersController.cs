using System.Web.Mvc;

namespace ScoutSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}