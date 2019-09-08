using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finnfox.Models
{
    public class PieChartViewModel
    {
       public List<string> nasloviSaProcentima;
       public List<double> kolicineNovcaPoTipu;

        public PieChartViewModel()
        {

            nasloviSaProcentima = new List<string>();
            kolicineNovcaPoTipu = new List<double>();
        }

    }
}