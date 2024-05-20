using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Domain.Model
{
    public class Cassette
    {
        public int Id { get; set; }
        public int MovieLength { get; set; }
        public int RentalPrice { get; set; }
        public bool Availability { get; set; }
        public string MovieTitle { get; set; }
        public string DirectorFirstName { get; set; }
        public string DirectorLastName { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string CassettePhoto { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
