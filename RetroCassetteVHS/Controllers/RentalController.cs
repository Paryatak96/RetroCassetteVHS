using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroCassetteVHS.Domain.Model;
using RetroCassetteVHS.Infrastructure;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Controllers
{
    public class RentalController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RentalController(Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> RentCassette(int cassetteId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var rentals = _context.Rentals.Where(r => r.UserId == userId && r.ActualReturnDate == null).ToList();
            if (rentals.Count >= 3)
            {
                return View("Error", "You cannot rent more than 3 cassettes at a time.");
            }

            var cassette = await _context.Cassettes.FindAsync(cassetteId);
            if (cassette == null || !cassette.Availability)
            {
                return View("Error", "Cassette not available.");
            }

            var rental = new Rental
            {
                CassetteId = cassetteId,
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
