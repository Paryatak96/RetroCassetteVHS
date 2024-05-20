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
using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SendGrid;
using RetroCassetteVHS.Services;
using RetroCassetteVHS.Application.Services;




namespace RetroCassetteVHS.Controllers
{
    public class CassetteController : Controller
    {
        private readonly ICassetteService _casService;
        private readonly Context _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly EmailSender _emailSender;

        public CassetteController(ICassetteService casService, Context context, UserManager<IdentityUser> userManager, IMapper mapper, EmailSender emailSender)
        {
            _casService = casService;
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _emailSender = emailSender;
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
        public async Task<IActionResult> AddCassette(NewCassetteVm cassette)
        {
            if (cassette.CassettePhotoFile == null)
            {
                ModelState.AddModelError("CassettePhotoFile", "The Cassette Photo is required.");
                return View(cassette);
            }

            await _casService.AddCassette(cassette, cassette.CassettePhotoFile, cassette.CassettePhoto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

            var rentals = await _context.Rentals
                .Where(r => r.CassetteId == id && r.ActualReturnDate == null)
                .ToListAsync();

            var nextAvailableDate = rentals
                .OrderBy(r => r.ExpectedReturnDate)
                .LastOrDefault()?.ExpectedReturnDate;

            var viewModel = _mapper.Map<CassetteDetailsVm>(cassette);
            viewModel.NextAvailableDate = nextAvailableDate;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> RentCassette(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return View("SpecifyError", "User doesn't exist");
            }
            var rentals = _context.Rentals.Where(r => r.UserId == userId && r.ActualReturnDate == null).ToList();
            if (rentals.Count >= 3)
            {
                return View("SpecifyError", "You cannot rent more than 3 cassettes at a time.");
            }

            var cassette = await _context.Cassettes.FindAsync(id);
            if (cassette == null || !cassette.Availability)
            {
                return View("SpecifyError", "Cassette not available.");
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null || wallet.Balance < cassette.RentalPrice)
            {
                return View("SpecifyError", "Insufficient funds in wallet.");
            }

            wallet.Balance -= cassette.RentalPrice;
            _context.Wallets.Update(wallet);

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

            await SendInvoice(userId, cassette);

            return RedirectToAction("Index");
        }

        private async Task SendInvoice(string userId, Cassette cassette)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var email = user.Email;
            var subject = "Your Rental Invoice";
            var body = $@"
            <h1>Invoice for Your Rental</h1>
            <p>Dear {user.UserName},</p>
            <p>Thank you for renting from RetroCassetteVHS. Here are the details of your rental:</p>
            <ul>
                <li>Title: {cassette.MovieTitle}</li>
                <li>Rental Date: {DateTime.Now.ToShortDateString()}</li>
                <li>Return Date: {DateTime.Now.AddDays(14).ToShortDateString()}</li>
                <li>Price: {cassette.RentalPrice:C}</li>
            </ul>
            <p>Thank you for your business!</p>
            <p>RetroCassetteVHS</p>";

            await _emailSender.SendEmail(subject, email, user.UserName, body);
        }
    }
}