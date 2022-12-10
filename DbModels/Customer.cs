using System;
using System.Collections.Generic;

namespace SewingCompany.DbModels
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
