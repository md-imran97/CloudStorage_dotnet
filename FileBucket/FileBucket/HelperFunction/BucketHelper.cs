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

        public static List<cloud> deleteFileList(int root, int child)
        {
            List<cloud> files = (from f in db.clouds
                                 where root == f.root && (child == f.parent || child==f.child)
                                 select f).ToList();
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
    }
}