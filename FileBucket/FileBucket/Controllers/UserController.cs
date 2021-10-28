using FileBucket.Models;
using FileBucket.HelperFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileBucket.UtilityModels;

namespace FileBucket.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(user u)
        {
            if(ModelState.IsValid)
            {
                if(UserHelper.userExist(u.phone))
                {
                    ViewBag.error = "User phone number already exist";
                }
                else
                {
                    UserHelper.addUser(u);
                    ViewBag.msg = "User registation successfull";
                }
            }
            
            return View(u);
        }

        [HttpGet]
        public ActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signin(UserSignin u)
        {
            if (ModelState.IsValid)
            {
                var user = UserHelper.auth(u);

                if(user != null)
                {
                    if(user.type==1)
                    {
                        //Response.Cookies["root"].Value = user.id;
                        Session["phone"] = user.phone;
                        Session["root"] = user.id;
                        Session["parent"] = user.id;
                        Session["name"] = user.name;
                        return RedirectToAction("Index", "Bucket",new { parent=user.id });
                    }
                    else
                    {
                        if(user.status==1)
                        {
                            Session["phone"] = user.phone;
                            Session["root"] = user.id;
                            Session["name"] = user.name;
                            return RedirectToAction("Index", "ControlPanel");
                        }
                        else
                        {
                            ViewBag.msg = "Please wait for admin approval";
                        }
                    }
                }
                else
                {
                    ViewBag.msg = "Phon number or password is incorrect";
                }
            }
                return View();
        }

        public ActionResult Update()
        {
            return View();
        }

        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult Bucket()
        {
            return View();
        }
    }
}