using AutoMapper;
using GuardarArchivos.DTO;
using Microsoft.EntityFrameworkCore;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SistemaERP.Models;
using SSO.DTO;
using SSO.Modelos;
using SSO.Servicios.Contratos;
using System.Linq.Expressions;

namespace SSO.Servicios
{
    public class ArchivosEmpresaService : IArchivosEmpresaService
    {
        private readonly ISSORepositorio<ParametrosTimbrado> _Repositorio;

        private readonly IMapper _mapper;

        public ArchivosEmpresaService(ISSORepositorio<ParametrosTimbrado> ArchivosEmpresaRepositorio, IMapper mapper)
        {
            _Repositorio = ArchivosEmpresaRepositorio; 
            _mapper = mapper;
        }

        public async Task<RespuestaDTO> Crear(ParametrosTimbradoDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var ArchivoEmpresaCreado = await _Repositorio.Crear(_mapper.Map<ParametrosTimbradoDTO>(parametro));

                if (ArchivoEmpresaCreado.Id == 0)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = $"No se pudo guardar.";
                    return respuesta;
                }
                respuesta.Estatus = true;
                respuesta.Descripcion = $"Se guardó correctamente.";
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = $"Algo salió mal en la creación.";
                return respuesta;
            }
        }

        public async Task<ParametrosTimbradoDTO> CrearYObtener(ParametrosTimbradoDTO parametro)
        {
            try
            {
                var ArchivoEmpresaCreado = await _Repositorio.Crear(_mapper.Map<ParametrosTimbradoDTO>(parametro));

                if (ArchivoEmpresaCreado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<ParametrosTimbradoDTO>(ArchivoEmpresaCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<RespuestaDTO> Editar(ParametrosTimbradoDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var ArchivoEmpresa = _mapper.Map<ParametrosTimbrado>(parametro);

                var ArchivoEmpresaEncontrado = await _Repositorio.Obtener(u => u.Id == ArchivoEmpresa.Id);

                if (ArchivoEmpresaEncontrado == null)
                    throw new TaskCanceledException("El archivo no se encontró");

                ArchivoEmpresaEncontrado.FechaVigencia = ArchivoEmpresa.FechaVigencia;
                ArchivoEmpresaEncontrado.FechaExpedicion = ArchivoEmpresa.FechaExpedicion;
                ArchivoEmpresaEncontrado.Estatus = ArchivoEmpresa.Estatus;
                ArchivoEmpresaEncontrado.Predeterminado = ArchivoEmpresa.Predeterminado;
                respuesta.Estatus = await _Repositorio.Editar(ArchivoEmpresaEncontrado);

                if (!respuesta.Estatus)
                    throw new TaskCanceledException("No se pudo editar");

                respuesta.Descripcion = "Actulizado correctamente.";
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaDTO> Eliminar(int Id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var ArchivoEmpresaEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

                if (ArchivoEmpresaEncontrado == null)
                    throw new TaskCanceledException("El archivo no existe");

                respuesta.Estatus = await _Repositorio.Eliminar(ArchivoEmpresaEncontrado);

                if (!respuesta.Estatus)
                    throw new TaskCanceledException("No se pudo eliminar");

                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<List<ParametrosTimbradoDTO>> ObtenTodos()
        {
            try
            {
                var listaarchivosempresa = await _Repositorio.ObtenerTodos(x=> x.Estatus == 1);
                return _mapper.Map<List<ParametrosTimbradoDTO>>(listaarchivosempresa.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ParametrosTimbradoDTO>> ObtenenXEmpresa(int IdEmpresa)
        {
            try
            {
                var listaarchivosempresa = await _Repositorio.ObtenerTodos(
                    z => z.IdEmpresa == IdEmpresa,
                    x => x.Include(_=> _.ContenidoCER).AsNoTracking(),
                    x => x.Include(_=> _.ContenidoKEY).AsNoTracking()
                );
                listaarchivosempresa.ForEach(x => { 
                    if(x.ContenidoCER != null && x.ContenidoKEY != null)
                        x.ContenidoCER.ArchivoCer = null;
                    if (x.ContenidoKEY != null) {
                        x.ContenidoKEY.ArchivoKey = null;
                    }
                });
                return _mapper.Map<List<ParametrosTimbradoDTO>>(listaarchivosempresa);
            }
            catch (Exception ex)
            {
                return new List<ParametrosTimbradoDTO>();
            }
        }

        public async Task<ParametrosTimbradoDTO> ObtenenXId(int id)
        {
            try
            {
                var archivo = await _Repositorio.ObtenerTodos(
                    z => z.Id == id,
                    x => x.Include(_ => _.ContenidoCER).AsNoTracking(),
                    x => x.Include(_ => _.ContenidoKEY).AsNoTracking()
                );
                archivo.ForEach(x => {
                    if (x.ContenidoCER != null)
                        x.ContenidoCER.ArchivoCer = null;
                    if (x.ContenidoKEY != null)
                    {
                        x.ContenidoKEY.ArchivoKey = null;
                    }
                });
                return _mapper.Map<ParametrosTimbradoDTO>(archivo.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return new ParametrosTimbradoDTO();
            }
        }

        public async Task<List<ParametrosTimbradoDTO>> Filtar(Expression<Func<ParametrosTimbrado, bool>> filtro)
        {
            try {
                var listaarchivosempresa = await _Repositorio.ObtenerTodos(
                    filtro,
                    x => x.Include(_ => _.ContenidoCER).AsNoTracking(),
                    x => x.Include(_ => _.ContenidoKEY).AsNoTracking()
                    );
                listaarchivosempresa.ForEach(x => {
                    if (x.ContenidoCER != null)
                        x.ContenidoCER.ArchivoCer = null;
                    if (x.ContenidoKEY != null)
                    {
                        x.ContenidoKEY.ArchivoKey = null;
                    }
                });
                return _mapper.Map<List<ParametrosTimbradoDTO>>(listaarchivosempresa);

            }
            catch (Exception ex)
            {
                return new List<ParametrosTimbradoDTO>();
            }
        }
    }
}
