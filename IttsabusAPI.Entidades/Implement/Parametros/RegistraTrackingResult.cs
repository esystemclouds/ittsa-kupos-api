using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Entidades
{
    public class RegistraTrackingResult : ReservarResult
    {
        public int id_log_auditoria { get; set; }
        public int id_tracking_boleto { get; set; }
        public int id_tracking_asiento { get; set; }
    }
}
