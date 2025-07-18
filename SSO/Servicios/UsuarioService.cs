using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ISSORepositorio<Usuario> _UsuarioRepositorio;
        private readonly IMapper _Mapper;

        public UsuarioService(
            ISSORepositorio<Usuario> usuarioRepositorio
            , IMapper mapper)
        {
            _UsuarioRepositorio = usuarioRepositorio;
            _Mapper = mapper;
        }
        /// <summary>
        /// Si esta activo para una empresa entonces se agrega un claim a AspNetUsers con el id de la empresa
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<RespuestaDTO> Activar(int IdUsuario)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _UsuarioRepositorio.Obtener(u => u.Id == IdUsuario);
            if (objetoEncontrado == null)
            {
                respuesta.Descripcion = "El usuario no se encuentra registrado";
            }
            objetoEncontrado.Activo = true;
            respuesta.Estatus = await _UsuarioRepositorio.Editar(objetoEncontrado);
            if (!respuesta.Estatus)
            {
                respuesta.Descripcion = "No se pudo activar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Usuario activado";
            return respuesta;
        }
        /// <summary>
        /// Crea y obtiene la relación del usuario y devuelve el registro
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<UsuarioDTO> CrearYObtener(UsuarioDTO parametro)
        {
            var objetoCreado = await _UsuarioRepositorio.Crear(_Mapper.Map<Usuario>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioDTO();
            }
            return _Mapper.Map<UsuarioDTO>(objetoCreado);
        }
        /// <summary>
        /// Si se desactiva se quita el claim con el id de la empresa de los AspNetUserClaimss
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        public async Task<RespuestaDTO> Desactivar(int IdUsuario)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _UsuarioRepositorio.Obtener(u => u.Id == IdUsuario);
            if (objetoEncontrado == null)
            {
                respuesta.Descripcion = "El usuario no se encuentra registrado";
            }
            objetoEncontrado.Activo = false;
            respuesta.Estatus = await _UsuarioRepositorio.Editar(objetoEncontrado);
            if (!respuesta.Estatus)
            {
                respuesta.Descripcion = "No se pudo desactivar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Usuario desactivado";
            return respuesta;
        }
        /// <summary>
        /// Edita los campos de un usuario
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO> Editar(UsuarioDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _UsuarioRepositorio.Obtener(u => u.Id == parametro.Id);
            if (objetoEncontrado.Id <= 0)
            {
                respuesta.Descripcion = "El usuario no existe";
                return respuesta;
            }
            objetoEncontrado.NombreCompleto = parametro.NombreCompleto;
            objetoEncontrado.NombreUsuario = parametro.NombreUsuario;
            objetoEncontrado.Correo = parametro.Correo;
            objetoEncontrado.Apaterno = parametro.Apaterno;
            objetoEncontrado.Amaterno = parametro.Amaterno;
            respuesta.Estatus = await _UsuarioRepositorio.Editar(objetoEncontrado);
            if (!respuesta.Estatus)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se pudo editar";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Usuario editado";
            return respuesta;
        }

        public async Task<List<UsuarioDTO>> ObtenTodos()
        {
            var listaUsuarios = await _UsuarioRepositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioDTO>>(listaUsuarios);
        }

        public async Task<List<UsuarioDTO>> ObtenXEmailUsuario(string Email)
        {
            var listaUsuarios = await _UsuarioRepositorio.ObtenerTodos(z => z.Correo.ToLower() == Email.ToLower());
            return _Mapper.Map<List<UsuarioDTO>>(listaUsuarios);
        }

        public async Task<UsuarioDTO> ObtenXGuidUsuario(string GuidUsuario)
        {
            var listaUsuarios = await _UsuarioRepositorio.Obtener(z => z.IdAspNetUser == GuidUsuario);
            return _Mapper.Map<UsuarioDTO>(listaUsuarios);
        }

        public async Task<List<UsuarioDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var listaUsuarios = await _UsuarioRepositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioDTO>>(listaUsuarios);
        }

        public async Task<UsuarioDTO> ObtenXIdUsuario(int IdUsuario)
        {
            var objeto = await _UsuarioRepositorio.Obtener(z => z.Id == IdUsuario);
            return _Mapper.Map<UsuarioDTO>(objeto);
        }

        public async Task<UsuarioDTO> ObtenXUsername(string Username)
        {
            var objeto = await _UsuarioRepositorio.Obtener(z => z.NombreUsuario == Username);
            return _Mapper.Map<UsuarioDTO>(objeto);
        }
    }
}
