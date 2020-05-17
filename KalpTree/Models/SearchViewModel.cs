using System;
using PagedList;
namespace KalpTree.Models
{
    public class SearchViewModel
    {
        public Int64 Id
        {
            get
            {
                return 1;
            }
           // set;
        }
        public string userType { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Decsription { get; set; }
    }
}
