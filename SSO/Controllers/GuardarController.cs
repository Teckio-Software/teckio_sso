
using GuardarArchivos;
using GuardarArchivos.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.DTO;



namespace GuardarController
{
    [Route("api/Guardar/")]
    [ApiController]
    public class GuardarController : ControllerBase
    {
        private readonly GuardarArchivosC _GuardarService;



        public GuardarController(GuardarArchivosC GuardarService)
        {
            _GuardarService = GuardarService;
        }

        /// <summary>
        /// controller que resive la peticion de guardado del archivo
        /// </summary>
        /// <param name="files"> iFormfile a del archivo a guardar</param>
        /// <param name="Ruta"> ruta base donde se van a crear las subcarpetas</param>
        /// <param name="RFCEmpresa"> El RFC de la empresa asociada al archivo</param>
        /// <param name="Empresa">El nombre de la empresa para obtener la cadena de conexión de la base de datos</param>
        /// <returns> id de registro del archivo en la tabla tblArchivos</returns>
        [HttpPost]
        [Route("Archivo")]
        public async Task<ActionResult<int>> Archivo(IFormFile files, [FromForm] string ruta, [FromForm] string RFC)
        {

            try
            {
                var respuesta = await _GuardarService.Post(files, ruta, RFC);

                return respuesta;
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                return 1;

            }

        }

        [HttpGet("obtenerArchivo/{id}")]

        public async Task<ActionResult<Archivo>> ObtenerArchivo(int id)
        {

            var respuesta = await _GuardarService.ObtenerArchivo(id);
            return respuesta;
        }

        [HttpPost("EditarArchivo")]
        public async Task<ActionResult<RespuestaDTO>> EditarArchivo([FromBody] Archivo archivo)
        {
            var authen = HttpContext.User;
            var usernameClaim = authen.Claims.FirstOrDefault()!.Value;
            return await _GuardarService.EditarArchivo(archivo);
        }

        [HttpGet("obtenerArchivoxUUID/{UUID}")]

        public async Task<ActionResult<Archivo>> obtenerArchivoxUUID(string UUID)
        {

            var respuesta = await _GuardarService.obtenerArchivoxUUID(UUID);
            return respuesta;
        }
    }
}