using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class RegistrarVentaResult : ReservarResult
    {
        public int id_cabecera_venta {get;set;}
        public int id_venta {get;set;}
        public int id_pedido { get; set; }
        public int id_persona_remitente { get; set; }
    }
}
