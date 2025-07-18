using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioSeccionService : IUsuarioSeccionService
    {
        private readonly ISSORepositorio<UsuarioSeccion> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioSeccionService(
            ISSORepositorio<UsuarioSeccion> usuarioRepositorio
            , IMapper mapper)
        {
            _Repositorio = usuarioRepositorio;
            _Mapper = mapper;
        }
        public async Task<RespuestaDTO> ActivarPermisoUsuario(UsuarioSeccionDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _Repositorio.ObtenerTodos(u => u.IdEmpresa == parametro.IdEmpresa);
            objetoEncontrado = objetoEncontrado.Where(z => z.IdUsuario ==  parametro.IdUsuario).ToList();
            objetoEncontrado = objetoEncontrado.Where(z => z.IdSeccion ==  parametro.IdSeccion).ToList();
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
                respuesta.Descripcion = "No se pudó activar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Permiso activado";
            return respuesta;
        }
        public async Task<RespuestaDTO> DesactivarPermisoUsuario(int Id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == Id);
            if (objetoEncontrado.Id <= 0)
            {
                respuesta.Descripcion = "No se pudó encontrar";
                return respuesta;
            }
            objetoEncontrado.EsActivo = false;
            respuesta.Estatus = await _Repositorio.Editar(objetoEncontrado);
            if (!respuesta.Estatus)
            {
                respuesta.Descripcion = "No se pudó desactivar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Permiso desactivado";
            return respuesta;
        }
        private async Task<UsuarioSeccionDTO> Crear(UsuarioSeccionDTO parametro)
        {
            parametro.EsActivo = true;
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioSeccion>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioSeccionDTO();
            }
            return _Mapper.Map<UsuarioSeccionDTO>(objetoCreado);
        }

        public async Task<List<UsuarioSeccionDTO>> ObtenTodos()
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioSeccionDTO>>(queryMenuRol);
        }

        public async Task<UsuarioSeccionDTO> ObtenUsuariosXId(int Id)
        {
            var queryMenuRol = await _Repositorio.Obtener(z => z.Id == Id);
            return _Mapper.Map<UsuarioSeccionDTO>(queryMenuRol);
        }

        public async Task<List<UsuarioSeccionDTO>> ObtenXIdSeccion(int IdSeccion)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdSeccion == IdSeccion);
            return _Mapper.Map<List<UsuarioSeccionDTO>>(queryMenuRol);
        }

        public async Task<List<UsuarioSeccionDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioSeccionDTO>>(queryMenuRol);
        }

        public async Task<List<UsuarioSeccionDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa)
        {
            var listaUsuarios = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            listaUsuarios = listaUsuarios.Where(z => z.IdEmpresa == IdEmpresa).ToList();
            return _Mapper.Map<List<UsuarioSeccionDTO>>(listaUsuarios);
        }
    }
}
