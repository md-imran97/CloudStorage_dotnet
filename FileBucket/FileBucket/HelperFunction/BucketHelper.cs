using FileBucket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace FileBucket.HelperFunction
{
    public class BucketHelper
    {
        static Database db;
        static BucketHelper()
        {
            db = new Database();
        }

        public static List<cloud> fileList(int root, int parent)
        {
            
            List<cloud> files = (from f in db.clouds
                         where root == f.root && parent == f.parent
                         select f).ToList();
            return files;
        }

        public static void fileUpload(cloud c)
        {
            db.clouds.Add(c);
            db.SaveChanges();
            //db.SaveChanges();
        }

        public static List<cloud> getFileList(int root, int child)
        {
            List<cloud> files = new List<cloud>();
            Stack<cloud> fileStack = new Stack<cloud>();
            var entity = (from e in db.clouds
                          where root == e.root && child == e.child
                          select e).FirstOrDefault();
            fileStack.Push(entity);
            while(fileStack.Count !=0)
            {
                var tempFile = fileStack.Pop();
                files.Add(tempFile);
                if(tempFile.type==0)
                {
                    List<cloud> tempFileList = new List<cloud>();
                    tempFileList = fileList(tempFile.root, tempFile.child);
                    foreach(var temp in tempFileList)
                    {
                        fileStack.Push(temp);
                    }
                }
            }

            return files;
            
        }

        public static void deleteData(cloud c)
        {
            db.clouds.Remove(c);
            db.SaveChanges();
        }
        public static bool fileExist(int root, int child)
        {
            var entity = (from e in db.clouds
                          where root == e.root && (child==e.child || child==e.parent)
                          select e).FirstOrDefault();
            if(entity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void shareFile(List<cloud> files, int shareType)
        {
            foreach(var file in files)
            {
                file.share = shareType;
                db.SaveChanges();
            }
        }

        public static List<cloud> listForUpdate(int root, int parent)
        {

            List<cloud> files = (from f in db.clouds
                                 where root == f.root && parent == f.child
                                 select f).ToList();
            return files;
        }

        public static cloud getOneFile(int root, int parent, int child)
        {
            var entity = (from e in db.clouds
                          where root == e.root && parent == e.parent && child == e.child
                          select e).FirstOrDefault();
            return entity;
        }

        public static void storageUpdate(int root, int parent, int child, int dataSize, int updateType)
        {
            List<cloud> files = new List<cloud>();
            Stack<cloud> fileStack = new Stack<cloud>();
            var entity = (from e in db.clouds
                          where root == e.root && parent == e.parent && child == e.child
                          select e).FirstOrDefault();

            
            fileStack.Push(entity);
            while (fileStack.Count != 0)
            {
                var tempFile = fileStack.Pop();
                files.Add(tempFile);
                
                List<cloud> tempFileList = new List<cloud>();
                tempFileList = listForUpdate(tempFile.root, tempFile.parent);
                foreach (var temp in tempFileList)
                {
                    fileStack.Push(temp);
                }
                
            }
            foreach(var data in files)
            {
                if(updateType==1)
                {
                    data.size += dataSize;
                    db.SaveChanges();
                }
                else
                {
                    data.size -= dataSize;
                    db.SaveChanges();
                }
            }
            user userInfo = UserHelper.getUser(root);
            if(updateType == 1)
            {
                userInfo.usedSpace += dataSize;
                db.SaveChanges();
            }
            else
            {
                userInfo.usedSpace -= dataSize;
                if (userInfo.usedSpace < 0) { userInfo.usedSpace = 0; }
                db.SaveChanges();
            }
        }

        public static cloud getFileByChild(int root, int child)
        {
            var entity = (from e in db.clouds
                          where root == e.root && child == e.child
                          select e).FirstOrDefault();
            return entity;
        }

        public static cloud sharedFile(int root, int parent, int child)
        {

            var file = (from f in db.clouds
                        where (root == f.root) && (parent == f.parent) && (child == f.child) && (f.share == 1)
                        select f).FirstOrDefault();

            return file;
        }

        public static List<cloud> sharedFileList(int root, int child)
        {

            var files = (from f in db.clouds
                        where (root == f.root) && (child == f.parent) && (f.share == 1)
                        select f).ToList();

            return files;
        }
    }
}