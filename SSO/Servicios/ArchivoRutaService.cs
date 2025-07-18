using AutoMapper;
using GuardarArchivos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SistemaERP.BLL.Contrato;

using SistemaERP.BLL.Contrato.Utilidades;

using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.Servicios.Utilidades
{
    public class ArchivoRutaService : IArchivoRutaService
    {
        private readonly ISSORepositorio<Archivo> _Archivos;
        private readonly IUsuarioService _UsuarioService;
        
        private readonly IConfiguration _Configuration;
       
   


        public ArchivoRutaService(ISSORepositorio<Archivo> Archivos
          
           , IConfiguration configuration
           , IUsuarioService usuarioService
            , IMapper mapper
            
           // ,IParametrosEmpresaGastosService<DbContext> parametrosempresaService
           )
        {
            _Archivos = Archivos;
            _Mapper = mapper;
            _UsuarioService = usuarioService;
            _Configuration = configuration;
          //  _parametrosempresaService = parametrosempresaService;
        }
       
        private readonly IMapper _Mapper;
        /// <summary>
        /// metodo para obtener la informacion registrada en la base acerca del archivo 
        /// </summary>
        /// <param name="id"> id a buscar en la base de datos</param>
        /// <returns> archivo dto con los registros del archivo guardado </returns>
        public async Task<Archivo> obtenerRutaArchivo(int id)
        {
            try
            {
              
                var query = await _Archivos.Obtener(z => z.Id == id && z.Borrado == false);
                return _Mapper.Map<Archivo>(query);
            }
            catch
            {
                return new Archivo();
            }
        }

        /// <summary>
        /// metodo para la edicion de el registro del archivo en la base
        /// </summary>
        /// <param name="archivo">Dto del archivo a editar</param>
        /// <returns>mensaje de estado del proceso </returns>
        public async Task<RespuestaDTO> EditarRutaArchivo(Archivo archivo)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var modelo = _Mapper.Map<Archivo>(archivo);
                var objetoEncontrado = await _Archivos.Obtener(z => z.Id == archivo.Id);
                if (objetoEncontrado == null)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "El archivo no existe";
                    return respuesta;
                }
                objetoEncontrado.Nombre = archivo.Nombre;
                objetoEncontrado.Ruta = archivo.Ruta;
                objetoEncontrado.Tamaño = archivo.Tamaño;
                objetoEncontrado.Borrado = archivo.Borrado;

                respuesta.Estatus = await _Archivos.Editar(objetoEncontrado);
                if (!respuesta.Estatus)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "No se pudo editar";
                    return respuesta;
                }
                respuesta.Estatus = true;
                respuesta.Descripcion = "Archivo editado";
                return respuesta;
            }
            catch
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Algo salió mal en la edición del archivo";
                return respuesta;
            }
        }
        
        public async Task<Archivo> GuardarArchivo(Archivo parametro)
        {
            var objetoCreado = await _Archivos.Crear(_Mapper.Map<Archivo>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new Archivo();
            }
            return _Mapper.Map<Archivo>(objetoCreado);
        }

        public async Task<Archivo> obtenerArchivoxUUID(string UUID)
        {
            try
            {

                var query = await _Archivos.Obtener(z => z.Nombre == UUID && z.Borrado == false);
                return _Mapper.Map<Archivo>(query);
            }
            catch
            {
                return new Archivo();
            }
        }

    } 
}
