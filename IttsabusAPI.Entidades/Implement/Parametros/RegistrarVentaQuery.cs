using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class RegistrarVentaQuery
    {

        public int? id_punto_venta {get;set;}
        public int? id_punto_venta_registro {get;set;}
        public int nu_boleto_venta {get;set;}
        public decimal va_monto_total {get;set;}
        public string nu_operacion {get;set;}
        public string co_tipo_tarjeta {get;set;}
        public int id_programacion_ida { get; set; }
        public int? id_programacion_retorno { get; set; }
        public string nu_pedido_safety { get; set; }
        public string nu_referencia_safety { get; set; }
        public int? id_pagador { get; set; }
        public int id_destino { get; set; }
        public int id_ciudad_destino { get; set; }
        public int id_ciudad_origen { get; set; }
        public string nu_ruc { get; set; }
        public string no_razon_social { get; set; }
        public string de_direccion { get; set; }
        public int id_usuario { get; set; }
        public int id_tipo_documento { get; set; }
        public string nu_documento { get; set; }
        public string am_pasajero { get; set; }
        public string ap_pasajero { get; set; }
        public string no_pasajero { get; set; }
    }
}
