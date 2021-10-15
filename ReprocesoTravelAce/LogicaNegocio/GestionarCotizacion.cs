using Newtonsoft.Json;
using NLog;
using ReprocesoTravelAce.Dto;
using ReprocesoTravelAce.Interfaces;
using ReprocesoTravelAce.Utiles;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce.LogicaNegocio
{
    public class GestionarCotizacion : IGestionarCotizacion
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IAccesoDatos accesoDatos;
        private readonly Utiles.Utiles utiles;
        private readonly Pago pago;

        private List<CotizacionDto> listaCotizaciones;

        public GestionarCotizacion(IAccesoDatos accesoDatos, Utiles.Utiles utiles, Pago pago)
        {
            this.accesoDatos = accesoDatos;
            this.utiles = utiles;
            this.pago = pago;
            this.listaCotizaciones = new List<CotizacionDto>();
        }

        public void LlamarSitioExito(int nroCotizacion)
        {
            var cot = new ParametrosPago();
            cot.idTrxCanal = $"APV_{nroCotizacion}";

            var resp = GetResponse("http://asistenciatravelace.bciseguros.cl/ExitoTransaccion.aspx?rut=OTY1NzM2MDA=", cot);
            logger.Info("exito: {esOk}", resp.Contains("La transacci�n fue realizada con �xito, su p�liza fue enviada a su correo electr�nico."));
        }

        public List<CotizacionDto> ObtenerCotizacionesPagadasNoEmitidas()
        {
            return accesoDatos.ListaCotizaciones();
        }

        public void RecorrerCotizacionesParaEmitir(List<CotizacionDto> cotizaciones)
        {
            foreach (var item in cotizaciones)
            {
                foreach (var intencion in item.IntencionesPago)
                {
                    if (VerificarPagoServicio(intencion.TRX_ID))
                    {
                        item.Pagada = true;
                        //logger.Info("Intenciona pagada: {cot} --> {idtrx}", item.NUMEROCOTIZACION , intencion.TRX_ID);
                        //existe en url pdf?
                        var urlPoliza = $"https://asistenciatravelace.bciseguros.cl/Bandeja/Poliza/{item.NUMEROPOLIZA}.pdf";
                        if (GetResponse(urlPoliza) == false)
                        {
                            LlamarSitioExito(item.NUMEROCOTIZACION);
                            logger.Info("EMITE: {@cot}", item);
                        }
                        //else
                        //{
                        //    logger.Info("Ya existe poliza: {@cot}", item.NUMEROCOTIZACION);
                        //}
                    }
                }
            }
        }

        string GetResponse(string url, ParametrosPago param)
        {
            var client = new RestClient(url);
            var request = new RestRequest("", RestSharp.Method.POST);
            var jsonString = JsonConvert.SerializeObject(param);
            var texto = utiles.Codificar(jsonString);
            request.AddParameter("params", texto);
            var response = client.Execute(request);
            return response.Content;
        }

        bool GetResponse(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(RestSharp.Method.GET);
            request.AddHeader("Accept", "application/pdf");
            var response = client.Execute(request);
            //var data = client.DownloadData(request);
            return response.IsSuccessful;
        }


        public void EmitirCotizaciones()
        {
            listaCotizaciones = ObtenerCotizacionesPagadasNoEmitidas();
            foreach (var item in listaCotizaciones)
            {
                item.IntencionesPago = accesoDatos.ListaIntenciones(item.NUMEROCOTIZACION);
            }
            RecorrerCotizacionesParaEmitir(listaCotizaciones);
        }

        public bool VerificarPagoServicio(string idtrx)
        {
            var esPago = pago.ValidarPagoWebPay(idtrx);
            return esPago.PagoOk;
        }

        public void ConsultarPolizasCargadasAcsel()
        {
            foreach (var item in listaCotizaciones.Where(x=>x.Pagada) )
            {
                if (!accesoDatos.ExisteEnAcselX(item.NUMEROPOLIZA))
                {
                    logger.Info("Poliza {pol} NO esta cargada en Acsel", item.NUMEROPOLIZA);
                }
            }
        }

    }

}
