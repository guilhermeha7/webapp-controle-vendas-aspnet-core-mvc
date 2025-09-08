using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        //readonly indica que o campo só pode receber valor uma única vez
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            List<SelectListItem> statusList = _salesRecordService.GetSaleStatusList();
            List<Seller> sellersList = await _sellerService.GetSellersListAsync();
            SalesRecordFormViewModel viewModel = new SalesRecordFormViewModel { StatusList = statusList, SellersList = sellersList };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SalesRecord salesRecord)
        {
            await _salesRecordService.InsertAsync(salesRecord);
            return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> Edit(int id, DateTime minDate, DateTime maxDate)
        {
            ViewData["minDate"] = minDate;
            ViewData["maxDate"] = maxDate;
            
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { Message = "Id not provided" });
            }

            List<SelectListItem> statusList = _salesRecordService.GetSaleStatusList();
            List<Seller> sellersList = await _sellerService.GetSellersListAsync();
            SalesRecord salesRecord = await _salesRecordService.GetByIdAsync(id);

            if (salesRecord == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            SalesRecordFormViewModel viewModel = new SalesRecordFormViewModel { StatusList = statusList, SellersList = sellersList, SalesRecord = salesRecord };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SalesRecord salesRecord, DateTime minDate, DateTime maxDate)
        {
            await _salesRecordService.UpdateAsync(salesRecord);
            //return RedirectToAction(nameof(SimpleSearch), new { minDate, maxDate });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            SalesRecord salesRecord = await _salesRecordService.GetByIdAsync(id);

            if (salesRecord == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found"});
            }

            return View(salesRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _salesRecordService.DeleteAsync(id);
            }
            catch (Exception e)
            {
                RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return View(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            SalesRecord salesRecord = await _salesRecordService.GetByIdAsync(id);

            if (salesRecord == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }


            return View(salesRecord);
        }

        public IActionResult Error(string message)
        {
            ErrorViewModel viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
