using FileBucket.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileBucket.Controllers
{
    public class PackageController : Controller
    {
        // GET: Package
        public ActionResult Index()
        {
            Database db = new Database();
            var package = db.packages.ToList();
            return View(package);
        }
    }
}