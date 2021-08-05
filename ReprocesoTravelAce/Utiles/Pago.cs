using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReprocesoTravelAce.Utiles
{
    public class Pago
    {
        public RespuestaPago ValidarPagoWebPay(string idTrx)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string urlConsultaPago = "https://online.bciseguros.cl/wsaccionespago/services/WsConsulta";
            var client = new RestClient(urlConsultaPago);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("SOAPAction", "");
            var cuerpoXml = CuerpoIdTrx(idTrx);
            request.AddParameter("text/xml", cuerpoXml, ParameterType.RequestBody);

            var response = client.Execute(request);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(response.Content);
            var codigoResultado = xmlDoc.GetElementsByTagName("codigoResultado")[0].InnerXml;

            var resp = new RespuestaPago()
            {
                PagoOk = codigoResultado == "1",
                XmlRespuesta = response.Content
            };

            return resp;

        }

        private string CuerpoIdTrx(string idtrx)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<soapenv:Envelope xmlns:soapenv = 'http://schemas.xmlsoap.org/soap/envelope/' ");
            sb.Append("xmlns:ws = 'http://ws.wsacciones.cl'>");
            sb.Append("<soapenv:Header/><soapenv:Body><ws:consultaEstadoTrx>");
            sb.Append($"<ws:idTrx>{idtrx}</ws:idTrx >");
            sb.Append("</ws:consultaEstadoTrx></soapenv:Body></soapenv:Envelope >");
            return sb.ToString();
        }



    }

    public class RespuestaPago
    {
        public bool PagoOk { get; set; }
        public string XmlRespuesta { get; set; }
    }

}
