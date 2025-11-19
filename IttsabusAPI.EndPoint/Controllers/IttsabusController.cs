using IttsabusAPI.Entidades;
using IttsabusAPI.Manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace IttsabusAPI.EndPoint.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1")]
    public class IttsabusController : ApiController
    {
        Logger logger = NLog.LogManager.GetLogger("loggerendpoint");

        [HttpGet]
        [Route("ciudades")]
        public IHttpActionResult ListarDestino(HttpRequestMessage request)
        {
            MensajeResult msg = new MensajeResult();
            try
            {
                var config = GetSetting();
                ManagerIttsabus mng = new ManagerIttsabus(logger,config.uskupos, config.puntoventa);
                List<Ciudad> list = mng.ListCiudades();
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);

                msg.Codigoresultado = 0;
                msg.Resultado = "OK";
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;

                //return Ok(new { mensaje = msg });
                //logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }
        }

        [HttpGet]
        [Route("viajes")]
        public IHttpActionResult ListarViajes([FromUri] ViajeQuery q)
        {
            MensajeResult msg = new MensajeResult();

            if (q == null)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject("Parámetros requeridos.");
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.BadRequest, msg);
            }
            if (!DateTime.TryParseExact(q.Fecha, "dd-MM-yyyy", new CultureInfo("es-CL"),
                                DateTimeStyles.None, out var fecha))
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject("Formato de fecha inválido (dd-MM-yyyy).");
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.BadRequest, msg);
            }

            try
            {
                //var qs = HttpUtility.ParseQueryString(request.RequestUri.Query);

                int origen = q.Origen;
                int destino = q.Destino;
                var fe = DateTime.ParseExact(q.Fecha, "dd/MM/yyyy", new CultureInfo("es-CL"));

                var config = GetSetting();
                ManagerIttsabus mng = new ManagerIttsabus(logger, config.uskupos, config.puntoventa);
                List<Viaje> list = mng.ListViajes(origen,destino,String.Format("{0:dd-MM-yyyy}", fe));
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);

                msg.Codigoresultado = 0;
                msg.Resultado = "OK";
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;
                //logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg);
            }
        }

        [HttpGet]
        [Route("asientos")]
        public IHttpActionResult ListarAsientosViaje(HttpRequestMessage request)
        {
            MensajeResult msg = new MensajeResult();

            var qs = HttpUtility.ParseQueryString(request.RequestUri.Query);
            if(qs==null)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject("Parámetros requeridos.");
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.BadRequest, msg) ;
                
            }
            
            if(qs["idprogramacion"]==null)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject("Parámetros requeridos.");
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.BadRequest, msg );
            }
            try
            {

                int idprogramacion = int.Parse(qs["idprogramacion"]);

                var config = GetSetting();
                ManagerIttsabus mng = new ManagerIttsabus(logger, config.uskupos, config.puntoventa);
                List<AsientoViaje> list = mng.ListAsientos(idprogramacion);
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);

                msg.Codigoresultado = 0;
                msg.Resultado = "OK";
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;
                //logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }
        }

        [HttpPost]
        [Route("reservar-asiento")]
        public IHttpActionResult ReservarAsiento(HttpRequestMessage request)
        {
            MensajeResult msg = new MensajeResult();
            var jsonRequest = request.Content.ReadAsStringAsync().Result;
            logger.Info("RequestUri: {0} JsonEntrada: {1}", request.RequestUri, jsonRequest);
            JObject jsonintput;
            try
            {

                logger.Info("{0}", jsonRequest.ToString());
                jsonintput = JObject.Parse(jsonRequest);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "Body Json Incorrecto";
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }

            try
            {
                AsientoQuery asiento = JsonConvert.DeserializeObject<AsientoQuery>(jsonRequest);

                var config = GetSetting();

                ManagerIttsabus mng = new ManagerIttsabus(logger, config.uskupos, config.puntoventa);
                var result = mng.ReservarAsiento(asiento.id_programacion_padre, asiento.id_asiento_venta, asiento.nu_asiento);


                msg.Codigoresultado = result.CodRespuesta;
                msg.Resultado = result.Mensaje;
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;
                logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }


        }


        [HttpPost]
        [Route("confirmar-venta")]
        public IHttpActionResult ConfirmarVenta(HttpRequestMessage request)
        {
            MensajeResult msg = new MensajeResult();
            var jsonRequest = request.Content.ReadAsStringAsync().Result;
            logger.Info("RequestUri: {0} JsonEntrada: {1}", request.RequestUri, jsonRequest);
            JObject jsonintput;
            try
            {

                logger.Info("{0}", jsonRequest.ToString());
                jsonintput = JObject.Parse(jsonRequest);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "Body Json Incorrecto";
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }

            try
            {
                RegistrarVentaQuery registro = JsonConvert.DeserializeObject<RegistrarVentaQuery>(jsonRequest);

                var config = GetSetting();

                ManagerIttsabus mng = new ManagerIttsabus(logger, config.uskupos, config.puntoventa);
                var result = mng.RegistrarVenta(registro);


                msg.Codigoresultado = result.CodRespuesta;
                msg.Resultado = result.Mensaje;
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;
                logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }


        }

        [HttpPost]
        [Route("generar-boleto")]
        public IHttpActionResult RegistrarBoleto(HttpRequestMessage request)
        {
            MensajeResult msg = new MensajeResult();
            var jsonRequest = request.Content.ReadAsStringAsync().Result;
            logger.Info("RequestUri: {0} JsonEntrada: {1}", request.RequestUri, jsonRequest);
            JObject jsonintput;
            try
            {

                logger.Info("{0}", jsonRequest.ToString());
                jsonintput = JObject.Parse(jsonRequest);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "Body Json Incorrecto";
                logger.Error(JsonConvert.SerializeObject(msg));
                logger.Error(ex.Message); 
                return Content(HttpStatusCode.InternalServerError, msg );
            }

            try
            {
                RegistraBoletoQuery registro = JsonConvert.DeserializeObject<RegistraBoletoQuery>(jsonRequest);

                var config = GetSetting();

                ManagerIttsabus mng = new ManagerIttsabus(logger, config.uskupos, config.puntoventa);
                var result = mng.RegistrarBoleto(registro);


                msg.Codigoresultado = result.CodRespuesta;
                msg.Resultado = result.Mensaje;
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;
                logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError,  msg );
            }


        }

        [HttpPost]
        [Route("tracking")]
        public IHttpActionResult RegistrarTracking(HttpRequestMessage request)
        {
            MensajeResult msg = new MensajeResult();
            var jsonRequest = request.Content.ReadAsStringAsync().Result;
            logger.Info("RequestUri: {0} JsonEntrada: {1}", request.RequestUri, jsonRequest);
            JObject jsonintput;
            try
            {

                logger.Info("{0}", jsonRequest.ToString());
                jsonintput = JObject.Parse(jsonRequest);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "Body Json Incorrecto";
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError,  msg );
            }

            try
            {
                RegistraTrackingQuery registro = JsonConvert.DeserializeObject<RegistraTrackingQuery>(jsonRequest);

                var config = GetSetting();

                ManagerIttsabus mng = new ManagerIttsabus(logger, config.uskupos, config.puntoventa);
                var result = mng.RegistrarTracking(registro);


                msg.Codigoresultado = result.CodRespuesta;
                msg.Resultado = result.Mensaje;
                string json = JsonConvert.SerializeObject(result, Formatting.Indented);
                dynamic jsonObj = JsonConvert.DeserializeObject(json);
                msg.Data = jsonObj;
                logger.Info(JsonConvert.SerializeObject(msg));
                return Ok(msg);
            }
            catch (Exception ex)
            {
                msg.Codigoresultado = -1;
                msg.Resultado = "ERR";
                dynamic jsonObj = JsonConvert.SerializeObject(ex.Message);
                msg.Data = jsonObj;
                logger.Error(JsonConvert.SerializeObject(msg));
                return Content(HttpStatusCode.InternalServerError, msg );
            }


        }


        private ConfigSL GetSetting()
        {
            ConfigSL configSL = new ConfigSL
            {
                uskupos =int.Parse(ConfigurationManager.AppSettings.Get("ittsa_us")),
                puntoventa = int.Parse(ConfigurationManager.AppSettings.Get("ittsa_ptoventa"))
            };

            return configSL;
        }
    }
}
