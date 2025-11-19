using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class RegistraTrackingQuery
    {
        public int id_asiento_venta { get; set; }
        public int nu_asiento {get;set;}
        public int id_usuario {get;set;}
        public int id_boleto {get; set;}
        public decimal va_venta { get; set; }
        public string numero_operacion { get; set; }
        public string am_pasajero { get; set; }
        public string ap_pasajero { get; set; }
        public string no_pasajero { get; set; }
        public int id_destino { get; set; }
        public int id_ciudad_origen { get; set; }
        public int id_ciudad_destino { get; set; }
        public int id_programacion { get; set; }
    }
}
