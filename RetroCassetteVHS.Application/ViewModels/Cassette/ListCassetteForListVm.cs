using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroCassetteVHS.Application.ViewModels.Cassette
{
    public class ListCassetteForListVm
    {
        public List<CassetteForListVm> Cassettes { get; set; }
        public int Count { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public object Cassette { get; internal set; }
    }
}
