using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finnfox.Models
{
    public class ListRacunovodstvenaPromenaMesecViewModel
    {
        public IList<RacunovodstvenaPromenaDTO> racunovodstvenePromene { get; set; }
        public double balans { get; set; }
        public IList<int> godine { get; set; }
        public IList<int> meseciZaDatuGodinu { get; set; }

        public ListRacunovodstvenaPromenaMesecViewModel()
        {
            racunovodstvenePromene = new List<RacunovodstvenaPromenaDTO>();
            godine = new List<int>();
        }
    }
}