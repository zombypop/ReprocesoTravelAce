using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce.Dto
{
    public class ParametrosPago
    {
        public string codigoResultado { get; set; } = "0";
        public string glosaResultado { get; set; } = "Transacción aprobada";
        public string idTrxCanal { get; set; }
        public string nroTrxMedioPago { get; set; } = "537252";
        public string ultNroTarjeta { get; set; } = "XXXX-XXXX-XXXX-9016";
        public string medioPago { get; set; } = "WebPay";
    }
}
