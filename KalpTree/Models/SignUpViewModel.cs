using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KalpTree.Models
{
    public class SignUpViewModel
    {
        public Double _id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        public string userfname { get; set; }
        [Required (ErrorMessage="Last name is required")]
        public string userlname { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string userlogonid { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string confirmpassword { get; set; }

        [Required]
        public string userrole { get; set; }

        [Required]
        [StringLength(4)]
        public string CaptchaCode { get; set; }
    }
}
