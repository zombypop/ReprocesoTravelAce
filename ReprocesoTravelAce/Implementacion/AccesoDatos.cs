using Dapper;
using Oracle.ManagedDataAccess.Client;
using ReprocesoTravelAce.Dto;
using ReprocesoTravelAce.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce.Implementacion
{
    public class AccesoDatos : IAccesoDatos
    {
        public List<CotizacionDto> ListaCotizaciones()
        {
            using (IDbConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["oracle"].ConnectionString))
            {
                string qry = @"SELECT NUMEROCOTIZACION, NUMEROPOLIZA 
                            FROM ADMAPYA.COT_COTIZACION@P04_RONLY.BCISEGUROS.CL 
                            WHERE  TIPODOCID = 1 
                            AND ESTADOCARGA IS NULL
                            AND NROCOTTRAVELACE IS NOT NULL 
                            AND ESTADOCARGAASCEL IS NULL
                            AND FECHACOTIZACION > SYSDATE-10";
                var lista = con.Query<CotizacionDto>(qry).ToList();
                return lista;
            }
        }

    }
}
