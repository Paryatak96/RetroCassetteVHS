using Microsoft.EntityFrameworkCore;
using RetroCassetteVHS.Domain.Interface;
using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Infrastructure.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly Context _context;

        public RentalRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetActiveRentalsByUserIdAsync(string userId)
        {
            return await _context.Rentals
                .Where(r => r.UserId == userId && r.ActualReturnDate == null)
                .ToListAsync();
        }

        public async Task AddRentalAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
            await _context.SaveChangesAsync();
        }
    }
}
