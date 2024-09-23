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

        public async Task<Wallet> GetUserWalletAsync(string userId)
        {
            return await _walletRepo.GetByUserIdAsync(userId);
        }

        public async Task UpdateWalletBalanceAsync(string userId, decimal balance)
        {
            var wallet = await _walletRepo.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            if (balance < 0)
            {
                throw new Exception("Balance cannot be negative.");
            }

            wallet.Balance = balance;
            await _walletRepo.UpdateAsync(wallet);
        }
    }
}
