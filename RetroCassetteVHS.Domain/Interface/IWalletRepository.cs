using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Domain.Interface
{
    public interface IWalletRepository
    {
        Task<Wallet> GetByUserIdAsync(string userId);
        Task UpdateAsync(Wallet wallet);
    }
}
