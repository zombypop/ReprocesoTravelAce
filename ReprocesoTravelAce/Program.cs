using Dapper;
using Newtonsoft.Json;
using NLog;
using Oracle.ManagedDataAccess.Client;
using ReprocesoTravelAce.Implementacion;
using ReprocesoTravelAce.LogicaNegocio;
using ReprocesoTravelAce.Utiles;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce
{
    class Program
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                logger.Info("___________INICIA___________");
                var gestionador = new GestionarCotizacion(
                    new AccesoDatos(), 
                    new Utiles.Utiles(), 
                    new Pago());
                gestionador.EmitirCotizaciones();
                gestionador.ConsultarPolizasCargadasAcsel();
                logger.Info("___________FIN___________");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.Message);
            }
        }
    }

}
