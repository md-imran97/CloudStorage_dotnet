using FileBucket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBucket.HelperFunction
{
    public class PackageHelper
    {
        static Database db;
        static PackageHelper()
        {
            db = new Database();
        }

        public static List<package> getAllPackage()
        {
            var allPackage = (from p in db.packages
                              select p).ToList();
            return allPackage;
        }

        public static package getPackage(int id)
        {
            var entity= (from e in db.packages
                         where id == e.id
                         select e).FirstOrDefault();
            return entity;
        }

        public static void addPackage(package p)
        {
            db.packages.Add(p);
            db.SaveChanges();
        }

        public static void packageUpdate(package p)
        {
            var pkg = getPackage(p.id);
            pkg.name = p.name;
            pkg.price = p.price;
            pkg.size = p.size;

            db.SaveChanges();
        }

        public static void deletePackage(package p)
        {
            db.packages.Remove(p);
            db.SaveChanges();
        }
    }
}