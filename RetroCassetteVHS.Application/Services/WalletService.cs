using RetroCassetteVHS.Application.Interfaces;
using RetroCassetteVHS.Domain.Interface;
using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepo;

        public WalletService(IWalletRepository walletRepo)
        {
            _walletRepo = walletRepo;
        }

        // Pobranie portfela użytkownika na podstawie identyfikatora użytkownika
        public async Task<Wallet> GetUserWalletAsync(string userId)
        {
            return await _walletRepo.GetByUserIdAsync(userId);
        }

        // Aktualizacja salda w portfelu użytkownika
        public async Task UpdateWalletBalanceAsync(Wallet wallet)
        {
            await _walletRepo.UpdateAsync(wallet);
        }
    }
}
