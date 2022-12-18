using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Worker
    {
        public Worker()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z-\s]+", ErrorMessage = "Not a name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z0-9-\s]+", ErrorMessage = "Not a section")]
        public string? Section { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z0-9-\s]+", ErrorMessage = "Not a position")]
        public string? Position { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
