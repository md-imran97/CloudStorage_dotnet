using FileBucket.Models;
using FileBucket.HelperFunction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileBucket.UtilityModels;
using System.Web.Security;


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
            if(Session["root"] != null)
            {
                var root = Convert.ToInt32(Session["root"]);
                var user = UserHelper.getUser(root);
                if(user != null)
                {
                    if(user.type==1)
                    {
                        return RedirectToAction("Index", "Bucket", new { parent = user.id });
                    }
                   else if(user.type == 0 && user.status==1)
                    {
                        return RedirectToAction("Index", "ControlPanel");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
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
                        FormsAuthentication.SetAuthCookie(user.phone, false);
                        return RedirectToAction("Index", "Bucket",new { parent=user.id });
                    }
                    else
                    {
                        if(user.status==1)
                        {
                            Session["phone"] = user.phone;
                            Session["root"] = user.id;
                            Session["name"] = user.name;
                            FormsAuthentication.SetAuthCookie(user.phone, false);
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

        [JointAuth]
        public ActionResult Profile()
        {
            var root = Convert.ToInt32(Session["root"]);
            var myInfo = UserHelper.getUser(root);
            return View(myInfo);
        }
        [JointAuth]
        [HttpGet]
        public ActionResult EditProfile()
        {
            var root = Convert.ToInt32(Session["root"]);
            var myInfo = UserHelper.getUser(root);
            return View(myInfo);
        }
        [JointAuth]
        [HttpPost]
        public ActionResult EditProfile(user u)
        {
            if(ModelState.IsValid)
            {
                UserHelper.updateUser(u);
                return RedirectToAction("Profile", "User");

            }
            return View();
        }
        [JointAuth]
        public ActionResult Delete()
        {
            var root = Convert.ToInt32(Session["root"]);
            UserHelper.removeUser(root);
            Session["root"] = null;
            return RedirectToAction("Index", "Home");
        }
        [JointAuth]
        public ActionResult Signout()
        {
            Session["phone"] = null;
            Session["root"] = null;
            Session["parent"] = null;
            Session["name"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Signin", "User");
            return View();
        }
    }
}