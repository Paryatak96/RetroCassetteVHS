using Microsoft.AspNetCore.Http;
using RetroCassetteVHS.Application.ViewModels.Cassette;
using RetroCassetteVHS.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroCassetteVHS.Application.Interfaces
{
    public interface ICassetteService
    {
        ListCassetteForListVm GetAllCassetteForList(int pageSize, int pageNo, string searchString);
        Task <int> AddCassette(NewCassetteVm cassette, IFormFile cassettePhotoFile, string cassettePhotoPath);
        Task <Cassette> GetCassetteDetails(int id);
        Task <List<Rental>> GetActiveRentalsForCassetteAsync(int cassetteId);
        void UpdateCassette(NewCassetteVm model);
        NewCassetteVm GetCassetteForEdit(int id);
        void DeleteCassette(int id);
    }
}
