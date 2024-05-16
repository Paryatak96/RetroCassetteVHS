using AutoMapper;
using RetroCassetteVHS.Application.Mapping;
using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.ViewModels.Wallet
{
    public class WalletDetailsVm : IMapFrom<RetroCassetteVHS.Domain.Model.Wallet>
    {
        public decimal Balance { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RetroCassetteVHS.Domain.Model.Wallet, WalletDetailsVm>();
        }
    }
}
