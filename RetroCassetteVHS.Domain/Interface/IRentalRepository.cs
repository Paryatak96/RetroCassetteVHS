using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Domain.Interface
{
    public interface IRentalRepository
    {
        Task<List<Rental>> GetActiveRentalsByUserIdAsync(string userId);
        Task AddRentalAsync(Rental rental);
    }
}
