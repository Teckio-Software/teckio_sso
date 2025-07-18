using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioActividadService : IUsuarioActividadService
    {
        private readonly ISSORepositorio<UsuarioActividad> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioActividadService(
            ISSORepositorio<UsuarioActividad> usuarioRepositorio
            , IMapper mapper)
        {
            _Repositorio = usuarioRepositorio;
            _Mapper = mapper;
        }
        public async Task<RespuestaDTO> ActivarPermisoUsuario(UsuarioActividadDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _Repositorio.ObtenerTodos(u => u.IdEmpresa == parametro.IdEmpresa);
            objetoEncontrado = objetoEncontrado.Where(z => z.IdUsuario == parametro.IdUsuario).ToList();
            objetoEncontrado = objetoEncontrado.Where(z => z.IdActividad == parametro.IdActividad).ToList();
            if (objetoEncontrado.Count() <= 0)
            {
                var resultado1 = await Crear(parametro);
                if (resultado1.Id > 0)
                {
                    respuesta.Estatus = true;
                    respuesta.Descripcion = "Permiso activado";
                    return respuesta;
                }
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se pudó encontrar";
                return respuesta;
            }
            objetoEncontrado[0].EsActivo = true;
            respuesta.Estatus = await _Repositorio.Editar(objetoEncontrado[0]);
            if (!respuesta.Estatus)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se pudo activar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Usuario activado";
            return respuesta;
        }
        public async Task<RespuestaDTO> DesactivarPermisoUsuario(int Id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

            if (objetoEncontrado == null)
                throw new TaskCanceledException("El usuario no existe");

            objetoEncontrado.EsActivo = false;
            respuesta.Estatus = await _Repositorio.Editar(objetoEncontrado);

            if (!respuesta.Estatus)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se pudo activar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Usuario activado";
            return respuesta;
        }
        private async Task<UsuarioActividadDTO> Crear(UsuarioActividadDTO parametro)
        {
            parametro.EsActivo = true;
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioActividad>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioActividadDTO();
            }
            return _Mapper.Map<UsuarioActividadDTO>(objetoCreado);
        }

        public async Task<List<UsuarioActividadDTO>> ObtenTodos()
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioActividadDTO>>(queryMenuRol);
        }

        public async Task<UsuarioActividadDTO> ObtenXId(int Id)
        {
            var queryMenuRol = await _Repositorio.Obtener(z => z.Id == Id);
            return _Mapper.Map<UsuarioActividadDTO>(queryMenuRol);
        }

        public async Task<List<UsuarioActividadDTO>> ObtenXIdActividad(int IdActividad)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdActividad == IdActividad);
            return _Mapper.Map<List<UsuarioActividadDTO>>(queryMenuRol);
        }

        public async Task<List<UsuarioActividadDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioActividadDTO>>(queryMenuRol);
        }

        public async Task<List<UsuarioActividadDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            queryMenuRol = queryMenuRol.Where(z => z.IdEmpresa == IdEmpresa).ToList();
            return _Mapper.Map<List<UsuarioActividadDTO>>(queryMenuRol);
        }
    }
}
