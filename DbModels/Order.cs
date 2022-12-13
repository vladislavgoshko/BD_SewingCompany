using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? OrderDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ExecutionStartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ImplementationDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DeliveryOrderDate { get; set; }
        public int? WorkerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Worker? Worker { get; set; }
    }
}
