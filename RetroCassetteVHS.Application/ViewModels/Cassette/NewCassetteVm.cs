using AutoMapper;
using FluentValidation;
using RetroCassetteVHS.Application.Mapping;
using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.ViewModels.Cassette
{
    public class NewCassetteVm : IMapFrom<RetroCassetteVHS.Domain.Model.Cassette>
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int MovieLength { get; set; }
        public string DirectorFirstName { get; set; }
        public string DirectorLastName { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string CassettePhoto { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewCassetteVm, RetroCassetteVHS.Domain.Model.Cassette>().ReverseMap();
        }
    }

    public class NewCassetteValidation : AbstractValidator<NewCassetteVm>
    {
        public NewCassetteValidation()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.MovieTitle).MaximumLength(100);
        }
    }
}
