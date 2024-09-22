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
    public class RentalService : IRentalService
    {
        private readonly ICassetteRepository _cassetteRepo;
        private readonly IRentalRepository _rentalRepo;

        public RentalService(ICassetteRepository cassetteRepo, IRentalRepository rentalRepo)
        {
            _cassetteRepo = cassetteRepo;
            _rentalRepo = rentalRepo;
        }

        public async Task <List<Rental>> GetUserActiveRentalsAsync(string userId)
        {
            return await _rentalRepo.GetActiveRentalsByUserIdAsync(userId);
        }

        public async Task <Cassette> GetCassetteByIdAsync(int id)
        {
            return await _cassetteRepo.GetCassetteDetails(id);
        }

        public async Task RentCassetteAsync(int cassetteId, string userId)
        {
            var cassette = await _cassetteRepo.GetCassetteDetails(cassetteId);
            if (cassette == null || !cassette.Availability)
            {
                throw new Exception("Cassette not available.");
            }

            var rental = new Rental
            {
                CassetteId = cassetteId,
                UserId = userId,
                RentalDate = DateTime.Now,
                ExpectedReturnDate = DateTime.Now.AddDays(14)
            };

            cassette.Availability = false;

            await _rentalRepo.AddRentalAsync(rental);
            _cassetteRepo.UpdateCassette(cassette);
        }
    }
}
