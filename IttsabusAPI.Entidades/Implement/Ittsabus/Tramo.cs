using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class Tramo
    {
        public int id_programacion_padre { get; set; }
        public int id_programacion { get; set; }
        public string fe_programacion { get; set; }
        public string de_hora_salida { get; set; }
        public string de_am_pm { get; set; }
        public int nu_horas_viaje { get; set; }
        public DateTime fe_salida_planificada { get; set; }
        public DateTime fe_llegada_planificada { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string dir_origen { get; set; }
        public string dir_destino { get; set; }

    }
}
