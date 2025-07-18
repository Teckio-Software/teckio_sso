
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using System.Transactions;

using System.Text.RegularExpressions;
using GuardarArchivos.DTO;
using System.IO;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Identity.Client;
using SistemaERP.BLL.Contrato.Utilidades;
using Log.clase;
using log4net;
using SSO.DTO;




namespace GuardarArchivos
{
    public class GuardarArchivosC
    {
        private readonly IConfiguration _Configuration;
        private readonly IArchivoRutaService _archivoRutaService;


        private static readonly ILog Log = Logs.GetLogger();

        public GuardarArchivosC(IConfiguration configuration
            , IArchivoRutaService archivorutaService

            )
        {

            _Configuration = configuration;
            _archivoRutaService = archivorutaService;

        }

        /// <summary>
        /// Guarda el archivo creado subcarpetas con el rfc de la empresa, año, mes, y dia
        /// Registra en la base de datos el nombre del archivo, la ruta, el pesoy si es borrado en la tabla tblArchivos
        /// </summary>
        /// <param name="files"> iFormfile a del archivo a guardar</param>
        /// <param name="Ruta"> ruta base donde se van a crear las subcarpetas</param>
        /// <param name="RFCEmpresa"> El RFC de la empresa asociada al archivo</param>
        /// <returns>El id del registro de los deatos del archivo en la base de datos </returns>
        public async Task<int> Post(IFormFile files, string Ruta, string RFCEmpresa, bool rutacustom = false)
        {
            try
            {
                Log.Info("inicia Proceso de guardado");
                var fecha = DateTime.Now;
                var mes = "";

                mes = fecha.ToString("MMMM", new System.Globalization.CultureInfo("es-ES"));
                var nombreArchivo = files.FileName;

                var rutaCompuesta = !rutacustom ? Path.Combine(Ruta, RFCEmpresa, fecha.Year.ToString(), mes, fecha.Day.ToString()): Ruta;

                if (!Directory.Exists(rutaCompuesta))
                {
                    try
                    {
                        Log.Info("Creado directorio" + rutaCompuesta);
                        Directory.CreateDirectory(rutaCompuesta);
                        Log.Info("Directorio Creado");
                    }
                    catch (Exception es)
                    {
                        Log.Error(es.ToString());
                    }
                }

                string ruta = Path.Combine(rutaCompuesta, nombreArchivo);
                var pesoBytes = 0;
                using (var memoryStream = new MemoryStream())
                {
                    try
                    {
                        await files.CopyToAsync(memoryStream);
                        var contenIdo = memoryStream.ToArray();
                        pesoBytes = contenIdo.Length;
                        await File.WriteAllBytesAsync(ruta, contenIdo);
                        Log.Info("Archivo Guardado");
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error: {ex.Message}");
                    }
                }
                Archivo archivo = new Archivo();

                archivo.Id = 0;
                archivo.Nombre = nombreArchivo;
                archivo.Ruta = ruta;
                archivo.Tamaño = pesoBytes;
                archivo.Borrado = false;

                try
                {
                    var Archivo = await _archivoRutaService.GuardarArchivo(archivo);
                    Log.Info("Datos del archivo registrado en la base de datos ");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error: {ex.Message}");

                }

                return archivo.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return 1;
            }
        }
        public async Task<ActionResult<Archivo>> ObtenerArchivo(int id)
        {
            try
            {
                var archivo = await _archivoRutaService.obtenerRutaArchivo(id);
                return archivo;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return new Archivo();
        }

        public async Task<ActionResult<Archivo>> obtenerArchivoxUUID(string UUID)
        {
            try
            {
                var archivo = await _archivoRutaService.obtenerArchivoxUUID(UUID);
                return archivo;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return new Archivo();
        }
        public async Task<ActionResult<RespuestaDTO>> EditarArchivo(Archivo archivo)
        {
            RespuestaDTO respuestaDTO = new RespuestaDTO();
            try
            {
                respuestaDTO = await _archivoRutaService.EditarRutaArchivo(archivo);
                return respuestaDTO;
            }
            catch (Exception ex)
            {

                Log.Error(ex.Message);
            }
            return respuestaDTO;
        }



    }


}
