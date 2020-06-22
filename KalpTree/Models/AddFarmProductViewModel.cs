using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KalpTree.Models
{
    public class AddFarmProductViewModel
    {
        [Required(ErrorMessage ="Product description is required.")]
        public string prodDescription { get; set; }

        [Required(ErrorMessage = "Product weight is required.")]
        public string weight { get; set; }

        [Required(ErrorMessage = "Product price is required.")]
        [DataType(DataType.Currency)]
        public string price { get; set; }

        [Required(ErrorMessage = "Product image is required.")]
        public IFormFile productImage { get; set; }
        

    }
}
