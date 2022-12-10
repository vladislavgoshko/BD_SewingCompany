using System;
using System.Collections.Generic;

namespace SewingCompany.DbModels
{
    public partial class Worker
    {
        public Worker()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Section { get; set; }
        public string? Position { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
