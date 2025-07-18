using AutoMapper;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.BLL.Contrato;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class RolService : IRolService
    {
        private readonly ISSORepositorio<Rol> _Repositorio;
        private readonly IMapper _mapper;

        public RolService(ISSORepositorio<Rol> rolRepositorio, IMapper mapper)
        {
            _Repositorio = rolRepositorio;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> ObtenTodos()
        {
            try
            {
                var listaRoles = await _Repositorio.ObtenerTodos();
                return _mapper.Map<List<RolDTO>>(listaRoles.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<RespuestaDTO> Crear(RolDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var RolCreado = await _Repositorio.Crear(_mapper.Map<Rol>(parametro));

                if (RolCreado.Id == 0)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "No se pudo crear";
                    return respuesta;
                }
                respuesta.Estatus = true;
                respuesta.Descripcion = "Rol creado";
                return respuesta;
                //return _mapper.Map<RolDTO>(RolCreado);

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Algo salió mal en la creación del rol";
                return respuesta;
            }
        }
        public async Task<RolDTO> CrearYObtener(RolDTO modelo)
        {
            try
            {
                var RolCreado = await _Repositorio.Crear(_mapper.Map<Rol>(modelo));

                if (RolCreado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<RolDTO>(RolCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<RespuestaDTO> Editar(RolDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var RolModelo = _mapper.Map<Rol>(parametro);

                var RolEncontrado = await _Repositorio.Obtener(u => u.Id == RolModelo.Id);

                if (RolEncontrado == null)
                    throw new TaskCanceledException("El rol no existe");

                RolEncontrado.Nombre = RolModelo.Nombre;


                respuesta.Estatus = await _Repositorio.Editar(RolEncontrado);

                if (!respuesta.Estatus)
                    throw new TaskCanceledException("No se pudo editar");

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
                var rolEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

                if (rolEncontrado == null)
                    throw new TaskCanceledException("El rol no existe");

                respuesta.Estatus = await _Repositorio.Eliminar(rolEncontrado);

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

        public async Task<RolDTO> ObtenXId(int Id)
        {
            try
            {
                var listaRoles = await _Repositorio.Obtener(z => z.Id == Id);
                return _mapper.Map<RolDTO>(listaRoles);
            }
            catch (Exception ex)
            {
                return new RolDTO();
            }
        }

        public async Task<List<RolDTO>> ObtenXIdAspNetRol(string Id)
        {
            try
            {
                var listaRoles = await _Repositorio.ObtenerTodos(z => z.IdAspNetRole.ToLower() == Id.ToLower());
                return _mapper.Map<List<RolDTO>>(listaRoles);
            }
            catch (Exception ex)
            {
                return new List<RolDTO>();
            }
        }

        public async Task<List<RolDTO>> ObtenXNombre(string Nombre)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.Nombre.ToLower() == Nombre.ToLower());
            return _mapper.Map<List<RolDTO>>(lista);
        }
    }
}
