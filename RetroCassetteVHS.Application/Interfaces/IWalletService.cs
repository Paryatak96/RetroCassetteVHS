using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> GetUserWalletAsync(string userId);
        Task UpdateWalletBalanceAsync(string userId, decimal balance);
    }
}
