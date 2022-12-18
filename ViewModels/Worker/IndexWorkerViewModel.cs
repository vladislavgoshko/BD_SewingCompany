using SewingCompany.DbModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace SewingCompany.ViewModels.Worker
{
    public class IndexWorkerViewModel
    {
        public IPagedList<DbModels.Worker> Workers { get; set; }
        public SelectList sections { get; set; }
        public string SelectedSection { get; set; }
        public bool isSection { get; set; }
        public bool isNotAtTime { get; set; }
        public string sortOrder { get; set; }
        public string currentFilter { get; set; }
        public string searchString { get; set; }
        public int? page { get; set; }
    }
}
