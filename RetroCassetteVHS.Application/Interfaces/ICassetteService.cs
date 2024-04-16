using RetroCassetteVHS.Application.ViewModels.Cassette;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroCassetteVHS.Application.Interfaces
{
    public interface ICassetteService
    {
        ListCassetteForListVm GetAllCassetteForList(int pageSize, int pageNo, string searchString);
        public int AddCassette(NewCassetteVm cassette);
        CassetteDetailsVm GetCassetteDetails(int id);
        NewCassetteVm GetCassetteForEdit(int id);
        void UpdateCassette(NewCassetteVm model);
        void DeleteCassette(int id);
    }
}
