using IttsabusAPI.EndPoint.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace IttsabusAPI.EndPoint
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Configuración y servicios de Web API

            // Rutas de Web API
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new BasicAuthMessageHandler());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
