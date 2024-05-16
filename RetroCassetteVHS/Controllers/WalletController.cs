using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroCassetteVHS.Application.ViewModels.Wallet;
using RetroCassetteVHS.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Controllers
{
    public class WalletController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WalletController(Context context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Metoda do wyświetlania salda dla zalogowanego użytkownika
        public async Task<IActionResult> Balance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            return View(wallet);
        }

        [HttpGet]
        public IActionResult FindUserByEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Nie znaleziono użytkownika o podanym adresie e-mail.";
                return View("NotFound"); // Upewnij się, że taki widok istnieje.
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == user.Id);
            if (wallet == null)
            {
                ViewBag.ErrorMessage = "Użytkownik nie posiada portfela.";
                return View("NotFound"); // Upewnij się, że taki widok istnieje.
            }

            return RedirectToAction("EditUserBalance", new { userId = user.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserBalance(string userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserWalletVm
            {
                UserId = userId,
                Balance = wallet.Balance,
                UserEmail = user.Email
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserBalance(string userId, decimal Balance)
        {
            if (Balance < 0)
            {
                ModelState.AddModelError("newBalance", "Saldo nie może być ujemne.");
                return View();
            }

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
            {
                return NotFound();
            }

            wallet.Balance = Balance;
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();

            return RedirectToAction("UserWallet", new { userId = userId });
        }

        // Metoda do wyświetlania portfela użytkownika dla admina
        public async Task<IActionResult> UserWallet(string userId)
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }
    }
}