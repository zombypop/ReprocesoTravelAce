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
                //string qry = @"SELECT NUMEROCOTIZACION, NUMEROPOLIZA 
                //            FROM ADMAPYA.COT_COTIZACION@P04_RONLY.BCISEGUROS.CL 
                //            WHERE  TIPODOCID = 1 
                //            AND ESTADOCARGA IS NULL
                //            AND NROCOTTRAVELACE IS NOT NULL 
                //            AND ESTADOCARGAASCEL IS NULL
                //            AND FECHACOTIZACION > SYSDATE-10";

                //string qry = @"SELECT   
                //            cot.NUMEROCOTIZACION, cot.NUMEROPOLIZA 
                //            FROM ADMAPYA.cot_cotizacion@P04_RONLY.BCISEGUROS.CL cot
                //            join ADMAPYA.cot_intencionpago@P04_RONLY.BCISEGUROS.CL inte
                //            on cot.NUMEROCOTIZACION = inte.NUMEROCOTIZACION
                //            where numeropoliza is not null
                //            and TIPODOCID=2 and to_char(tRunc(FECHACOTIZACION),'YYYYMM')='202108'
                //            and GLOSARESULTADO = 'Transacción aprobada'";

                string qry = @"SELECT NUMEROCOTIZACION, NUMEROPOLIZA 
                            FROM ADMAPYA.COT_COTIZACION@P04_RONLY.BCISEGUROS.CL 
                            WHERE  ESTADOCARGA IS NULL
                            AND NROCOTTRAVELACE IS NOT  NULL 
                            /*AND ESTADOCARGAASCEL IS NULL*/
                            AND NUMEROPOLIZA IS NOT NULL
                            AND FECHACOTIZACION > SYSDATE-60 AND NUMEROCOTIZACION != 578704 ";


                var lista = con.Query<CotizacionDto>(qry).ToList();
                return lista;
            }
        }

        public List<IntencionPago> ListaIntenciones(int numeroCotizacion)
        {
            using (IDbConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["oracle"].ConnectionString))
            {
                string qry = $@"SELECT TRX_ID FROM ADMAPYA.COT_INTENCIONPAGO@P04_RONLY.BCISEGUROS.CL 
                            WHERE NUMEROCOTIZACION = '{numeroCotizacion}'";
                var lista = con.Query<IntencionPago>(qry).ToList();
                return lista;
            }
        }

        public bool ExisteEnAcselX(int numeroPoliza)
        {
            bool existe = false;
            using (IDbConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["oracle"].ConnectionString))
            {
                string qry = $@"SELECT COUNT(*) as CUENTA FROM ACSEL.POLIZA @p01_ronly.bciseguros.cl where NUMPOL = {numeroPoliza} ";
                var lista = con.QueryFirst<int>(qry);
                if ( lista != 0)
                {
                    existe = true;
                }
                return existe;
            }
        }
    }
}
