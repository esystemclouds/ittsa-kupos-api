using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class MensajeResult
    {
        public int Codigoresultado { get; set; }
        public string Resultado { get; set; }
        public dynamic Data { get; set; }
    }
}
