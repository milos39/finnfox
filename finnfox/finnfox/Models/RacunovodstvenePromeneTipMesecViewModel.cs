using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finnfox.Models
{
    public class RacunovodstvenePromeneTipMesecViewModel
    {


        public List<int> meseci { get; set; }
        public List<List<double>> vrednostiPoKategoriji { get; set; }
        public List<string> kategorije { get; set; }


        public RacunovodstvenePromeneTipMesecViewModel()
        {

            meseci = new List<int>();
            kategorije = new List<string>();
            List<List<double>> vrednostiPoKategoriji = new List<List<double>>();



        }
    }
}