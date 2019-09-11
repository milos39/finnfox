using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finnfox.Models
{
    public class RacunovodstvenePromeneTipMesecViewModel
    {
        public List<int> meseci { get; set; }
        public List<KeyValuePair<string, List<int>>> promenePoTipu { get; set; }
      

        public RacunovodstvenePromeneTipMesecViewModel()
        {

            meseci = new List<int>();
            promenePoTipu = new List<KeyValuePair<string, List<int>>>();

        }
    }
}