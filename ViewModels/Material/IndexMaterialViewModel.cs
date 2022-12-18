using SewingCompany.DbModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace SewingCompany.ViewModels.Material
{
    public class IndexMaterialViewModel
    {
        public IPagedList<DbModels.Material> Materials { get; set; }
        public bool isEnds { get; set; }
        public string sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchString { get; set; }
        public int? page { get; set; }
    }
}
