using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Domain.Interface
{
    public interface ICassetteRepository
    {
        void DeleteCassette(int cassetteId);

        int AddCassette(Cassette cassette);

        IQueryable<Cassette> GetAllCassettes();

        Task <Cassette> GetCassetteDetails(int cassetteId);

        Task<List<Rental>> GetRentalsForCassetteAsync(int cassetteId);

        void UpdateCassette(Cassette cassette);
    }
}
