using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "> 0")]
        [Required(ErrorMessage = "Required")]
        public int? Amount { get; set; }

        [DisplayName("Order date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessage = "Required")]
        public DateTime? OrderDate { get; set; }

        [DisplayName("Execution start date")]
        [Required(ErrorMessage = "Required")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ExecutionStartDate { get; set; }

        [DisplayName("Implementation date")]
        [Required(ErrorMessage = "Required")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ImplementationDate { get; set; }

        [DisplayName("Delivery order date")]
        [Required(ErrorMessage = "Required")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? DeliveryOrderDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? WorkerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Worker? Worker { get; set; }
    }
}
