using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileBucket.HelperFunction
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class JointAuth : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var flag = base.AuthorizeCore(httpContext);
            if (flag)
            {

                var userType = UserHelper.getUserType(httpContext.User.Identity.Name);
                if (userType == 0 || userType == 1) return true;
            }
            return false;
        }
    }
}