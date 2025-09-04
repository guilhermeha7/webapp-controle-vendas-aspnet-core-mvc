using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime minDate, DateTime maxDate)
        {
            List<SalesRecord> salesRecords = await _salesRecordService.GetSalesListByDateAsync(minDate, maxDate);
            //ViewData é um dicionário usado para passar dados do Controller para a View
            ViewData["minDate"] = minDate.ToString("yyyy-MM-dd"); //Não tem como mostrar um DateTime, pois o atributo value do campo input type="date" espera receber string
            ViewData["maxDate"] = maxDate.ToString("yyyy-MM-dd");
            return View(salesRecords);
        }

        public async Task<IActionResult> GroupingSearch(DateTime minDate, DateTime maxDate)
        {
            var salesRecords = await _salesRecordService.GetSalesListByDepartmentAsync(minDate, maxDate);
            ViewData["minDate"] = minDate.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.ToString("yyyy-MM-dd");
            return View(salesRecords);
        }
    }
}
