using Microsoft.AspNetCore.Http;
using RetroCassetteVHS.Application.ViewModels.Cassette;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroCassetteVHS.Application.Interfaces
{
    public interface ICassetteService
    {
        ListCassetteForListVm GetAllCassetteForList(int pageSize, int pageNo, string searchString);
        Task <int> AddCassette(NewCassetteVm cassette, IFormFile cassettePhotoFile, string cassettePhotoPath);
        CassetteDetailsVm GetCassetteDetails(int id);
        NewCassetteVm GetCassetteForEdit(int id);
        void UpdateCassette(NewCassetteVm model);
        void DeleteCassette(int id);
    }
}
