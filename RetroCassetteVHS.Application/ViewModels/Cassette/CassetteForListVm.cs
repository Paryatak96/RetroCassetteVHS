using AutoMapper;
using RetroCassetteVHS.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.ViewModels.Cassette
{
    public class CassetteForListVm : IMapFrom<RetroCassetteVHS.Domain.Model.Cassette>
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string Language { get; set; }
        public string CassettePhoto { get; set; }
        public bool Availability { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<RetroCassetteVHS.Domain.Model.Cassette, CassetteForListVm>();
        }
    }
}
