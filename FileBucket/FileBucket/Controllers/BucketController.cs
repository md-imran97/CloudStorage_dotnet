﻿using FileBucket.HelperFunction;
using FileBucket.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileBucket.Controllers
{
    public class BucketController : Controller
    {
        // GET: Bucket
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index(int parent)
        {
            ViewBag.parent = parent;
            var root = Convert.ToInt32(Session["root"]);
            
            var files = BucketHelper.fileList(root, parent);
            if(files.Count<1 && parent !=root && !BucketHelper.fileExist(root, parent))
            {
                return RedirectToAction("Index", "Bucket", new { parent = root });
            }
            return View(files);
        }
        

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file,int parent)
        {
            var root = Convert.ToInt32(Session["root"]);
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                var fileSize = file.ContentLength;

                
                //var parent = Convert.ToInt32(Session["parent"]);
                var keyId = UserHelper.getKeyId(root);
                var uniqName = root + "_" + parent + "_" + keyId + "_" + fileName;
                cloud entity = new cloud();
                entity.root = root;entity.parent = parent;entity.child = keyId;
                entity.name = fileName;entity.type = 1;entity.share = 0;entity.size =fileSize;
                entity.time= DateTime.Now.ToString();
                BucketHelper.fileUpload(entity);
                UserHelper.updatekeyId(root);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/resource/"), uniqName);
                file.SaveAs(path);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index","Bucket", new { parent = parent });
        }

        public FileResult Download( int parent, int child, string name)
        {
            var root = Convert.ToInt32(Session["root"]);
            string uniqName = root + "_" + parent + "_" + child + "_" + name;
            string path = Server.MapPath("~/resource/") + uniqName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            return File(bytes, "application/octet-stream", name);
        }

        public ActionResult CreateFolder(string folderName, int parent)
        {
            //ViewBag.parent = parent;
            //ViewBag.root = root;
            var root = Convert.ToInt32(Session["root"]);
            if (folderName !="")
            {
                var keyId = UserHelper.getKeyId(root);
                cloud entity = new cloud();
                entity.root = root; entity.parent = parent; entity.child = keyId;
                entity.name = folderName; entity.type = 0; entity.share = 0; entity.size = 0;
                entity.time = DateTime.Now.ToString();
                BucketHelper.fileUpload(entity);
                UserHelper.updatekeyId(root);
            }
            return RedirectToAction("Index", "Bucket", new {  parent = parent });
        }

        public ActionResult Delete(int parent, int child)
        {
            var root = Convert.ToInt32(Session["root"]);
            var files = BucketHelper.deleteFileList(root, child);
            foreach (var file in files)
            {
                if (file.type == 1)
                {
                    string uniqName = file.root + "_" + file.parent + "_" + file.child + "_" + file.name;
                    string fullPath = Request.MapPath("~/resource/" + uniqName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    BucketHelper.deleteData(file);
                }
                else
                {
                    BucketHelper.deleteData(file);
                }
            }
            return RedirectToAction("Index", "Bucket", new { parent = parent });
        }
    }
}