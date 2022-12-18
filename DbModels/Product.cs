using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Product
    {
        public Product()
        {
            MaterialLists = new HashSet<MaterialList>();
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z0-9-\s]+", ErrorMessage = "Not a name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, 9999999999999999.99, ErrorMessage = "> 0")]
        public decimal? Price { get; set; }

        public virtual ICollection<MaterialList> MaterialLists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
