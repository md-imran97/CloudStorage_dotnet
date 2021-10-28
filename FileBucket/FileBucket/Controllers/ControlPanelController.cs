using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileBucket.Controllers
{
    public class ControlPanelController : Controller
    {
        // GET: ControlPanel
        public ActionResult Index()
        {
            return View();
        }
    }
}