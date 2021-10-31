using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileBucket.Models;
using FileBucket.UtilityModels;

namespace FileBucket.HelperFunction
{
    public class UserHelper
    {
        static Database db;
        static UserHelper()
        {
            db = new Database();
        }

        public static bool userExist(string phone)
        {
            var entity = (from e in db.users
                         where phone == e.phone
                         select e).FirstOrDefault();

            if (entity == null) return false;
            return true;
        }

        public static void addUser(user u)
        {
            u.keyId = 5001;
            u.packageId = 1;
            u.usedSpace = 0;
            if (u.type == 1) u.status = 1;
            if (u.type == 0) u.status = 0;
            db.users.Add(u);
            db.SaveChanges();
        }

        public static user auth(UserSignin u)
        {
            var entity = (from e in db.users
                          where u.phone==e.phone & u.password==e.password
                          select e).FirstOrDefault();
            return entity;
        }

        public static int getKeyId(int root)
        {
            var entity = (from e in db.users
                          where root == e.id
                          select e).FirstOrDefault();
            var key = entity.keyId;
            return key;
        }

        public static void updatekeyId(int root)
        {
            var entity = (from e in db.users
                          where root == e.id
                          select e).FirstOrDefault();
            entity.keyId++;
            db.SaveChanges();

        }
        
        public static user getUser(int id)
        {
            var entity = (from e in db.users
                          where id == e.id
                          select e).FirstOrDefault();
            return entity;
        }

        public static void updateUser(user u)
        {
            var entity = getUser(u.id);
            entity.name = u.name;
            entity.password = u.password;
            db.SaveChanges();
        }

        public static void removeUser(int id)
        {
            var entity = getUser(id);
            db.users.Remove(entity);
            db.SaveChanges();
        }

        public static List<user> getAllUser()
        {
            var allUser = (from e in db.users
                           select e).ToList();
            return allUser;
        }

        public static void adminApproval(int root)
        {
            var entity = getUser(root);
            entity.status = 1;
            db.SaveChanges();
        }

        public static void userPackageUpdate(user u)
        {
            var entity = getUser(u.id);
            entity.packageId = u.packageId;
            db.SaveChanges();
        }

        public static int getUserType(string phone)
        {
            var entity = (from e in db.users
                          where phone == e.phone
                          select e).FirstOrDefault();
            return entity.type;
        }
    }
}