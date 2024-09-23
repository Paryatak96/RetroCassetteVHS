using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetroCassetteVHS.Application.Interfaces;
using RetroCassetteVHS.Application.ViewModels.Wallet;
using RetroCassetteVHS.Infrastructure;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Controllers
{
    public class WalletController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly UserManager<IdentityUser> _userManager;

        public WalletController(IWalletService walletService, UserManager<IdentityUser> userManager)
        {
            _walletService = walletService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Balance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wallet = await _walletService.GetUserWalletAsync(userId);
            return View(wallet);
        }

        [HttpGet]
        public async Task<IActionResult> FindUserByEmail()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserWallet(string userId)
        {
            var wallet = await _walletService.GetUserWalletAsync(userId);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = "Nie znaleziono użytkownika o podanym adresie e-mail.";
                return View("NotFound");
            }

            var wallet = await _walletService.GetUserWalletAsync(user.Id);
            if (wallet == null)
            {
                ViewBag.ErrorMessage = "Użytkownik nie posiada portfela.";
                return View("NotFound");
            }

            return RedirectToAction("EditUserBalance", new { userId = user.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserBalance(string userId)
        {
            var wallet = await _walletService.GetUserWalletAsync(userId);
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

            try
            {
                await _walletService.UpdateWalletBalanceAsync(userId, Balance);
            }

            catch (Exception ex)
            {
                return View("Error", new { message = ex.Message });
            }

            return RedirectToAction("UserWallet", new { userId = userId });
        }
    }
}