


using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Controlador para saber cual es el ultimo proyecto del usuario dentro de cada empresa
    /// </summary>
    [Route("api/UsuarioUltimaSeccion")]
    [ApiController]
    public class UsuariosUltimaSeccionController : ControllerBase
    {
        private readonly IUsuariosUltimaSeccionService _UsuariosUltimaSeccion;
        public UsuariosUltimaSeccionController(
            IUsuariosUltimaSeccionService usuariosUltimaSeccion)
        {
            _UsuariosUltimaSeccion = usuariosUltimaSeccion;
        }
        /// <summary>
        /// Para obtener la relación de los usuarios y su ultima proyecto escogido por empresa
        /// </summary>
        /// <returns></returns>
        [HttpGet("obtenerUsuariosUltimaSeccion")]
        public async Task<List<UsuariosUltimaSeccionDTO>> ObtenerUsuarioUltimaSeccion()
        {
            var registros = await _UsuariosUltimaSeccion.ObtenTodos();
            return registros;
        }
        /// <summary>
        /// Para obtener el último proyecto usado en la empresa por el usuario
        /// </summary>
        /// <param name="idUsuarioUltimaS"></param>
        /// <returns></returns>
        [HttpGet("obtenerXIdUsuarioUltimaSeccion/{idUsuarioUltimaS:int}")]
        public async Task<ActionResult<UsuariosUltimaSeccionDTO>> ObtenerXIdUsuarioUltimaSeccion(int idUsuarioUltimaS)
        {
            var resultado = await _UsuariosUltimaSeccion.ObtenXIdUltimoS(idUsuarioUltimaS);
            return resultado;
        }
        /// <summary>
        /// Para crear la relación del usuario-proyecto por empresa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost("crearUsuarioUltimaSeccion")]
        public async Task<ActionResult<UsuariosUltimaSeccionDTO>> CrearYObtener([FromBody] UsuariosUltimaSeccionDTO modelo)
        {
            return await _UsuariosUltimaSeccion.CrearYObtener(modelo);

        }
        /// <summary>
        /// Para editar la relación del usuario-proyecto por empresa
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost("editarUsuarioUltimaSeccion")]
        public  async Task<ActionResult<bool>> EditarUsuarioUltimaSeccion([FromBody] UsuariosUltimaSeccionDTO modelo)
        {
            return await _UsuariosUltimaSeccion.Editar(modelo);

        }
        /// <summary>
        /// Para las relaciones del usuario-proyecto a través del usuario
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("obtenrXIdUsuario/{IdUsuario:int}")]
        public async Task<ActionResult<UsuariosUltimaSeccionDTO>> ObtenerXIdUsuarioTablaUsuario(int IdUsuario)
        {
            return await _UsuariosUltimaSeccion.ObtenerXIdUsuario(IdUsuario);
        }
    }
}
