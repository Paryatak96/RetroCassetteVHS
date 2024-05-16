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
using Microsoft.AspNetCore.Authorization;
using RetroCassetteVHS.Domain.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using RetroCassetteVHS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using AutoMapper;



namespace RetroCassetteVHS.Controllers
{
    public class CassetteController : Controller
    {
        private readonly ICassetteService _casService;
        private readonly Context _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;

        public CassetteController(ICassetteService casService, Context context, UserManager<IdentityUser> userManager, IMapper mapper)
        {
            _casService = casService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
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
        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> CassetteDetails(int id)
        {
            var cassette = await _context.Cassettes.FindAsync(id);
            if (cassette == null)
            {
                return NotFound();
            }

            // Pobierz wszystkie wypożyczenia tej kasety, które jeszcze się nie zakończyły
            var rentals = await _context.Rentals
                .Where(r => r.CassetteId == id && r.ActualReturnDate == null)
                .ToListAsync();

            // Oblicz najbliższą datę dostępności
            var nextAvailableDate = rentals
                .OrderBy(r => r.ExpectedReturnDate)
                .LastOrDefault()?.ExpectedReturnDate;

            var viewModel = _mapper.Map<CassetteDetailsVm>(cassette);
            viewModel.NextAvailableDate = nextAvailableDate;

            return View(viewModel);
        }

        public async Task<IActionResult> RentCassette(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rentals = _context.Rentals.Where(r => r.UserId == userId && r.ActualReturnDate == null).ToList();
            if (rentals.Count >= 3)
            {
                return View("Error", "You cannot rent more than 3 cassettes at a time.");
            }

            var cassette = await _context.Cassettes.FindAsync(id);
            if (cassette == null || !cassette.Availability)
            {
                return View("Error", "Cassette not available.");
            }

            var rental = new Rental
            {
                CassetteId = id,
                UserId = userId,
                RentalDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14)
            };

            cassette.Availability = false;
            _context.Rentals.Add(rental);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Redirect to a relevant view
        }
    }
}