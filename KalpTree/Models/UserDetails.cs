using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalpTree.Models
{
    public class UserDetails
    {
        public Double _id { get; set; }
        [Required]
        public string userfname { get; set; }
        [Required]
        public string userlname { get; set; }
        [Required]
        [EmailAddress]
        public string userlogonid { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }


        [Required]
        public string userrole { get; set; }
    }
}
