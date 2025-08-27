using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System.Diagnostics.Contracts;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;

        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.GetSellersList();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //Indica que esse método só deve responder a requisições POST.
        [ValidateAntiForgeryToken] //Protege contra ataques CSRF, validando um token que deve vir do formulário enviado pelo navegador.
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index)); //nameof melhora a manutenibilidade do software. Se o método Index mudar de nome, então aqui será mudado também automaticamente
        }
    }
}
