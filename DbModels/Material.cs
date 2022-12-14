using System.ComponentModel.DataAnnotations;

namespace SewingCompany.DbModels
{
    public partial class Material
    {
        public Material()
        {
            MaterialLists = new HashSet<MaterialList>();
        }

        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z-\s]+", ErrorMessage = "Not a name")]
        [Required(ErrorMessage = "Required")]
        public string? Name { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Length 3 and longer")]
        [RegularExpression(@"[А-Яа-яA-Za-z0-9-\s]+", ErrorMessage = "Not a type")]
        [Required(ErrorMessage = "Required")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "Required")]
        public int? ProviderId { get; set; }

        public virtual Provider? Provider { get; set; }
        public virtual ICollection<MaterialList> MaterialLists { get; set; }
    }
}
