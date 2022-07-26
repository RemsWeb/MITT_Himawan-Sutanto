using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MITT_HIMAWAN_SUTANTO.Models
{
    public class User
    {
        [Required(ErrorMessage = "* Harap Isi UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* Harap Isi Password Lama Anda")]
        public string Password { get; set; }

        [Required(ErrorMessage = "* Harap Isi Nama Anda")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Harap Isi Alamat Anda")]
        public string Address { get; set; }

        [Required(ErrorMessage = "* Harap Tanggal Lahir Anda")]
        public string DOB { get; set; }

        [Required(ErrorMessage = "* Harap Isi Email Anda")]
        public string Email { get; set; }
    }
}