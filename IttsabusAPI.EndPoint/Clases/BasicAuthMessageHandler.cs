using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace IttsabusAPI.EndPoint.Clases
{
    public class BasicAuthMessageHandler : DelegatingHandler
    {
        Logger logger = NLog.LogManager.GetLogger("loggerromani");
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string msgReturn = "";

            var headers = request.Headers;
            if (headers.Authorization != null && headers.Authorization.Scheme == "Basic")
            {
                string AppUser = string.Empty;
                string AppKey = string.Empty;
                string AuthorizationKey = string.Empty;


                AuthorizationKey = headers.Authorization.Parameter;

                if (!FuncionesComunes.IsBase64String(AuthorizationKey))
                {
                    msgReturn = "";
                    msgReturn += "\n" + request.RequestUri.PathAndQuery;
                    msgReturn += "\n" + HttpStatusCode.BadRequest.ToString();
                    msgReturn += ": AuthorizationKey Incorrecto..";
                    msgReturn += "\n";

                    logger.Error(msgReturn);
                    HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //message.Content = new StringContent(msgReturn);

                    return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.BadRequest) { });
                }

                AuthorizationKey = FuncionesComunes.Decrypt(AuthorizationKey);
                //Validar AuthorizationKey: 
                if (AuthorizationKey != "ITTSABUS_ENDPOINT")
                {
                    msgReturn = "";
                    msgReturn += "\n" + request.RequestUri.PathAndQuery;
                    msgReturn += "\n" + HttpStatusCode.BadRequest.ToString();
                    msgReturn += ": {Authorization} valor Incorrecto..";
                    msgReturn += "\n";

                    logger.Error(msgReturn);
                    HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //message.Content = new StringContent(msgReturn);

                    return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.BadRequest) { });
                }

                if (headers.Contains("API-AppUser"))
                {
                    AppUser = headers.GetValues("API-AppUser").First();
                }
                else
                {
                    msgReturn = "";
                    msgReturn += "\n" + request.RequestUri.PathAndQuery;
                    msgReturn += "\n" + HttpStatusCode.BadRequest.ToString();
                    msgReturn += ": {API-AppUser} no indicado..";
                    msgReturn += "\n";

                    logger.Error(msgReturn);
                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                    //message.Content = new StringContent(msgReturn);
                    HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));

                    return Task<HttpResponseMessage>.Factory.StartNew(() => message); // new HttpResponseMessage(HttpStatusCode.BadRequest) { });
                }

                if (headers.Contains("API-AppKey"))
                {
                    AppKey = headers.GetValues("API-AppKey").First();
                    if (!FuncionesComunes.IsBase64String(AppKey))
                    {
                        msgReturn = "";
                        msgReturn += "\n" + request.RequestUri.PathAndQuery;
                        msgReturn += "\n" + HttpStatusCode.BadRequest.ToString();
                        msgReturn += ": {API-AppKey} incorrecto..";
                        msgReturn += "\n";

                        logger.Error(msgReturn);
                        HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                        //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //message.Content = new StringContent(msgReturn);

                        return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.BadRequest) { });
                    }
                    AppKey = FuncionesComunes.Decrypt(AppKey);
                }
                else
                {
                    msgReturn = "";
                    msgReturn += "\n" + request.RequestUri.PathAndQuery;
                    msgReturn += "\n" + HttpStatusCode.Unauthorized.ToString();
                    msgReturn += ": {API-AppKey} no indicado..";
                    msgReturn += "\n";

                    logger.Error(msgReturn);
                    HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    //message.Content = new StringContent(msgReturn);

                    return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                }

                //aqui validar credenciales 

                if (AppUser != "apius" || AppUser == string.Empty)
                {
                    msgReturn = "";
                    msgReturn += "\n" + request.RequestUri.PathAndQuery;
                    msgReturn += "\n" + HttpStatusCode.Unauthorized.ToString();
                    msgReturn += ": {API-AppUser} no valido..";
                    msgReturn += "\n";

                    logger.Error(msgReturn);
                    HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    //message.Content = new StringContent(msgReturn);

                    return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                }

                if (AppKey != "3sp0m3g@")
                {
                    msgReturn = "";
                    msgReturn += "\n" + request.RequestUri.PathAndQuery;
                    msgReturn += "\n" + HttpStatusCode.Unauthorized.ToString();
                    msgReturn += ": {API-AppKey} no valido..";
                    msgReturn += "\n";

                    logger.Error(msgReturn);
                    HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                    //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    //message.Content = new StringContent(msgReturn);

                    return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.Unauthorized) { });
                }
                //--------------------------------------------------------------


                if (headers.Contains("sandbox"))
                {
                    string sandboxValor = headers.GetValues("sandbox").First();
                    logger.Error("valor sandbox: {0}", sandboxValor);

                    if (!FuncionesComunes.IsNumber(sandboxValor))
                    {
                        msgReturn = "";
                        msgReturn += "\n" + request.RequestUri.PathAndQuery;
                        msgReturn += "\n" + HttpStatusCode.BadRequest.ToString();
                        msgReturn += ": {sandbox} Incorrecto.";
                        msgReturn += "\n";



                        logger.Error("{0} valor sandbox: {1}", msgReturn, sandboxValor);

                        HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.InternalServerError, msgReturn.Replace("\n", " ")); //HttpResponseMessage(HttpStatusCode.BadRequest);

                        return Task<HttpResponseMessage>.Factory.StartNew(() => message);
                        //return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.BadRequest) { });
                    }
                    else if (!(Convert.ToInt16(sandboxValor) < 6 && Convert.ToInt16(sandboxValor) >= 0))
                    {
                        msgReturn = "";
                        msgReturn += "\n" + request.RequestUri.PathAndQuery;
                        msgReturn += "\n" + HttpStatusCode.BadRequest.ToString();
                        msgReturn += ": {sandbox} Incorrecto..";
                        msgReturn += "\n";

                        logger.Error("{0} valor sandbox: {1}", msgReturn, sandboxValor);

                        HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));
                        //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //message.Content = new StringContent(msgReturn);


                        return Task<HttpResponseMessage>.Factory.StartNew(() => message); //new HttpResponseMessage(HttpStatusCode.BadRequest) { });
                    }
                }

                //--------------------------------------------------------------

                //var userPwd = Decrypt(headers.Authorization.Parameter);// Encoding.UTF8.GetString(Convert.FromBase64String(headers.Authorization.Parameter));
                //var user = userPwd.Substring(0, userPwd.IndexOf(':'));
                //var password = userPwd.Substring(userPwd.IndexOf(':') + 1);

                // Validamos user y password (aquí asumimos que siempre son ok)
                var principal = new GenericPrincipal(new GenericIdentity(AppUser), null);
                PutPrincipal(principal);
            }
            else
            {
                msgReturn = "";
                msgReturn += "\n" + request.RequestUri.PathAndQuery;
                msgReturn += "\n" + HttpStatusCode.Unauthorized.ToString();
                msgReturn += ": {Authorization} no indicado..";
                msgReturn += "\n";

                logger.Error(msgReturn);
                HttpResponseMessage message = request.CreateErrorResponse(HttpStatusCode.BadRequest, msgReturn.Replace("\n", " "));

                //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                //message.Content = new StringContent(msgReturn);
                return Task<HttpResponseMessage>.Factory.StartNew(() => message);
            }
            return base.SendAsync(request, cancellationToken);
        }

        private void PutPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}