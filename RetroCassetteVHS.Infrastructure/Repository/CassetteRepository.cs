using RetroCassetteVHS.Domain.Interface;
using RetroCassetteVHS.Domain.Model;
using RetroCassetteVHS.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroCassetteVHS.Infrastructure.Repository
{
    public class CassetteRepository : ICassetteRepository
    {
        private readonly Context _context;

        public CassetteRepository(Context context)
        {
            _context = context;
        }

        public IQueryable<Cassette> GetAllCassettes()
        {
            return _context.Cassettes;
        }

        public Cassette GetCassette(int cassetteId)
        {
            return _context.Cassettes.FirstOrDefault(p => p.Id == cassetteId);
        }

        public void DeleteCassette(int id)
        {
            var cassette = _context.Cassettes.Find(id);
            if (cassette != null)
            {
                _context.Cassettes.Remove(cassette);
                _context.SaveChanges();
            }
        }

        public int AddCassette(Cassette cassette)
        {
            _context.Cassettes.Add(cassette);
            _context.SaveChanges();
            return cassette.Id;
        }

        public void UpdateCassette(Cassette cassette)
        {
            _context.Attach(cassette);
            _context.Entry(cassette).Property("MovieTitle").IsModified = true;
            _context.Entry(cassette).Property("Availability").IsModified = true;
            _context.Entry(cassette).Property("Description").IsModified = true;
            _context.Entry(cassette).Property("Language").IsModified = true;
            _context.Entry(cassette).Property("Quantity").IsModified = true;
            _context.Entry(cassette).Property("Genre").IsModified = true;
            _context.Entry(cassette).Property("CassettePhoto").IsModified = true;
            _context.SaveChanges();
        }
    }
}