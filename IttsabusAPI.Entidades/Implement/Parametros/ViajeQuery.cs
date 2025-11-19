using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class ViajeQuery
    {
        public int Origen { get; set; }
        public int Destino { get; set; }
        public string Fecha { get; set; } // o DateTime si usarás ISO
    }
}
