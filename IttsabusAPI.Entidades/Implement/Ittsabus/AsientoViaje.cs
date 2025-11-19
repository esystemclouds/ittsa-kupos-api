using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class AsientoViaje
    {
        public int id_programacion_padre { get; set; }
        public int id_programacion { get; set; }
        public int id_asiento_venta	{get;set;}
        public int nu_fila {get;set;}	
        public int nu_asiento {get;set;}
        public int nu_piso {get;set;}
        public string co_estado_asiento {get;set;}
        public string no_tipo_asiento {get;set;}
        public int nu_columna {get;set;}
        public string id_clase_asiento {get;set;}
        public string no_clase_asiento {get;set;}
        public decimal va_precio { get; set; }

    }
}
