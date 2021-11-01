using FileBucket.HelperFunction;
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
        [AdminAuth]
        public ActionResult Index()
        {
            var packages = PackageHelper.getAllPackage();
            
            return View(packages);
        }
        [UserAuth]
        public ActionResult Store()
        {
            var root = Convert.ToInt32(Session["root"]);
            var packages = PackageHelper.getAllPackage();
            var user = UserHelper.getUser(root);
            ViewBag.packageId = user.packageId;
            ViewBag.root = user.id;
            return View(packages);
        }

        [AdminAuth]
        public ActionResult CreatePackage()
        {
            return View();
        }
        [AdminAuth]
        [HttpPost]
        public ActionResult CreatePackage(package p)
        {
            if(ModelState.IsValid)
            {
                var pkg = PackageHelper.getPackage(p.id);
                if(pkg==null)
                {
                    PackageHelper.addPackage(p);
                    ViewBag.msg = "Package added successfully";
                    return View();
                }
                else
                {
                    ViewBag.msg = "Package id already exist";
                    return View();
                }
            }
            return View();
        }
        [AdminAuth]
        [HttpGet]
        public ActionResult EditPackage(int id)
        {
            var pkg = PackageHelper.getPackage(id);
            return View(pkg);

        }
        [AdminAuth]
        [HttpPost]
        public ActionResult EditPackage(package p)
        {
            if(ModelState.IsValid)
            {
                PackageHelper.packageUpdate(p);
                ViewBag.msg = "Updated successfully";
                return RedirectToAction("Index");
            }
            return RedirectToAction("EditPackage", new { id = p.id });

        }
        [UserAuth]
        public ActionResult Subscribe(int id)
        {
            var root = Convert.ToInt32(Session["root"]);
            var pkg = PackageHelper.getPackage(id);
            var user = UserHelper.getUser(root);
            if(user.usedSpace>pkg.size)
            {
                return RedirectToAction("Error", "Package", new { i = 1 });
            }
            user.packageId = pkg.id;
            UserHelper.userPackageUpdate(user);
            return RedirectToAction("Store", "Package");
        }

        public ActionResult Error(int i)
        {
            if(i==1)
            {
                ViewBag.msg = "You can not subscribe this package because your used space is bigger than this package space";
            }
            else
            {
                ViewBag.msg = "You can not delete this package because some user hold this package";
            }
            return View();
        }
        [AdminAuth]
        public ActionResult Delete(int id)
        {
            var pkg = PackageHelper.getPackage(id);
            if(pkg.users.Count>0)
            {
                return RedirectToAction("Error", "Package",new {i=0 });
            }
            else
            {
                PackageHelper.deletePackage(pkg);
                return RedirectToAction("Index", "Package");
            }
        }
    }
}