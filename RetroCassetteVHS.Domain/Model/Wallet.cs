using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Domain.Model
{
    public class Wallet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }


        public virtual IdentityUser User { get; set; }
    }

}
