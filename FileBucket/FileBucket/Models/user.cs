//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileBucket.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class user
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Phone number required")]
        public string phone { get; set; }
        [Required(ErrorMessage = "Name required")]
        public string name { get; set; }
        [Required(ErrorMessage = "Password required")]
        public string password { get; set; }
        public int type { get; set; }
        public int packageId { get; set; }
        public int status { get; set; }
        public int keyId { get; set; }
        public int usedSpace { get; set; }
    
        public virtual package package { get; set; }
    }
}
