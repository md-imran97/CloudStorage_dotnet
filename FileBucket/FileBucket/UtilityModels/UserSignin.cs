using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileBucket.UtilityModels
{
    public class UserSignin
    {
        [Required(ErrorMessage = "Phone number required")]
        public string phone { get; set; }

        [Required(ErrorMessage = "Password required")]
        public string password { get; set; }
    }
}