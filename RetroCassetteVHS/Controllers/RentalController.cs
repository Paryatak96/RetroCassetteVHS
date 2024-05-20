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
    }
}
