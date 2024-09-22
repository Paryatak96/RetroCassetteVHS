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
    public class WalletRepository : IWalletRepository
    {
        private readonly Context _context;

        public WalletRepository(Context context)
        {
            _context = context;
        }

        // Pobranie portfela użytkownika na podstawie identyfikatora użytkownika
        public async Task<Wallet> GetByUserIdAsync(string userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        // Aktualizacja portfela użytkownika
        public async Task UpdateAsync(Wallet wallet)
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
        }
    }
}
