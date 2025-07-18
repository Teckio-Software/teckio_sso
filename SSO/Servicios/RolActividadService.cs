using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class RolActividadService : IRolActividadService
    {
        private readonly ISSORepositorio<RolActividad> _Repositorio;
        private readonly IMapper _mapper;

        public RolActividadService(ISSORepositorio<RolActividad> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _mapper = mapper;
        }

        public async Task<RespuestaDTO> Crear(RolActividadDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var objetoCreado = await _Repositorio.Crear(_mapper.Map<RolActividad>(parametro));

                if (objetoCreado.Id <= 0)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "No se pudo crear";
                    return respuesta;
                }
                respuesta.Estatus = true;
                respuesta.Descripcion = "Rol creado";
                return respuesta;
                //return _mapper.Map<RolMenuDTO>(MenuRolCreado);

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<RolActividadDTO> CrearYObtener(RolActividadDTO parametro)
        {
            try
            {
                var objetoCreado = await _Repositorio.Crear(_mapper.Map<RolActividad>(parametro));

                if (objetoCreado.Id <= 0)
                {
                    return new RolActividadDTO();
                }
                return _mapper.Map<RolActividadDTO>(objetoCreado);

            }
            catch (Exception ex)
            {
                return new RolActividadDTO();
            }
        }

        public async Task<RespuestaDTO> Eliminar(int Id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var rolEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

                if (rolEncontrado == null)
                    throw new TaskCanceledException("El permiso del menú no existe");

                respuesta.Estatus = await _Repositorio.Eliminar(rolEncontrado);

                if (!respuesta.Estatus)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "No se pudo eliminar";
                    return respuesta;
                }
                respuesta.Estatus = true;
                respuesta.Descripcion = "Rol eliminado";
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<List<RolActividadDTO>> ObtenTodos()
        {
            try
            {
                var queryMenuRol = await _Repositorio.ObtenerTodos();
                var listaUsuarios = queryMenuRol.ToList();
                return _mapper.Map<List<RolActividadDTO>>(listaUsuarios);
            }
            catch (Exception ex)
            {
                return new List<RolActividadDTO>();
            }
        }

        public async Task<List<RolActividadDTO>> ObtenTodosXIdActividad(int IdActividad)
        {
            try
            {
                var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdActividad == IdActividad);
                //var listaUsuarios = queryMenuRol.ToList();
                return _mapper.Map<List<RolActividadDTO>>(queryMenuRol);
            }
            catch (Exception ex)
            {
                return new List<RolActividadDTO>();
            }
        }

        public async Task<List<RolActividadDTO>> ObtenTodosXIdRol(int IdRol)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdRol == IdRol);
            return _mapper.Map<List<RolActividadDTO>>(queryMenuRol);
        }


        public async Task<List<RolActividadDTO>> ObtenTodosXIdRolXIdActividad(int IdRol, int IdActividad)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdRol == IdRol);
            var resultado = queryMenuRol.Where(z => z.IdActividad == IdActividad);
            return _mapper.Map<List<RolActividadDTO>>(resultado);
        }
    }
}
