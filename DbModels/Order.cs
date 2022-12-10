using System;
using System.Collections.Generic;

namespace SewingCompany.DbModels
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExecutionStartDate { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public DateTime? DeliveryOrderDate { get; set; }
        public int? WorkerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Worker? Worker { get; set; }
    }
}
