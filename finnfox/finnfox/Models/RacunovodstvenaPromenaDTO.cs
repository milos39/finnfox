using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finnfox.Models
{
    public class RacunovodstvenaPromenaDTO
    {
        

      
        public string NazivPromene { get; set; }

        public string DatumPromene { get; set; }

        public int TipPromeneId { get; set; }

        public string  TipRacunovodstvenePromene { get; set; }

        public double KolicinaNovca { get; set; }

        public string Valuta { get; set; }

    }
}