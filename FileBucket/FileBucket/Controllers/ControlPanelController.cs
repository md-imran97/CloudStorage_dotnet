using FileBucket.HelperFunction;
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
        [AdminAuth]
        public ActionResult Index()
        {
            var root = Convert.ToInt32(Session["root"]);
            ViewBag.root = root;
            var users = UserHelper.getAllUser();
            return View(users);
        }
        [AdminAuth]
        public ActionResult Approval(int root)
        {
            UserHelper.adminApproval(root);
            return RedirectToAction("Index");
        }
        [AdminAuth]
        public ActionResult Delete(int root)
        {
            UserHelper.removeUser(root);
            return RedirectToAction("Index");
        }
    }
}