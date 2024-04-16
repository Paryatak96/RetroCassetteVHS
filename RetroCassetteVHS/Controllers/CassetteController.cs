using RetroCassetteVHS.Application.Interfaces;
using RetroCassetteVHS.Application.ViewModels.Cassette;
using RetroCassetteVHS.Infrastructure.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;



namespace RetroCassetteVHS.Controllers
{
    public class CassetteController : Controller
    {
        private readonly ICassetteService _casService;

        public CassetteController(ICassetteService casService)
        {
            _casService = casService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = _casService.GetAllCassetteForList(5, 1, "");
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(int pageSize, int? pageNo, string searchString)
        {
            if (!pageNo.HasValue)
            {
                pageNo = 1;
            }
            if (searchString is null)
            {
                searchString = String.Empty;
            }
            var model = _casService.GetAllCassetteForList(pageSize, pageNo.Value, searchString);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddCassette()
        {
            return View(new NewCassetteVm());
        }

        [HttpPost]
        public IActionResult AddCassette(NewCassetteVm model)
        {
            var id = _casService.AddCassette(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditCassette(int id)
        {
            var cassette = _casService.GetCassetteForEdit(id);
            return View(cassette);
        }

        [HttpPost]
        public IActionResult EditCassette(NewCassetteVm model)
        {
            _casService.UpdateCassette(model);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteCassette(int id)
        {
            _casService.DeleteCassette(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CassetteDetails(int id)
        {
            var model = _casService.GetCassetteDetails(id);
            return View(model);
        }
    }
}