using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class RegistraBoletoQuery
    {
        public int id_venta { get; set; }
        public int id_asiento_venta { get; set; }
        public decimal va_venta { get; set; }
        public string co_tipo_tarjeta { get; set; }
        public string numero_operacion { get; set; }
        public string am_pasajero { get; set; }
        public string ap_pasajero { get; set; }
        public string no_pasajero { get; set; }
        public string nu_documento { get; set; }
        public int? id_pasajero { get; set; }
        public int id_destino { get; set; }
        public int id_ciudad_origen { get; set; }
        public int id_ciudad_destino { get; set; }
        public int id_usuario { get; set; }
        public string de_total_letras { get; set; }
        public string no_oficina_venta { get; set; }
        public int nu_asiento { get; set; }
        public string nu_ip_venta { get; set; }
        public int id_programacion { get; set; }
        public string nu_ruc { get; set; }
        public string no_razon_social { get; set; }
        public string de_direccion { get; set; }
        public int id_tipo_documento { get; set; }
        public int id_persona_remitente { get; set; }
        public int id_pedido {get;set;}
        public string mo_asiento {get; set;}

    }
}
