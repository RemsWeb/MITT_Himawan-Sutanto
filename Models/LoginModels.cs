using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
namespace MITT_HIMAWAN_SUTANTO.Models
{
    public class LoginModels
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required.")]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "ID Password is required.")]
        public string password { get; set; }
        
    }
}