using System;
using System.Collections.Generic;

namespace SewingCompany.DbModels
{
    public partial class MaterialList
    {
        public int Id { get; set; }
        public int? MaterialId { get; set; }
        public int? ProductId { get; set; }
        public double? MaterialAmount { get; set; }

        public virtual Material? Material { get; set; }
        public virtual Product? Product { get; set; }
    }
}
