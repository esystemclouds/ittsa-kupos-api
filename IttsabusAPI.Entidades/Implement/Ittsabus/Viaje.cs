using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class Viaje 
    {

        public int id_programacion_padre { get; set; }
        public int id_programacion { get; set; }
        public int id_destino { get; set; }


        public int id_ciudad_origen { get; set; }
        public int id_ciudad_destino { get; set; }
        public string fe_programacion { get; set; }
        public string de_hora_salida { get; set; }
        public string de_am_pm { get; set; }
        public string de_clase_servicio { get; set; }
        public int nu_horas_viaje { get; set; }
        public string no_tipo_asiento_piso1 { get; set; }
        public string no_tipo_asiento_piso2 { get; set; }
        public DateTime fe_salida_planificada { get; set; }
        public DateTime fe_llegada_planificada { get; set; }
        //public int id_piso {get;set;}		
        //public string co_tipo_asiento {get;set;}
        public string origen { get; set; }
        public string destino { get; set; }
        public string dir_origen { get; set; }
        public string dir_destino { get; set; }
        public List<Tramo> tramos { get; set; }
    }
}
