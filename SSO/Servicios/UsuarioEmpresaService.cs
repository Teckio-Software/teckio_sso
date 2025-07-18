using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioEmpresaService : IUsuarioEmpresaService
    {
        private readonly ISSORepositorio<UsuarioEmpresa> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioEmpresaService(
            ISSORepositorio<UsuarioEmpresa> usuarioRepositorio
            , IMapper mapper)
        {
            _Repositorio = usuarioRepositorio;
            _Mapper = mapper;
        }
        /// <summary>
        /// En caso de encontrar la relación, solo la activa, sino la crea
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO> Activar(UsuarioEmpresaDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _Repositorio.ObtenerTodos(u => u.IdUsuario == parametro.IdUsuario);
            objetoEncontrado = objetoEncontrado.Where(u => u.IdEmpresa == parametro.IdEmpresa).ToList();
            if (parametro.IdRol > 0)
            {
                objetoEncontrado = objetoEncontrado.Where(u => u.IdRol == parametro.IdRol).ToList();
            }
            if (objetoEncontrado.Count() <= 0)
            {
                var resultado = await Crear(parametro);
                if (resultado.Id <= 0)
                {
                    respuesta.Descripcion = "No se pudo activar el usuario";
                    return respuesta;
                }
                else
                {
                    respuesta.Estatus = true;
                    respuesta.Descripcion = "Usuario activado";
                    return respuesta;
                }
            }
            objetoEncontrado[0].Activo = !objetoEncontrado[0].Activo;
            respuesta.Estatus = await _Repositorio.Editar(objetoEncontrado[0]);
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
        /// Para cambiar el rol dentro de una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<bool> CambiarRol(UsuarioEmpresaDTO parametro)
        {
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == parametro.Id);

            if (objetoEncontrado == null)
                return false;

            objetoEncontrado.IdRol = parametro.IdRol;
            var estatus = await _Repositorio.Editar(objetoEncontrado);
            return estatus;
        }
        /// <summary>
        /// Crea la relación usuario empresa y si es posible con el rol
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        private async Task<UsuarioEmpresaDTO> Crear(UsuarioEmpresaDTO parametro)
        {
            parametro.Activo = true;
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioEmpresa>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioEmpresaDTO();
            }
            return _Mapper.Map<UsuarioEmpresaDTO>(objetoCreado);
        }
        /// <summary>
        /// Para desactivar un usuario dentro de una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO> Desactivar(UsuarioEmpresaDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            respuesta.Estatus = false;
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == parametro.Id);

            if (objetoEncontrado.Id <= 0)
            {
                objetoEncontrado = await _Repositorio.Obtener(u => u.IdUsuario == parametro.IdUsuario && u.IdEmpresa == parametro.IdEmpresa);
                if (objetoEncontrado.Id <= 0)
                {
                    respuesta.Descripcion = "No se encontró la relación";
                    return respuesta;
                }
            }
            objetoEncontrado.Activo = false;
            respuesta.Estatus = await _Repositorio.Editar(objetoEncontrado);
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
        /// Para obtener todos las relaciones de las empresas y los usuarios sin filtro
        /// </summary>
        /// <returns></returns>
        public async Task<List<UsuarioEmpresaDTO>> ObtenTodos()
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioEmpresaDTO>>(queryMenuRol);
        }
        /// <summary>
        /// Para obtener las relaciones de los usuarios dentro de una empresa
        /// </summary>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public async Task<List<UsuarioEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _Mapper.Map<List<UsuarioEmpresaDTO>>(queryMenuRol);
        }
        /// <summary>
        /// Obtiene las relaciones con los usuarios y las empresas por rol
        /// </summary>
        /// <param name="IdRol"></param>
        /// <returns></returns>
        public async Task<List<UsuarioEmpresaDTO>> ObtenXIdRol(int IdRol)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdRol == IdRol);
            return _Mapper.Map<List<UsuarioEmpresaDTO>>(queryMenuRol);
        }
        /// <summary>
        /// Obtiene las relaciones de las empresas a través del Id del usuario
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        public async Task<List<UsuarioEmpresaDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var queryMenuRol = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioEmpresaDTO>>(queryMenuRol);
        }
        /// <summary>
        /// Obtiene la relación del usuario con la empresa
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        public async Task<List<UsuarioEmpresaDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            lista = lista.Where(z => z.IdEmpresa == IdEmpresa).ToList();
            return _Mapper.Map<List<UsuarioEmpresaDTO>>(lista);
        }
    }
}
