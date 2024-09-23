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
using System.Text.Encodings.Web;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


namespace RetroCassetteVHS.Controllers
{
    public class CassetteController : Controller
    {
        private readonly ICassetteService _casService;
        private readonly IRentalService _rentalService;
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailSender _emailSender;

        public CassetteController(ICassetteService casService, IRentalService rentalService, IWalletService walletService, UserManager<IdentityUser> userManager, IMapper mapper, EmailSender emailSender)
        {
            _casService = casService;
            _rentalService = rentalService;
            _walletService = walletService;
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

        [HttpPost]
        public IActionResult EditCassette(NewCassetteVm model)
        {
            _casService.UpdateCassette(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> CassetteDetails(int id)
        {
            var cassette = await _casService.GetCassetteDetails(id);
            if (cassette == null)
            {
                return NotFound();
            }

            var rentals = await _casService.GetActiveRentalsForCassetteAsync(id);

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

            // Sprawdzenie liczby aktywnych wypożyczeń
            var rentals = await _rentalService.GetUserActiveRentalsAsync(userId);
            if (rentals.Count >= 3)
            {
                return View("SpecifyError", "You cannot rent more than 3 cassettes at a time.");
            }

            // Sprawdzenie dostępności kasety
            var cassette = await _rentalService.GetCassetteByIdAsync(id);
            if (cassette == null || !cassette.Availability)
            {
                return View("SpecifyError", "Cassette not available.");
            }

            var wallet = await _walletService.GetUserWalletAsync(userId);
            if (wallet == null || wallet.Balance < cassette.RentalPrice)
            {
                return View("SpecifyError", "Insufficient funds in wallet.");
            }

            wallet.Balance -= cassette.RentalPrice;

            await _walletService.UpdateWalletBalanceAsync(userId, wallet.Balance);

            await _rentalService.RentCassetteAsync(id, userId);

            await SendInvoice(userId, cassette);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCassette(int id)
        {
            _casService.DeleteCassette(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditCassette(int id)
        {
            var cassette = _casService.GetCassetteForEdit(id);
            return View(cassette);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCassette()
        {
            return View(new NewCassetteVm());
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