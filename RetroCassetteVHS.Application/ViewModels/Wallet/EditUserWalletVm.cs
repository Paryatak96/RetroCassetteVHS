using AutoMapper;
using RetroCassetteVHS.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.ViewModels.Wallet
{
    public class EditUserWalletVm : IMapFrom<RetroCassetteVHS.Domain.Model.Wallet>
    {
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public string UserEmail { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<RetroCassetteVHS.Domain.Model.Wallet, EditUserWalletVm>();
        }
    }
}
