using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using RetroCassetteVHS.Application.Interfaces;
using RetroCassetteVHS.Application.ViewModels.Cassette;
using RetroCassetteVHS.Domain.Interface;
using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroCassetteVHS.Application.Services
{
    public class CassetteService : ICassetteService
    {
        private readonly ICassetteRepository _cassetteRepo;
        private readonly IMapper _mapper;

        public CassetteService(ICassetteRepository cassetteRepo, IMapper mapper)
        {
            _cassetteRepo = cassetteRepo;
            _mapper = mapper;
        }

        public async Task<int> AddCassette(NewCassetteVm cassette, IFormFile cassettePhotoFile, string cassettePhotoPath)
        {
            if (cassettePhotoFile != null)
            {
                var fileName = Path.GetFileName(cassettePhotoFile.FileName);
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await cassettePhotoFile.CopyToAsync(fileStream);
                }

                cassette.CassettePhoto = "/images/" + fileName;
            }

            var cas = _mapper.Map<Cassette>(cassette);
            var id = _cassetteRepo.AddCassette(cas);
            return id;
        }

        public ListCassetteForListVm GetAllCassetteForList(int pageSize, int pageNo, string searchString)
        {
            var cassette = _cassetteRepo.GetAllCassettes().Where(p => p.MovieTitle.StartsWith(searchString) ||
            p.DirectorLastName.StartsWith(searchString) || p.DirectorFirstName.StartsWith(searchString))
                .ProjectTo<CassetteForListVm>(_mapper.ConfigurationProvider).ToList();

            var cassetteToShow = cassette.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList();
            var cassetteList = new ListCassetteForListVm()
            {
                PageSize = pageSize,
                CurrentPage = pageNo,
                SearchString = searchString,
                Cassettes = cassetteToShow,
                Count = cassette.Count
            };
            return cassetteList;
        }

        public CassetteDetailsVm GetCassetteDetails(int cassetteId)
        {
            var cassette = _cassetteRepo.GetCassette(cassetteId);
            var cassetteVm = _mapper.Map<CassetteDetailsVm>(cassette);
            return cassetteVm;
        }

        public NewCassetteVm GetCassetteForEdit(int id)
        {
            var cassette = _cassetteRepo.GetCassette(id);
            var cassetteVm = _mapper.Map<NewCassetteVm>(cassette);
            return cassetteVm;
        }

        public void UpdateCassette(NewCassetteVm model)
        {
            var cassette = _mapper.Map<Cassette>(model);
            _cassetteRepo.UpdateCassette(cassette);
        }

        public void DeleteCassette(int id)
        {
            _cassetteRepo.DeleteCassette(id);
        }
    }
}