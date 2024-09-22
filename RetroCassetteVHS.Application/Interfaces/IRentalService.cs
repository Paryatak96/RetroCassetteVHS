using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.Interfaces
{
    public interface IRentalService
    {
        Task<List<Rental>> GetUserActiveRentalsAsync(string userId);
        Task<Cassette> GetCassetteByIdAsync(int id);
        Task RentCassetteAsync(int cassetteId, string userId);
    }
}
