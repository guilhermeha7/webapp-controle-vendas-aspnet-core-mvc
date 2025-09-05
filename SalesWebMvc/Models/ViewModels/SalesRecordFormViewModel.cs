using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Models.ViewModels
{
    public class SalesRecordFormViewModel
    {
        public SalesRecord SalesRecord { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<Seller> SellersList { get; set; }
    }
}
