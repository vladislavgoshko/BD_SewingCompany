using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Provider
    {
        public Provider()
        {
            Materials = new HashSet<Material>();
        }

        public int Id { get; set; }

        [DisplayName("Provider name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z0-9-.""\s]+", ErrorMessage = "Not a name")]
        [Required(ErrorMessage = "Required")]
        public string? Name { get; set; }

        [DisplayName("Amount of material")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [Required(ErrorMessage = "Required")]
        [Range(0, double.MaxValue, ErrorMessage = "> 0")]
        public double? MaterialAmount { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, 9999999999999999.99, ErrorMessage = "> 0")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }

        [DisplayName("Delivery date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [Required(ErrorMessage = "Required")]
        public DateTime? DeliveryDate { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}
