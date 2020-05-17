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
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Required]
        public string userrole { get; set; }
    }
}
