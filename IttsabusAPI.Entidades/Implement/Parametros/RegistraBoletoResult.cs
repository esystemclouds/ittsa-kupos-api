using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class RegistraBoletoResult : ReservarResult
    {
        public int id_comprobante_pago {get;set;}
        public int id_detalle {get;set;}
        public int id_boleto {get;set;}
    }
}
