using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System.Diagnostics.Contracts;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;

        }

        public IActionResult Index()
        {
            var list = _sellerService.GetSellersList();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departmentService.GetDepartmentsList();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //Indica que esse método só deve responder a requisições POST.
        [ValidateAntiForgeryToken] //Protege contra ataques CSRF, validando um token que deve vir do formulário enviado pelo navegador.
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //nameof melhora a manutenibilidade do software. Se o método Index mudar de nome, então aqui será mudado também automaticamente
        }
        public IActionResult Delete(int? id) //Esse ? significa que o parâmetro é nullable, ou seja, pode ser nulo
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.GetById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obj = _sellerService.GetById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
    }
}
