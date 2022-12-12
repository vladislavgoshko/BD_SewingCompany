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
        [StringLength(30, MinimumLength = 5, ErrorMessage ="Length 5 and longer")]
        [RegularExpression(@"[a-zA-Z]+", ErrorMessage ="Only letters")]
        [Required(ErrorMessage ="Required")]
        public string? Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
