using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class MaterialList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? MaterialId { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Range(0, double.MaxValue, ErrorMessage = "> 0")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        [DisplayName("Amount of material")]
        public double? MaterialAmount { get; set; }

        public virtual Material? Material { get; set; }
        public virtual Product? Product { get; set; }
    }
}
