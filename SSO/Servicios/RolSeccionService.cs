using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class RolSeccionService : IRolSeccionService
    {
        private readonly ISSORepositorio<RolSeccion> _Repositorio;
        private readonly IMapper _mapper;

        public RolSeccionService(ISSORepositorio<RolSeccion> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _mapper = mapper;
        }

        public async Task<RespuestaDTO> Crear(RolSeccionDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var objetoCreado = await _Repositorio.Crear(_mapper.Map<RolSeccion>(parametro));

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

        public async Task<RolSeccionDTO> CrearYObtener(RolSeccionDTO parametro)
        {
            try
            {
                var objetoCreado = await _Repositorio.Crear(_mapper.Map<RolSeccion>(parametro));

                if (objetoCreado.Id <= 0)
                {
                    return new RolSeccionDTO();
                }
                return _mapper.Map<RolSeccionDTO>(objetoCreado);

            }
            catch (Exception ex)
            {
                return new RolSeccionDTO();
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

        public async Task<List<RolSeccionDTO>> ObtenTodos()
        {
            try
            {
                var queryMenuRol = await _Repositorio.ObtenerTodos();
                var listaUsuarios = queryMenuRol.ToList();
                return _mapper.Map<List<RolSeccionDTO>>(listaUsuarios);
            }
            catch (Exception ex)
            {
                return new List<RolSeccionDTO>();
            }
        }

        public async Task<List<RolSeccionDTO>> ObtenTodosXIdRol(int IdRol)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdRol == IdRol);
            return _mapper.Map<List<RolSeccionDTO>>(lista);
        }

        public async Task<List<RolSeccionDTO>> ObtenTodosXIdRolXIdSeccion(int IdRol, int IdSeccion)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdRol == IdRol);
            var lista = queryMenuRol.Where(z => z.IdSeccion == IdSeccion).ToList();
            return _mapper.Map<List<RolSeccionDTO>>(lista);
        }

        public async Task<List<RolSeccionDTO>> ObtenTodosXIdSeccion(int IdSeccion)
        {
            try
            {
                var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdSeccion == IdSeccion);
                var listaUsuarios = queryMenuRol.ToList();
                return _mapper.Map<List<RolSeccionDTO>>(listaUsuarios);
            }
            catch (Exception ex)
            {
                return new List<RolSeccionDTO>();
            }
        }
    }
}
