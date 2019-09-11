using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace finnfox.Models
{
    public class RacunovodstvenePromeneTipMesecViewModel
    {
        List<int> meseci;
        List<KeyValuePair<string, List<int> > > promenePoTipu;

        public RacunovodstvenePromeneTipMesecViewModel()
        {

            meseci = new List<int>();
            promenePoTipu = new List<KeyValuePair<string, List<int>>>();

        }
    }
}