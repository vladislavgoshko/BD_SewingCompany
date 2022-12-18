using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage ="Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z-\s]+", ErrorMessage ="Not a name")]
        [Required(ErrorMessage ="Required")]
        public string? Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
