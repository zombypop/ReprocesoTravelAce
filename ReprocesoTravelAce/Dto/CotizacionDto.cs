using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce.Dto
{
    public class CotizacionDto
    {
        public int NUMEROCOTIZACION { get; set; }
        public int NUMEROPOLIZA { get; set; }
        public List<IntencionPago> IntencionesPago { get; set; }
        public bool Pagada { get; set; }
    }

    public class IntencionPago
    {
        public string TRX_ID { get; set; }
    }
}
