using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Domain.Model
{
    public class Rental
    {
        public int Id { get; set; }
        public int CassetteId { get; set; }
        public string UserId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public virtual Cassette Cassette { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
