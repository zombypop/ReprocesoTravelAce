using ReprocesoTravelAce.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce.Interfaces
{
    public interface IGestionarCotizacion
    {
        List<CotizacionDto> ObtenerCotizacionesPagadasNoEmitidas();
        void RecorrerCotizacionesParaEmitir(List<CotizacionDto> cotizaciones);
    }
}
