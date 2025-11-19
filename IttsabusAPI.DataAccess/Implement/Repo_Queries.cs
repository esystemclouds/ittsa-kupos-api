using IttsabusAPI.Entidades;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IttsabusAPI.DataAccess
{
    public class Repo_Queries
    {
        Logger logger;
        int idkupos;
        int idpuntoventa;
        public Repo_Queries(Logger _logger, int _idkupos, int _idpuntoventa) {
            logger = _logger;
            idkupos = _idkupos;
            idpuntoventa = _idpuntoventa;

        }
        public string ListCuidades()
        {
            string sCmd = "";
            sCmd = @"
					SELECT ac.ID_CIUDADES as Id, ac.NOMBRE as Nombre 
                    FROM API_CIUDADES AS ac
					";
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            //JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;

        }

        public string ListViajes(int origen, int destino, string fecha)
        {
            string sCmd = "";
            sCmd = @"
					DECLARE
	                @idorigen INT ={0},
	                @iddestino INT ={1},
	                @fecha DATE ='{2}',
	                @idorigendestino VARCHAR(10)
	
                SET @idorigendestino=CAST(@idorigen AS VARCHAR(10)) + '-' + CAST(@iddestino AS VARCHAR(10))

				SELECT	CASE WHEN pg.IN_RUTA_PRINCIPAL=1 THEN pg.ID_PROGRAMACION ELSE pg.ID_PROGRAMACION_PADRE END AS ID_PROGRAMACION_PADRE,
						pg.ID_PROGRAMACION, 
						pg.ID_DESTINO,
						ago.ID_AGENCIA AS ID_AGENCIA_ORIGEN,
						agd.ID_AGENCIA AS ID_AGENCIA_DESTINO,
						ago.NO_AGENCIA AS ORIGEN,
						agd.NO_AGENCIA AS DESTINO,
						o.ID_CIUDADES AS ID_CIUDAD_ORIGEN,
						d.ID_CIUDADES AS ID_CIUDAD_DESTINO,
	 					CONVERT(VARCHAR(30), pg.FE_PROGRAMACION,103) AS FE_PROGRAMACION, 
						pg.DE_HORA_SALIDA, 
						pg.DE_AM_PM, 
						cs.DE_CLASE_SERVICIO, 
						pg.NU_HORAS_VIAJE, 
						pg.NO_TIPO_ASIENTO_PISO1, 
						pg.NO_TIPO_ASIENTO_PISO2, 
						pg.FE_SALIDA_PLANIFICADA,  
						pg.FE_LLEGADA_PLANIFICADA,
						o.ID_CIUDADES AS ID_ORIGEN
						,ofo.DE_DIRECCION AS DIR_ORIGEN
						,ofd.DE_DIRECCION AS DIR_DESTINO
				FROM PRTM_PROGRAMACION AS pg 
				JOIN ADTM_CLASE_SERVICIO AS cs ON cs.ID_CLASE_SERVICIO = pg.ID_CLASE_SERVICIO
				JOIN API_CIUDADES AS o ON o.ID_CIUDADES = CAST(SUBSTRING(pg.COD_CIUDADES,1,CHARINDEX('-', pg.COD_CIUDADES)-1) AS INT)
				JOIN API_CIUDADES AS d ON d.ID_CIUDADES = CAST(SUBSTRING(pg.COD_CIUDADES,CHARINDEX('-', pg.COD_CIUDADES)+1,3) AS INT) 

				JOIN PRTM_DESTINO AS pd ON pd.ID_DESTINO = pg.ID_DESTINO 

				JOIN ADTM_OFICINA AS ofo ON ofo.ID_OFICINA = pd.ID_OFICINA_ORIGEN
				JOIN ADTM_OFICINA AS ofd ON ofd.ID_OFICINA = pd.ID_OFICINA_DESTINO

				JOIN ADTM_AGENCIA AS ago ON ago.ID_AGENCIA = ofo.ID_AGENCIA
				JOIN ADTM_AGENCIA AS agd ON agd.ID_AGENCIA = ofd.ID_AGENCIA
				WHERE pg.COD_CIUDADES=@idorigendestino 
				AND pg.FE_PROGRAMACION=@fecha
				AND pg.ES_PROGRAMACION='A'
					";
            sCmd = String.Format(sCmd, origen, destino, fecha);
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            //JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;

        }

        
        public string ListTramos(int idprogramacion)
        {
            string sCmd = "";
            sCmd = @"
					DECLARE @idprogramacion INT={0}
					DECLARE 
						@ID_PROGRAMACION_PADRE INT
					SELECT @ID_PROGRAMACION_PADRE = CASE WHEN pp.IN_RUTA_PRINCIPAL=1 THEN pp.ID_PROGRAMACION ELSE pp.ID_PROGRAMACION_PADRE END FROM PRTM_PROGRAMACION AS pp WHERE pp.ID_PROGRAMACION IN(@idprogramacion)
                
					SELECT  CASE WHEN pg.IN_RUTA_PRINCIPAL=1 THEN pg.ID_PROGRAMACION ELSE pg.ID_PROGRAMACION_PADRE END AS ID_PROGRAMACION_PADRE,
							pg.ID_PROGRAMACION, 
							ago.NO_AGENCIA AS ORIGEN,
							agd.NO_AGENCIA AS DESTINO,
	 						CONVERT(VARCHAR(30), pg.FE_PROGRAMACION,103) AS FE_PROGRAMACION, 
							pg.DE_HORA_SALIDA, 
							pg.DE_AM_PM, 
							pg.NU_HORAS_VIAJE, 
							pg.FE_SALIDA_PLANIFICADA,  
							pg.FE_LLEGADA_PLANIFICADA  
							,ofo.DE_DIRECCION AS DIR_ORIGEN
							,ofd.DE_DIRECCION AS DIR_DESTINO
					FROM PRTM_PROGRAMACION AS pg
					JOIN ADTM_CLASE_SERVICIO AS cs ON cs.ID_CLASE_SERVICIO = pg.ID_CLASE_SERVICIO
					JOIN API_CIUDADES AS o ON o.ID_CIUDADES = CAST(SUBSTRING(pg.COD_CIUDADES,1,CHARINDEX('-', pg.COD_CIUDADES)-1) AS INT)
					JOIN API_CIUDADES AS d ON d.ID_CIUDADES = CAST(SUBSTRING(pg.COD_CIUDADES,CHARINDEX('-', pg.COD_CIUDADES)+1,3) AS INT)
					
					JOIN PRTM_DESTINO AS pd ON pd.ID_DESTINO = pg.ID_DESTINO 

					JOIN ADTM_OFICINA AS ofo ON ofo.ID_OFICINA = pd.ID_OFICINA_ORIGEN
					JOIN ADTM_OFICINA AS ofd ON ofd.ID_OFICINA = pd.ID_OFICINA_DESTINO

					JOIN ADTM_AGENCIA AS ago ON ago.ID_AGENCIA = ofo.ID_AGENCIA
					JOIN ADTM_AGENCIA AS agd ON agd.ID_AGENCIA = ofd.ID_AGENCIA

					WHERE pg.ID_PROGRAMACION=@ID_PROGRAMACION_PADRE
					AND pg.ES_PROGRAMACION='A'

					UNION
                
					SELECT  pg.ID_PROGRAMACION_PADRE,
							pg.ID_PROGRAMACION, 
							ago.NO_AGENCIA AS ORIGEN,
							agd.NO_AGENCIA AS DESTINO,
	 						CONVERT(VARCHAR(30), pg.FE_PROGRAMACION,103) AS FE_PROGRAMACION, 
							pg.DE_HORA_SALIDA, 
							pg.DE_AM_PM, 
							pg.NU_HORAS_VIAJE, 
							pg.FE_SALIDA_PLANIFICADA,  
							pg.FE_LLEGADA_PLANIFICADA  
							,ofo.DE_DIRECCION AS DIR_ORIGEN
							,ofd.DE_DIRECCION AS DIR_DESTINO
					FROM PRTM_PROGRAMACION AS pg
					JOIN ADTM_CLASE_SERVICIO AS cs ON cs.ID_CLASE_SERVICIO = pg.ID_CLASE_SERVICIO
					JOIN API_CIUDADES AS o ON o.ID_CIUDADES = CAST(SUBSTRING(pg.COD_CIUDADES,1,CHARINDEX('-', pg.COD_CIUDADES)-1) AS INT)
					JOIN API_CIUDADES AS d ON d.ID_CIUDADES = CAST(SUBSTRING(pg.COD_CIUDADES,CHARINDEX('-', pg.COD_CIUDADES)+1,3) AS INT)
					
					JOIN PRTM_DESTINO AS pd ON pd.ID_DESTINO = pg.ID_DESTINO 

					JOIN ADTM_OFICINA AS ofo ON ofo.ID_OFICINA = pd.ID_OFICINA_ORIGEN
					JOIN ADTM_OFICINA AS ofd ON ofd.ID_OFICINA = pd.ID_OFICINA_DESTINO

					JOIN ADTM_AGENCIA AS ago ON ago.ID_AGENCIA = ofo.ID_AGENCIA
					JOIN ADTM_AGENCIA AS agd ON agd.ID_AGENCIA = ofd.ID_AGENCIA

					WHERE pg.ID_PROGRAMACION_PADRE=@ID_PROGRAMACION_PADRE
					AND pg.ES_PROGRAMACION='A'
					ORDER BY 2

					";
            sCmd = String.Format(sCmd, idprogramacion);
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            //JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;

        }
        public string ListAsientos(int idprogramacion)
        {
            string sCmd = "";
            sCmd = @"
					DECLARE @idprogramacion INT={0}
					
					DECLARE 
						@ID_PROGRAMACION_PADRE INT
						SELECT @ID_PROGRAMACION_PADRE = CASE WHEN pp.IN_RUTA_PRINCIPAL=1 THEN pp.ID_PROGRAMACION ELSE pp.ID_PROGRAMACION_PADRE END FROM PRTM_PROGRAMACION AS pp WHERE pp.ID_PROGRAMACION IN(@idprogramacion)

					SELECT 
						asi.ID_PROGRAMACION AS ID_PROGRAMACION_PADRE,
						@idprogramacion AS ID_PROGRAMACION,
						asi.ID_ASIENTO_VENTA as ID_ASIENTO_VENTA,
						asi.NU_FILA as NU_FILA, 
						asi.NU_ASIENTO as NU_ASIENTO, 
						asi.NU_PISO as NU_PISO, 
						ea.CO_ESTADO_ASIENTO as CO_ESTADO_ASIENTO,
						asi.NO_TIPO_ASIENTO as NO_TIPO_ASIENTO,
						asi.NU_COLUMNA as NU_COLUMNA,
						asi.ID_CLASE_ASIENTO as ID_CLASE_ASIENTO, 
						cs.NO_CLASE_ASIENTO as NO_CLASE_ASIENTO,
						cap.VA_PRECIO
					FROM dbo.VTTR_ASIENTO_VENTA asi 
					JOIN ADTR_CLASE_ASIENTO_PROGRAMACION_PRECIO cap ON cap.ID_PROGRAMACION = @idprogramacion AND cap.ID_CLASE_ASIENTO = asi.ID_CLASE_ASIENTO and cap.CO_ESTADO='A'
					inner join dbo.VTTM_ESTADO_ASIENTO ea on asi.CO_ESTADO_ASIENTO=ea.CO_ESTADO_ASIENTO                    
					INNER JOIN dbo.ADTM_CLASE_ASIENTO cs on cs.ID_CLASE_ASIENTO = asi.ID_CLASE_ASIENTO 
					AND cs.CO_ESTADO = 'A'                   
					where ea.CO_ESTADO_ASIENTO not in ('ECC')
					AND asi.ID_PROGRAMACION = @ID_PROGRAMACION_PADRE
					order by asi.NU_FILA, asi.NU_PISO, asi.NU_COLUMNA

					";
            sCmd = String.Format(sCmd, idprogramacion);
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            //JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;

        }

        
		public string ReservarAsiento(int idprogramacion, int idasiento, int asiento)
        {
            string sCmd = "";
            sCmd = @"
					DECLARE
					@ID_PROGRAMACION INT={0},
					@ID_ASIENTO_VENTA INT={1},
					@NU_ASIENTO INT={2} 

				DECLARE
					@actualizado INT,
					@estadoasiento VARCHAR(10)
	
				BEGIN TRAN
					SELECT @estadoasiento = CO_ESTADO_ASIENTO
					FROM VTTR_ASIENTO_VENTA
					WHERE ID_PROGRAMACION=@ID_PROGRAMACION
						AND ID_ASIENTO_VENTA=@ID_ASIENTO_VENTA
						AND NU_ASIENTO=@NU_ASIENTO

					IF @estadoasiento IS NULL OR @estadoasiento<>'LIB'
					BEGIN
						SELECT 1 AS CodRespuesta, 'Asiento no disponible.' AS Mensaje
						ROLLBACK TRAN
						RETURN
					END
					ELSE
						BEGIN
							UPDATE VTTR_ASIENTO_VENTA
							SET CO_ESTADO_ASIENTO='BLQ',
								FE_MOD_REGISTRO= GETDATE(),
								ID_MOD_REGISTRO={3},
								MO_BLOCKEO='CO', 
								TIEMPO_BLOQUEO_API=20,
								ID_USUARIO_BLOQUEO={3},
								ID_PUNTO_VENTA_SEPARA={4}
							WHERE ID_PROGRAMACION=@ID_PROGRAMACION
								AND ID_ASIENTO_VENTA=@ID_ASIENTO_VENTA
								AND NU_ASIENTO=@NU_ASIENTO
								AND CO_ESTADO_ASIENTO='LIB'
	
							SELECT @actualizado=@@ROWCOUNT
						END
	
					COMMIT TRAN
					IF @actualizado=0
					BEGIN
						SELECT 1 AS CodRespuesta, 'Asiento no disponible...' AS Mensaje
						RETURN
					END	
					IF @actualizado>0
					BEGIN
						SELECT 0 AS CodRespuesta, 'Asiento Bloqueado correctamente' AS Mensaje
						RETURN
					END

					";
            sCmd = String.Format(sCmd, idprogramacion, idasiento, asiento, idkupos, idpuntoventa);
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;

        }

        public string ConfirmarVenta(RegistrarVentaQuery registro)
		{
            string sCmd = "";
			sCmd = @"
				DECLARE
					@ID_PUNTO_VENTA INT={0}, --validar cual va a ser para kupos
					@ID_PUNTO_VENTA_REGISTRO INT={1},
					@NU_BOLETO_VENTA INT ={2}, -- cantidad de asientos vendidos
					@VA_MONTO_TOTAL NUMERIC(18,4)={3},
					@NU_OPERACION VARCHAR(50)='{4}', --numero comprobante pago tarjeta
					@CO_TIPO_TARJETA VARCHAR(50) = '{5}', --tipo tarjeta MAStercard, VISa
					@ID_PROGRAMACION_IDA INT  = {6},
					@ID_PROGRAMACION_RETORNO INT  = {7},
					@NU_PEDIDO_SAFETY VARCHAR(50) = '{8}',
					@NU_REFERENCIA_SAFETY VARCHAR(50) = '{9}',
					@ID_PAGADOR2 VARCHAR(20)= '{10}',
					@ID_DESTINO INT = {11},
					@ID_CIUDAD_ORIGEN INT = {12},
					@ID_CIUDAD_DESTINO INT = {13}
				-------------------
				DECLARE
					@ID_PAGADOR INT,
					@ID_CABECERA_VENTA INT,
					@ID_VENTA INT,
	
					@fecharegistro DATETIME=GETDATE(),
					@CO_CANAL_VENTA VARCHAR(30)='ONL',
					@TIPO_PAGO VARCHAR(30)='CON',
					@ID_FORMA_PAGO VARCHAR(30)='TRJ'

				BEGIN TRY
					DECLARE @NU_PEDIDO INT
					BEGIN TRAN
						SELECT @NU_PEDIDO = VA_VALOR  
						FROM SGTX_PARAMETRO 
						WHERE DE_PARAMETRO = 'PARAM_NUM_PEDIDO_SAFETY'
	
						UPDATE SGTX_PARAMETRO
						SET VA_VALOR = VA_VALOR + 1 
						WHERE DE_PARAMETRO = 'PARAM_NUM_PEDIDO_SAFETY'
					COMMIT TRAN

					BEGIN TRAN
						IF @ID_PUNTO_VENTA=0
							SET @ID_PUNTO_VENTA = NULL

						IF @ID_PUNTO_VENTA_REGISTRO=0
							SET @ID_PUNTO_VENTA_REGISTRO = NULL
						--Registro venta boleto (cabecera) ---------------------------------
						INSERT INTO VTTC_VENTA (
							ID_AREA, 
							ID_PUNTO_VENTA, 
							FE_CREA_REGISTRO, 
							NU_BOLETOS, 
							TIPO_PAGO, 
							VA_MONTO_TOTAL, 
							CO_CANAL_VENTA, 
							ID_FORMA_PAGO
						)VALUES (
							'PSJ',
							@ID_PUNTO_VENTA,
							@fecharegistro,
							@NU_BOLETO_VENTA,
							@TIPO_PAGO,
							@VA_MONTO_TOTAL,
							@CO_CANAL_VENTA,
							@ID_FORMA_PAGO --venta tarjeta
						)

						SELECT @ID_CABECERA_VENTA = @@IDENTITY 

						--------------------------------------------------------------
							DECLARE 
								@NU_RUC VARCHAR(11)='{14}',
								@DE_RAZONSOCIAL VARCHAR(250)='{15}',
								@DE_DIRECCION VARCHAR(250)='{16}',
								@ID_CREA_REGISTRO INT={17},
								@ID_MOD_REGISTRO INT={18},
								@ES_EMPRESA CHAR(1)='A'

								DECLARE 
									@ID_EMPRESA INT = NULL,
									@CO_TIPO_COMPROBANTE VARCHAR(20) = 'BOL'
								IF @NU_RUC<>''
								BEGIN
									SELECT @CO_TIPO_COMPROBANTE='FAC'

									IF EXISTS(SELECT 1 FROM ADTM_EMPRESA WHERE NU_RUC=@NU_RUC)
									BEGIN
										SELECT @ID_EMPRESA=ID_EMPRESA FROM ADTM_EMPRESA WHERE NU_RUC=@NU_RUC
										UPDATE ADTM_EMPRESA
										SET DE_DIRECCION = @DE_DIRECCION,
											ID_MOD_REGISTRO=@ID_MOD_REGISTRO,
											FE_MOD_REGISTRO=GETDATE()
										WHERE ID_EMPRESA=@ID_EMPRESA
		
									END
									ELSE
									BEGIN
									  INSERT INTO ADTM_EMPRESA
									  ([NU_RUC],[DE_RAZONSOCIAL],[DE_DIRECCION],[FE_CREA_REGISTRO],[ID_CREA_REGISTRO],[ID_MOD_REGISTRO],[ES_EMPRESA])
									  VALUES(
											  @NU_RUC,
											  @DE_RAZONSOCIAL,
											  @DE_DIRECCION,
											  GETDATE(),
											  @ID_CREA_REGISTRO,
											  @ID_MOD_REGISTRO,
											  @ES_EMPRESA
										  )

									  SELECT @ID_EMPRESA=@@IDENTITY
									END
								END
						-------------------------------------------------------------------
						--Crear persona si no existe -------------------------------------
                        DECLARE 
							@ID_TIPO_DOCUMENTO INT = {19},
							@NU_DOCUMENTO VARCHAR(20) = '{20}',
							@NO_PERSONA VARCHAR(100) = '{21}',
							@AP_PERSONA VARCHAR(100) = '{22}',
							@AM_PERSONA VARCHAR(100) = '{23}',
							@ID_PERSONA INT = NULL
							
							IF EXISTS(SELECT 1 FROM ADTM_PERSONA AS ap WHERE NU_DOCUMENTO=@NU_DOCUMENTO AND ID_TIPO_DOCUMENTO=@ID_TIPO_DOCUMENTO)
							BEGIN
								SELECT @ID_PERSONA=ap.ID_PERSONA FROM ADTM_PERSONA AS ap WHERE NU_DOCUMENTO=@NU_DOCUMENTO AND ID_TIPO_DOCUMENTO=@ID_TIPO_DOCUMENTO
							END
							ELSE
							BEGIN
								INSERT INTO ADTM_PERSONA (NO_PERSONA,AP_PERSONA,AM_PERSONA,ID_TIPO_DOCUMENTO,NU_DOCUMENTO,DE_SEXO,FE_CREA_REGISTRO,ID_CREA_REGISTRO,FE_MOD_REGISTRO,ID_MOD_REGISTRO)
								VALUES (@NO_PERSONA,@AP_PERSONA,@AM_PERSONA,@ID_TIPO_DOCUMENTO,@NU_DOCUMENTO,'M', GETDATE(),@ID_CREA_REGISTRO,GETDATE(),@ID_CREA_REGISTRO)
    
								SELECT @ID_PERSONA=@@IDENTITY
								
								INSERT INTO VTTM_PASAJERO (ID_PERSONA,FE_CREA_REGISTRO,ID_CREA_REGISTRO)
								VALUES (@ID_PERSONA,GETDATE(),@ID_CREA_REGISTRO)
			
							END
							
						SELECT @ID_PAGADOR = @ID_PERSONA
						-------------------------------------------------------------------
						--Registro venta boleto (cabecera relacionada)---------------------
						INSERT INTO VTTM_VENTA (
							ID_PUNTO_VENTA, 
							ID_PUNTO_VENTA_REGISTRO,
							CO_TIPO_VENTA, 
							FE_VENTA, 
							IN_COBRO_ASUMIDO_X_EMPRESA, 
							IN_MULTIPLE, 
							IN_VENTA_MULTIPLE, 
							NU_BOLETO_VENTA, 
							ID_CABECERA_VENTA, 
							CO_CANAL_VENTA,
							ID_EMPRESA,
							ID_PERSONA,
							VA_TOTAL,
							VA_PRECIO_COBRO
						) VALUES (
							@ID_PUNTO_VENTA,
							@ID_PUNTO_VENTA_REGISTRO,
							'DIR',
							@fecharegistro,
							0,
							1,
							1,
							@NU_BOLETO_VENTA, 
							@ID_CABECERA_VENTA,
							@CO_CANAL_VENTA,
							@ID_EMPRESA,
							@ID_PERSONA,
							@VA_MONTO_TOTAL,
							@VA_MONTO_TOTAL		
						)

						SELECT @ID_VENTA = @@IDENTITY

						
						----Registro pagos comprobante -----------------------
						--insert into VTTM_PAGOS (
						--	FE_CREA_REGISTRO, 
						--	MONTO, 
						--	NU_OPERACION, 
						--	ID_CABECERA_VENTA, 
						--	ID_FORMA_PAGO, 
						--	CO_TIPO_TARJETA) 
						--values (
						--	@fecharegistro,
						--	@VA_MONTO_TOTAL,
						--	@NU_OPERACION,
						--	@ID_CABECERA_VENTA,
						--	@ID_FORMA_PAGO,
						--	@CO_TIPO_TARJETA
						--)
						

						--Registro Pedido Online Safety -----------------------

						
						DECLARE @DE_DESTINO_ITINERARIO VARCHAR(100)
						DECLARE @DE_ORIGEN_ITINERARIO VARCHAR(100)


						SELECT @DE_DESTINO_ITINERARIO=ac.NOMBRE  FROM API_CIUDADES AS ac WHERE ac.ID_CIUDADES=@ID_CIUDAD_DESTINO
						SELECT @DE_ORIGEN_ITINERARIO=ac.NOMBRE  FROM API_CIUDADES AS ac WHERE ac.ID_CIUDADES=@ID_CIUDAD_ORIGEN
						
						DECLARE
							@ID_OFICINA_ORIGEN INT,
							@ID_OFICINA_DESTINO INT,
							@ID_AGENCIA_ORIGEN INT,
							@ID_AGENCIA_DESTINO INT

						SELECT @ID_OFICINA_ORIGEN=pd.ID_OFICINA_ORIGEN,@ID_OFICINA_DESTINO=pd.ID_OFICINA_DESTINO FROM PRTM_DESTINO AS pd WHERE pd.ID_DESTINO= @ID_DESTINO
	
						SELECT @ID_AGENCIA_ORIGEN=ao.ID_AGENCIA FROM ADTM_OFICINA AS ao WHERE ao.ID_OFICINA=@ID_OFICINA_ORIGEN
						SELECT @ID_AGENCIA_DESTINO=ao.ID_AGENCIA FROM ADTM_OFICINA AS ao WHERE ao.ID_OFICINA=@ID_OFICINA_DESTINO
	
						SELECT @DE_ORIGEN_ITINERARIO=aa.NO_AGENCIA FROM ADTM_AGENCIA AS aa WHERE aa.ID_AGENCIA=@ID_AGENCIA_ORIGEN
						SELECT @DE_DESTINO_ITINERARIO=aa.NO_AGENCIA FROM ADTM_AGENCIA AS aa WHERE aa.ID_AGENCIA=@ID_AGENCIA_DESTINO

						DECLARE @ID_PEDIDO INT

						INSERT INTO VTTM_PEDIDO_ONLINE
						(
							FE_PEDIDO,
							CO_ERROR, 
							CO_ESTADO,
							CO_TIPO_PEDIDO, 
							NU_PEDIDO,
							FE_EXPIRACION, 
							VA_PAGO, 
							CO_MONEDA,
							ID_VENTA,
							FE_PAGO, 
							NU_PEDIDO_SAFETY, 
							NU_REFERENCIA_SAFETY, 
							ID_PROGRAMACION_IDA,
							ID_PROGRAMACION_RETORNO,
							ID_PAGADOR,
							DE_ORIGEN_ITINERARIO,
							DE_DESTINO_ITINERARIO
						)
						VALUES(
							@fecharegistro,
							0,
							'PAC',
							'KUPOS',
							@NU_PEDIDO,
							DATEADD(hh,2,@fecharegistro),
							@VA_MONTO_TOTAL,
							'PEN',
							@ID_VENTA,
							@fecharegistro,
							@NU_PEDIDO_SAFETY,
							@NU_REFERENCIA_SAFETY,
							@ID_PROGRAMACION_IDA,
							@ID_PROGRAMACION_RETORNO,
							@ID_PAGADOR,
							@DE_ORIGEN_ITINERARIO,
							@DE_DESTINO_ITINERARIO
						)

						SELECT @ID_PEDIDO = @@IDENTITY

						
								

					COMMIT TRAN
					SELECT 0 AS CodRespuesta, @ID_CABECERA_VENTA AS ID_CABECERA_VENTA, @ID_VENTA AS ID_VENTA, @ID_PEDIDO AS ID_PEDIDO, @ID_PERSONA AS ID_PERSONA_REMITENTE,  'Venta Registrada correctamente' AS Mensaje
				END TRY 
					BEGIN CATCH
						SELECT @@ERROR AS CodRespuesta, 0 AS ID_CABECERA_VENTA , 0 AS ID_VENTA, 0 AS ID_PEDIDO,0 AS ID_PERSONA_REMITENTE,  ERROR_MESSAGE() AS Mensaje
						ROLLBACK TRAN
		
					END CATCH
					";
            sCmd = String.Format(sCmd, 
				registro.id_punto_venta, 
				registro.id_punto_venta_registro, 
				registro.nu_boleto_venta, 
				registro.va_monto_total.ToString().Replace(",", "."), 
				registro.nu_operacion, 
				registro.co_tipo_tarjeta, 
				registro.id_programacion_ida,
				string.Format("{0}", registro.id_programacion_retorno == null  ? "NULL" : registro.id_programacion_retorno.ToString()),
				registro.nu_pedido_safety, 
				registro.nu_referencia_safety,
                string.Format("{0}", registro.id_pagador == null ? "NULL" : registro.id_pagador.ToString()), 
				registro.id_destino,
				registro.id_ciudad_origen,
                registro.id_ciudad_destino,
				registro.nu_ruc,
				registro.no_razon_social,
				registro.de_direccion,
				registro.id_usuario,
				registro.id_usuario,
				registro.id_tipo_documento,
				registro.nu_documento,
				registro.am_pasajero,
				registro.ap_pasajero,
				registro.no_pasajero
                );
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;
        }


        public string RegistrarBoleto(RegistraBoletoQuery registro)
        {
            string sCmd = "";
            sCmd = @"
				
					SET LANGUAGE Spanish

					DECLARE
						@ID_VENTA INT={0},
						@ID_ASIENTO_VENTA INT={1},
						@VA_VENTA NUMERIC(18,4)={2},
						@CO_TIPO_TARJETA VARCHAR(10)='{3}',
						@NUMERO_OPERACION VARCHAR(100)='{4}', --numero comprobante pago tarjeta
						@AM_PASAJERO VARCHAR(100)='{5}', --apellido materno
						@AP_PASAJERO VARCHAR(100)='{6}', --apellido paterno
						@NO_PASAJERO VARCHAR(100)='{7}', --nombre pasajero
						@NU_DOCUMENTO VARCHAR(100)='{8}', --identidicador del pasajero DNI, pasaporte... 
						@ID_PASAJERO INT={9},
						@ID_DESTINO INT={10},
						@ID_CIUDAD_ORIGEN INT={11},
						@ID_CIUDAD_DESTINO INT={12},
						@ID_USUARIO INT = {13},
						@DE_TOTAL_LETRAS VARCHAR(200)='{14}',
						@NO_OFICINA_VENTA VARCHAR(100)='{15}', --nombre oficina de venta
						@NU_ASIENTO INT={16},
						@NU_IP_VENTA VARCHAR(100)='{17}',
						@ID_PROGRAMACION INT={18},
						@ID_PEDIDO INT ={26},
						@MO_ASIENTO VARCHAR(10) = '{27}'
	
					BEGIN TRY
						BEGIN TRAN	
							DECLARE
								@fecharegistro DATETIME=GETDATE(),
								@DE_DESTINO VARCHAR(100),
								@DE_ORIGEN VARCHAR(100),
								@DE_MES_DOCUMENTO VARCHAR(100), 
								@TIPO_PAGO VARCHAR(30)='CON',
								--@ID_TIPO_DOCUMENTO INT=3,
								@CO_ESTADO_ASIENTO_ACTUAL VARCHAR(50)
			
								SELECT @DE_MES_DOCUMENTO = UPPER(FORMAT(@fecharegistro, N'MMMM', 'es-ES'))
								SELECT @DE_DESTINO=ac.NOMBRE  FROM API_CIUDADES AS ac WHERE ac.ID_CIUDADES=@ID_CIUDAD_DESTINO
								SELECT @DE_ORIGEN=ac.NOMBRE  FROM API_CIUDADES AS ac WHERE ac.ID_CIUDADES=@ID_CIUDAD_ORIGEN
	                            
                                DECLARE
									@ID_OFICINA_ORIGEN INT,
									@ID_OFICINA_DESTINO INT,
									@ID_AGENCIA_ORIGEN INT,
									@ID_AGENCIA_DESTINO INT

								SELECT @ID_OFICINA_ORIGEN=pd.ID_OFICINA_ORIGEN,@ID_OFICINA_DESTINO=pd.ID_OFICINA_DESTINO FROM PRTM_DESTINO AS pd WHERE pd.ID_DESTINO= @ID_DESTINO
	
								SELECT @ID_AGENCIA_ORIGEN=ao.ID_AGENCIA FROM ADTM_OFICINA AS ao WHERE ao.ID_OFICINA=@ID_OFICINA_ORIGEN
								SELECT @ID_AGENCIA_DESTINO=ao.ID_AGENCIA FROM ADTM_OFICINA AS ao WHERE ao.ID_OFICINA=@ID_OFICINA_DESTINO
	
								SELECT @DE_ORIGEN=aa.NO_AGENCIA FROM ADTM_AGENCIA AS aa WHERE aa.ID_AGENCIA=@ID_AGENCIA_ORIGEN
								SELECT @DE_DESTINO=aa.NO_AGENCIA FROM ADTM_AGENCIA AS aa WHERE aa.ID_AGENCIA=@ID_AGENCIA_DESTINO
								
								DECLARE
									--@ID_PROGRAMACION INT,
									@FE_SALIDA_PLANIFICADA VARCHAR(100),
									@DE_HORA_SALIDA VARCHAR(50),
									@DE_AM_PM VARCHAR(10)
				
								SELECT @CO_ESTADO_ASIENTO_ACTUAL = vav.CO_ESTADO_ASIENTO FROM VTTR_ASIENTO_VENTA AS vav WHERE vav.ID_ASIENTO_VENTA=@ID_ASIENTO_VENTA
			
								--SELECT @ID_PROGRAMACION, @CO_ESTADO_ASIENTO_ACTUAL
			
								IF ISNULL(@CO_ESTADO_ASIENTO_ACTUAL,'XXXX') IN('VEN','XXXX')
								BEGIN
									SELECT 1 AS CodRespuesta, 0 AS ID_COMPROBANTE_PAGO,0 AS ID_DETALLE,0 AS ID_BOLETO,  'Asiento ya no esta disponible' AS Mensaje
									ROLLBACK TRAN
									RETURN
								END
								IF ISNULL(@ID_PROGRAMACION,0) IN(0)
								BEGIN
									SELECT 2 AS CodRespuesta, 0 AS ID_COMPROBANTE_PAGO,0 AS ID_DETALLE,0 AS ID_BOLETO,  'Programacion ya no esta disponible' AS Mensaje
									ROLLBACK TRAN
									RETURN
								END
			
								SELECT	@FE_SALIDA_PLANIFICADA= FE_SALIDA_PLANIFICADA,
										@DE_HORA_SALIDA=DE_HORA_SALIDA,
										@DE_AM_PM=DE_AM_PM 
								FROM PRTM_PROGRAMACION AS pp WHERE pp.ID_PROGRAMACION=@ID_PROGRAMACION
			
			
							--------------------------------------------------------------
							DECLARE 
								@NU_RUC VARCHAR(11)='{19}',
								@DE_RAZONSOCIAL VARCHAR(250)='{20}',
								@DE_DIRECCION VARCHAR(250)='{21}',
								@ID_CREA_REGISTRO INT={22},
								@ID_MOD_REGISTRO INT={23},
								@ES_EMPRESA CHAR(1)='A'

								DECLARE 
									@ID_EMPRESA INT = NULL,
									@CO_TIPO_COMPROBANTE VARCHAR(20) = 'BOL'
								IF @NU_RUC<>''
								BEGIN
									SELECT @CO_TIPO_COMPROBANTE='FAC'

									IF EXISTS(SELECT 1 FROM ADTM_EMPRESA WHERE NU_RUC=@NU_RUC)
									BEGIN
										SELECT @ID_EMPRESA=ID_EMPRESA FROM ADTM_EMPRESA WHERE NU_RUC=@NU_RUC
										UPDATE ADTM_EMPRESA
										SET DE_DIRECCION = @DE_DIRECCION,
											ID_MOD_REGISTRO=@ID_MOD_REGISTRO,
											FE_MOD_REGISTRO=GETDATE()
										WHERE ID_EMPRESA=@ID_EMPRESA
		
									END
									ELSE
									BEGIN
									  INSERT INTO ADTM_EMPRESA
									  ([NU_RUC],[DE_RAZONSOCIAL],[DE_DIRECCION],[FE_CREA_REGISTRO],[ID_CREA_REGISTRO],[ID_MOD_REGISTRO],[ES_EMPRESA])
									  VALUES(
											  @NU_RUC,
											  @DE_RAZONSOCIAL,
											  @DE_DIRECCION,
											  GETDATE(),
											  @ID_CREA_REGISTRO,
											  @ID_MOD_REGISTRO,
											  @ES_EMPRESA
										  )

									  SELECT @ID_EMPRESA=@@IDENTITY
								  END
							END
							--------------------------------------------------------------		
							--Crear persona si no existe -------------------------------------
							DECLARE 
								@ID_TIPO_DOCUMENTO INT = {24},
  								@ID_PERSONA_REMITENTE INT = {25},
								@ID_PERSONA INT = NULL
								--@ID_PASAJERO INT = NULL
							
								IF EXISTS(SELECT 1 FROM ADTM_PERSONA AS ap WHERE NU_DOCUMENTO=@NU_DOCUMENTO AND ID_TIPO_DOCUMENTO=@ID_TIPO_DOCUMENTO)
								BEGIN
									SELECT @ID_PERSONA=ap.ID_PERSONA FROM ADTM_PERSONA AS ap WHERE NU_DOCUMENTO=@NU_DOCUMENTO AND ID_TIPO_DOCUMENTO=@ID_TIPO_DOCUMENTO
									SELECT @ID_PASAJERO=ID_PASAJERO FROM VTTM_PASAJERO WHERE ID_PERSONA=@ID_PERSONA
								END
								ELSE
								BEGIN
									INSERT INTO ADTM_PERSONA (NO_PERSONA,AP_PERSONA,AM_PERSONA,ID_TIPO_DOCUMENTO,NU_DOCUMENTO,DE_SEXO,FE_CREA_REGISTRO,ID_CREA_REGISTRO,FE_MOD_REGISTRO,ID_MOD_REGISTRO)
									VALUES (@NO_PASAJERO,@AP_PASAJERO,@AM_PASAJERO,@ID_TIPO_DOCUMENTO,@NU_DOCUMENTO,'M', GETDATE(),@ID_CREA_REGISTRO,GETDATE(),@ID_CREA_REGISTRO)
    
									SELECT @ID_PERSONA=@@IDENTITY
								
									INSERT INTO VTTM_PASAJERO (ID_PERSONA,FE_CREA_REGISTRO,ID_CREA_REGISTRO)
									VALUES (@ID_PERSONA,GETDATE(),@ID_CREA_REGISTRO)
									
									SELECT @ID_PASAJERO=@@IDENTITY	
								END
							
							IF ISNULL(@ID_PASAJERO,0) IN(0)
							BEGIN
								INSERT INTO VTTM_PASAJERO (ID_PERSONA,FE_CREA_REGISTRO,ID_CREA_REGISTRO)
								VALUES (@ID_PERSONA,GETDATE(),@ID_CREA_REGISTRO)
									
								SELECT @ID_PASAJERO=@@IDENTITY
							END	
							-------------------------------------------------------------------
							DECLARE
								@ID_COMPROBANTE_PAGO INT

							--Registrar comprobante de pago C ----------------------------
							INSERT INTO VTTC_COMPROBANTE_PAGO (ID_EMPRESA,
								DE_DESTINO, 	DE_MES_DOCUMENTO,	DE_ORIGEN,	DE_TOTAL_LETRAS,	FE_CREA_REGISTRO,	FE_DOCUMENTO, 
								ID_CREA_REGISTRO, 	IN_CONSIGNATARIO,IN_MIGRADO_ERP,	IN_REMITENTE,	NU_ANIO_DOCUMENTO,	NU_CORRELATIVO,	NU_DIA_DOCUMENTO, 
								NU_DOCUMENTO, NU_MES_DOCUMENTO,	NU_SERIE,TIPO_AFECTACION,	TIPO_PAGO,	VA_DESCUENTO,	VA_IGV,	VA_IGV_PORCENTAJE,	VA_PRECIO_COBRO, 
								VA_TOTAL, VA_VENTA, VA_VENTA_EFECTIVO,	VA_VENTA_TARJETA,	CO_ESTADO_COMPROBANTE,	CO_TIPO_COMPROBANTE,ID_VENTA,IS_VENTA_ONLINE,CO_TIPO_PEDIDO
								,ID_PERSONA_REMITENTE,ID_EMPRESA_REMITENTE
							) 
							VALUES (@ID_EMPRESA,
							@DE_DESTINO,@DE_MES_DOCUMENTO,@DE_ORIGEN,@DE_TOTAL_LETRAS,@fecharegistro,@fecharegistro,
							@ID_USUARIO,0,'N',0,CONVERT(VARCHAR(10), YEAR(@fecharegistro)),	
							NULL, --Es el correlativo o folio del boleto, enviar nulo y la tabla autotenera un folio		
							CONVERT(VARCHAR(10), DAY(@fecharegistro)),
							@NU_DOCUMENTO,
							CONVERT(VARCHAR(10), MONTH(@fecharegistro)),'1',	'EXONERADA',@TIPO_PAGO,	0,0,0.18,@VA_VENTA,
							@VA_VENTA,@VA_VENTA,0,0,'ACT',@CO_TIPO_COMPROBANTE,@ID_VENTA,1,NULL,@ID_PERSONA_REMITENTE,@ID_EMPRESA
							)
							SELECT @ID_COMPROBANTE_PAGO = @@IDENTITY
		
							DECLARE 
								@NU_SERIE VARCHAR(10),
								@NU_CORRELATIVO INT 

							SELECT @NU_SERIE=NU_SERIE, @NU_CORRELATIVO=NU_CORRELATIVO FROM VTTC_COMPROBANTE_PAGO WHERE ID_COMPROBANTE_PAGO = @ID_COMPROBANTE_PAGO

							DECLARE @ID_DETALLE INT
							--Registrar comprobante de pago D ---------------------------
							insert into VTTD_COMPROBANTE_PAGO (
								DE_DETALLE, 
								FE_CREA_REGISTRO, 
								ID_CREA_REGISTRO, 
								NU_CANTIDAD, 
								VA_UNITARIO, 
								VA_VENTA, 
								ID_COMPROBANTE_PAGO, 
								ID_ASIENTO_VENTA
							) 
							values (
								'Boleto',
								@fecharegistro,
								@ID_USUARIO,
								1,
								@VA_VENTA,
								@VA_VENTA,
								@ID_COMPROBANTE_PAGO,
								@ID_ASIENTO_VENTA
							)
		
							SELECT @ID_DETALLE=@@IDENTITY
		
							--Registrar Boleto ------------------------
							DECLARE 
								@ID_BOLETO INT
		
							INSERT INTO VTTC_BOLETO (
								ID_TIPO_DOCUMENTO, 
								AM_PASAJERO, 
								AM_RESPONSABLE, 
								AP_PASAJERO, 
								AP_RESPONSABLE, 
								DE_AM_PM, 
								DE_PRECIO_TOTAL, 
								DE_SEXO, 
								FE_CREA_REGISTRO, 
								FE_EMISION, 
								FE_VIAJE, 
								HR_VIAJE, 
								ID_CREA_REGISTRO, 
								IN_ABORDO, IN_DESCUENTO, IN_DESCUENTO_SOL_PREPAGADO,  IN_QUEDADO, 
								IS_BOLETO_MIGRADO,  MOTIVO_DESCUENTO, NO_DESTINO, NO_OFICINA_VENTA, NO_ORIGEN, NO_PASAJERO, NO_RESPONSABLE, 
								NU_ASIENTO, NU_CORRELATIVO, 
								NU_DOCUMENTO, NU_IP_VENTA,  NU_SERIE, 
								PRECIO_ORIGINAL, ID_DESTINO, 
								TIPO_PAGO, VA_DESCUENTO, VA_PRECIO_COBRO, VA_PRECIO_TOTAL, ID_DETALLE, 
								CO_ESTADO_BOLETO, ID_PASAJERO,ID_EMPRESA) 
							values (
								@ID_TIPO_DOCUMENTO,@AM_PASAJERO,'KUPOS',@AP_PASAJERO,'KUPOS',@DE_AM_PM,@DE_TOTAL_LETRAS,'',@fecharegistro,
								@fecharegistro,@FE_SALIDA_PLANIFICADA,@DE_HORA_SALIDA,@ID_USUARIO,0,0,0,1,0,'',@DE_DESTINO,@NO_OFICINA_VENTA,@DE_ORIGEN,@NO_PASAJERO,'KUPOS',
								@NU_ASIENTO,@NU_CORRELATIVO,
								@NU_DOCUMENTO,@NU_IP_VENTA,@NU_SERIE,@VA_VENTA,@ID_DESTINO,@TIPO_PAGO,0,@VA_VENTA,@VA_VENTA,
								@ID_DETALLE,'NEM',@ID_PASAJERO,@ID_EMPRESA
							)
		
							SELECT @ID_BOLETO=@@IDENTITY
		
							--
							--INSERT INTO VTTC_PAGOS_COMPROBANTE (
							--	MONTO_PAGADO, 
							--	NUMERO_OPERACION, 
							--	ID_COMPROBANTE_PAGO, 
							--	ID_FORMA_PAGO, 
							--	CO_TIPO_TARJETA
							--) 
							--VALUES (
							--	@VA_VENTA,@NUMERO_OPERACION,@ID_COMPROBANTE_PAGO,'TRJ',@CO_TIPO_TARJETA	
							--) 
			
									

							--Actualizar estado del boleto
							UPDATE VTTR_ASIENTO_VENTA 
							SET FE_MOD_REGISTRO=@fecharegistro, 
								TIEMPO_BLOQUEO_API=NULL, 
								VA_DESCUENTO=0, 
								VA_PRECIO=0, 
								VA_PRECIO_VENTA=@VA_VENTA, 
								CO_ESTADO_ASIENTO='VEN', 
								ID_PASAJERO=@ID_PASAJERO,
								--ID_PROGRAMACION=@ID_PROGRAMACION, 
								ID_PROGRAMACION_VENTA=@ID_PROGRAMACION
							WHERE ID_ASIENTO_VENTA=@ID_ASIENTO_VENTA
					
					    -----------------------------------------------------------
							IF ISNULL(@ID_PEDIDO,0)>0
							BEGIN
								INSERT INTO [dbo].[VTTR_PEDIDO_ASIENTO]
									   ([ID_PEDIDO]
									   ,[ID_ASIENTO]
									   ,[MO_ASIENTO]
									   ,[ID_PASAJERO_INICIAL]
									   ,[ID_PROGRAMACION_VENTA]
									   ,[VA_PRECIO_VENTA]
									   ,[FE_CREA_REGISTRO]
									   ,[FE_MOD_REGISTRO]
									   ,[ID_EMPRESA]
									   ,[ID_DECLARACION_JURADA])
								 VALUES (
										@ID_PEDIDO,
										@ID_ASIENTO_VENTA,
										@MO_ASIENTO,
										@ID_PASAJERO,
										@ID_PROGRAMACION,
										@VA_VENTA,
										@fecharegistro,
										NULL,
										@ID_EMPRESA,
										NULL
										)
							END
						COMMIT TRAN
						SELECT 0 AS CodRespuesta,@ID_COMPROBANTE_PAGO AS ID_COMPROBANTE_PAGO,@ID_DETALLE AS ID_DETALLE,@ID_BOLETO AS ID_BOLETO, 'Boleto Registrado correctamente' AS Mensaje
					END TRY 
						BEGIN CATCH
							SELECT CASE WHEN ISNULL(@@ERROR,0)=0 THEN 3 ELSE ISNULL(@@ERROR,0) END  AS CodRespuesta, 0 AS ID_COMPROBANTE_PAGO,0 AS ID_DETALLE,0 AS ID_BOLETO,  ERROR_MESSAGE() AS Mensaje
							ROLLBACK TRAN
		
						END CATCH
	
					";
            sCmd = String.Format(sCmd, 
				registro.id_venta, 
				registro.id_asiento_venta, 
				registro.va_venta.ToString().Replace(",", "."),
                registro.co_tipo_tarjeta,
                registro.numero_operacion,
                registro.am_pasajero,
				registro.ap_pasajero, 
				registro.no_pasajero,
				registro.nu_documento,
                registro.id_pasajero == null ? "NULL" : registro.id_pasajero.ToString(), 
				registro.id_destino, 
				registro.id_ciudad_origen,
                registro.id_ciudad_destino,
                registro.id_usuario, 
				registro.de_total_letras, 
				registro.no_oficina_venta,
				registro.nu_asiento, 
				registro.nu_ip_venta,
				registro.id_programacion,
				registro.nu_ruc,
				registro.no_razon_social,
				registro.de_direccion,
				registro.id_usuario,
				registro.id_usuario,
                registro.id_tipo_documento,
				registro.id_persona_remitente,
				registro.id_pedido,
				registro.mo_asiento
                );
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;
        }

		public string RegistrarTracking(RegistraTrackingQuery registro)
		{
			string sCmd = "";
			sCmd = @"
				SET LANGUAGE Spanish

				DECLARE 
					@ID_ASIENTO_VENTA INT={0},
					@NU_ASIENTO INT = {1},
					@ID_USUARIO INT = {2},
					@ID_BOLETO INT = {3},
					@VA_VENTA NUMERIC(18,4)={4},
					@NUMERO_OPERACION VARCHAR(100)='{5}', --numero comprobante pago tarjeta
					@AM_PASAJERO VARCHAR(100)='{6}', --apellido materno
					@AP_PASAJERO VARCHAR(100)='{7}', --apellido paterno
					@NO_PASAJERO VARCHAR(100)='{8}', --nombre pasajero
					@ID_DESTINO INT={9},
					@ID_CIUDAD_ORIGEN INT={10},
					@ID_CIUDAD_DESTINO INT={11},
					@ID_PROGRAMACION INT={12}
	
				BEGIN TRY
					BEGIN TRAN
						DECLARE
							@fecharegistro DATETIME=GETDATE(),
							--@ID_PROGRAMACION INT,
							@FE_SALIDA_PLANIFICADA VARCHAR(100),
							@DE_HORA_SALIDA VARCHAR(50),
							@DE_AM_PM VARCHAR(10),
							@DE_DESTINO VARCHAR(100),
							@DE_ORIGEN VARCHAR(100)
		
						--SELECT @ID_PROGRAMACION = vav.ID_PROGRAMACION  FROM VTTR_ASIENTO_VENTA AS vav WHERE vav.ID_ASIENTO_VENTA=@ID_ASIENTO_VENTA	
						SELECT @DE_DESTINO=ac.NOMBRE  FROM API_CIUDADES AS ac WHERE ac.ID_CIUDADES=@ID_CIUDAD_DESTINO
						SELECT @DE_ORIGEN=ac.NOMBRE  FROM API_CIUDADES AS ac WHERE ac.ID_CIUDADES=@ID_CIUDAD_ORIGEN
		
						------------------------------------------------------------------------------------------------------------
						DECLARE
							@ID_OFICINA_ORIGEN INT,
							@ID_OFICINA_DESTINO INT,
							@ID_AGENCIA_ORIGEN INT,
							@ID_AGENCIA_DESTINO INT

						SELECT @ID_OFICINA_ORIGEN=pd.ID_OFICINA_ORIGEN,@ID_OFICINA_DESTINO=pd.ID_OFICINA_DESTINO FROM PRTM_DESTINO AS pd WHERE pd.ID_DESTINO= @ID_DESTINO
	
						SELECT @ID_AGENCIA_ORIGEN=ao.ID_AGENCIA FROM ADTM_OFICINA AS ao WHERE ao.ID_OFICINA=@ID_OFICINA_ORIGEN
						SELECT @ID_AGENCIA_DESTINO=ao.ID_AGENCIA FROM ADTM_OFICINA AS ao WHERE ao.ID_OFICINA=@ID_OFICINA_DESTINO
	
						SELECT @DE_ORIGEN=aa.NO_AGENCIA FROM ADTM_AGENCIA AS aa WHERE aa.ID_AGENCIA=@ID_AGENCIA_ORIGEN
						SELECT @DE_DESTINO=aa.NO_AGENCIA FROM ADTM_AGENCIA AS aa WHERE aa.ID_AGENCIA=@ID_AGENCIA_DESTINO
						------------------------------------------------------------------------------------------------------------
						SELECT	@FE_SALIDA_PLANIFICADA= FE_SALIDA_PLANIFICADA,
								@DE_HORA_SALIDA=DE_HORA_SALIDA,
								@DE_AM_PM=DE_AM_PM 
						FROM PRTM_PROGRAMACION AS pp WHERE pp.ID_PROGRAMACION=@ID_PROGRAMACION
								
						----------------------------------------------			
						DECLARE @ID_LOG_AUDITORIA INT

						INSERT INTO SGTM_LOG_AUDITORIA (
							DE_ACCION, 
							DE_DESCRIPCION, 
							FE_HORAREGISTRO, 
							FE_REGISTRO, 
							NU_IP, 
							ID_USUARIO
						) 
						VALUES (
							'INSERT',
							NULL,
							@fecharegistro,
							@fecharegistro,
							'',
							@ID_USUARIO
						)

						SELECT @ID_LOG_AUDITORIA = @@IDENTITY
						----------------------------------------------------
						DECLARE @ID_TRACKING_BOLETO INT
						
						SELECT @NUMERO_OPERACION = vb.NU_DOCUMENTO  FROM VTTC_BOLETO AS vb WHERE vb.ID_BOLETO=@ID_BOLETO

						INSERT INTO VTTM_TRACKING_BOLETO (
							AM_PASAJERO, 
							AP_PASAJERO, 
							DE_AM_PM, 
							DE_DESCRIPCION, 
							FE_CREA_REGISTRO, 
							FE_VIAJE, 
							HR_VIAJE, 
							ID_CREA_REGISTRO, 
							NO_DESTINO, 
							NO_ORIGEN, 
							NO_PASAJERO, 
							NU_ASIENTO, 
							NU_DOCUMENTO, 
							VA_PRECIO_TOTAL, 
							ID_BOLETO, 
							CO_ESTADO_BOLETO
						) 
						VALUES (@AM_PASAJERO,@AP_PASAJERO,@DE_AM_PM,'Boleto',@fecharegistro,@FE_SALIDA_PLANIFICADA,@DE_HORA_SALIDA,@ID_USUARIO,
						@DE_DESTINO,@DE_ORIGEN,@NO_PASAJERO,@NU_ASIENTO,
						@NUMERO_OPERACION,@VA_VENTA,@ID_BOLETO,'NEM'
						)

						SELECT @ID_TRACKING_BOLETO = @@IDENTITY
						----------------------------------------------------
						DECLARE @ID_TRACKING_ASIENTO INT	

						insert into VTTM_TRACKING_ASIENTO_VENTA (
							DE_ACCION, 
							DE_DESCRIPCION, 
							FE_CREA_REGISTRO, 
							ID_ASIENTO_VENTA
						) 
						values (
						'INSERT',
						CONCAT('Venta del asiento venta  (',@NU_ASIENTO,')'),
						@fecharegistro,
						@ID_ASIENTO_VENTA
						)

						SELECT @ID_TRACKING_ASIENTO = @@IDENTITY

						------------------------------
						COMMIT TRAN
						SELECT 0 AS CodRespuesta, @ID_LOG_AUDITORIA AS ID_LOG_AUDITORIA, @ID_TRACKING_BOLETO AS ID_TRACKING_BOLETO, @ID_TRACKING_ASIENTO AS ID_TRACKING_ASIENTO, 'Tracking Registrada correctamente' AS Mensaje
				END TRY 
					BEGIN CATCH
						SELECT @@ERROR AS CodRespuesta, 0 AS ID_LOG_AUDITORIA, 0 AS ID_TRACKING_BOLETO, 0 AS ID_TRACKING_ASIENTO, ERROR_MESSAGE() AS Mensaje
						ROLLBACK TRAN
		
					END CATCH		
			";

            sCmd = String.Format(sCmd,
                registro.id_asiento_venta,
				registro.nu_asiento,
				registro.id_usuario,
				registro.id_boleto,
				registro.va_venta.ToString().Replace(",", "."),
				registro.numero_operacion,
				registro.am_pasajero,
				registro.ap_pasajero,
				registro.no_pasajero,
				registro.id_destino,
				registro.id_ciudad_origen,
				registro.id_ciudad_destino,
                registro.id_programacion
                );
            var db = new cnnDatos();
            string sCnn = db.Database.Connection.ConnectionString;
            string JSONresult = Repo_Database.EjecutaCmd(sCnn, sCmd);
            JSONresult = JSONresult.Substring(1, JSONresult.Length - 2);
            return JSONresult;
        }
    }
}
