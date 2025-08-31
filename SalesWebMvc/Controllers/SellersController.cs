using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Excepcions;
using System.Diagnostics;
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

        [HttpPost] //Indica que esse método é uma requisição POST.
        [ValidateAntiForgeryToken] //Protege contra ataques CSRF, validando um token que deve vir do formulário enviado pelo navegador.
        public IActionResult Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                /*var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .Select(x => new { x.Key, x.Value.Errors })
                .ToArray();

                foreach (var error in errors)
                {
                    foreach (var e in error.Errors)
                    {
                        Console.WriteLine($"Campo {error.Key} -> Erro: {e.ErrorMessage}");
                    }
                }*/

                var departments = _departmentService.GetDepartmentsList();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel);
            }

            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //nameof melhora a manutenibilidade do software. Se o método Index mudar de nome, então o compilador irá alertar incoerências
        }
        public IActionResult Delete(int? id) //Esse ? significa que o parâmetro é nullable, ou seja, pode ser nulo
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.GetById(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _sellerService.Remove(id);
            }
            catch (Exception e)
            {
                RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = _sellerService.GetById(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            Seller seller = _sellerService.GetById(id.Value);

            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = _departmentService.GetDepartmentsList();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Seller seller)
        {
            if (!ModelState.IsValid) //Se der algum problema de validação
            {
                var departments = _departmentService.GetDepartmentsList();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                return View(viewModel); //Recarregue a página, com os dados já inseridos
            }

            //Criar, atualizar e deletar um registro é uma operação sensível, por isso é bom usar try catch
            try
            {
                _sellerService.Update(seller);
            }
            catch (Exception e)
            {
                RedirectToAction(nameof(Error), new { message = e.Message });
            }

            return RedirectToAction(nameof(Index));
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
