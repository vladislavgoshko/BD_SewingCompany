using System;
using System.Collections.Generic;

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
        public string? Name { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<MaterialList> MaterialLists { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
