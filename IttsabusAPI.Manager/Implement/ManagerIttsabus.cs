using IttsabusAPI.DataAccess;
using IttsabusAPI.Entidades;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.Manager
{
    public class ManagerIttsabus
    {
        Logger logger;
        int idkupos;
        int idpuntoventa;
        public ManagerIttsabus(Logger _logger, int _idkupos, int _idpuntoventa)
        {
            logger= _logger;
            idkupos = _idkupos;
            idpuntoventa = _idpuntoventa;
        }

        public List<Ciudad> ListCiudades()
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.ListCuidades();
                if (json != null && json != "[]")
                {
                    return JsonConvert.DeserializeObject<List<Ciudad>>(json);

                }
                return new List<Ciudad>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Viaje> ListViajes(int origen, int destino, string fecha)
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.ListViajes(origen,destino,fecha);
                if (json != null && json != "[]")
                {
                    var viajes = JsonConvert.DeserializeObject<List<Viaje>>(json);
                    foreach (var v in viajes)
                    {
                        json = repo.ListTramos(v.id_programacion);
                        if (json != null && json != "[]")
                        {
                            v.tramos = JsonConvert.DeserializeObject<List<Tramo>>(json);
                        }
                    }

                    return viajes; // JsonConvert.DeserializeObject<List<Viaje>>(json);

                }
                return new List<Viaje>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AsientoViaje> ListAsientos(int idprogramacion)
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.ListAsientos(idprogramacion);
                if (json != null && json != "[]")
                {
                   
                    return JsonConvert.DeserializeObject<List<AsientoViaje>>(json);

                }
                return new List<AsientoViaje>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ReservarResult ReservarAsiento(int idprogramacion, int idasiento, int asiento)
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.ReservarAsiento(idprogramacion,idasiento,asiento);
                if (json != null && json != "[]")
                {
                    return JsonConvert.DeserializeObject<ReservarResult>(json);

                }
                return new ReservarResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RegistrarVentaResult RegistrarVenta(RegistrarVentaQuery registro)
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.ConfirmarVenta(registro);
                if (json != null && json != "[]")
                {
                    return JsonConvert.DeserializeObject<RegistrarVentaResult>(json);

                }
                return new RegistrarVentaResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RegistraBoletoResult RegistrarBoleto(RegistraBoletoQuery registro)
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.RegistrarBoleto(registro);
                if (json != null && json != "[]")
                {
                    return JsonConvert.DeserializeObject<RegistraBoletoResult>(json);

                }
                return new RegistraBoletoResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RegistraTrackingResult RegistrarTracking(RegistraTrackingQuery registro)
        {
            try
            {
                var repo = new Repo_Queries(logger, idkupos, idpuntoventa);
                var json = repo.RegistrarTracking(registro);
                if (json != null && json != "[]")
                {
                    return JsonConvert.DeserializeObject<RegistraTrackingResult>(json);

                }
                return new RegistraTrackingResult();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
