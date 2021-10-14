using Microsoft.AspNetCore.Mvc;

namespace PopApis.Controllers
{
    public class AdminController : CommonController
    {
        // GET: AdminController
        public ActionResult Index()
        {
            return ViewWithSession ();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }
    }
}
