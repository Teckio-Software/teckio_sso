using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Para descargar el manual de usuario
    /// </summary>
    [Route("api/manual")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ManualController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        public ManualController(
            IWebHostEnvironment env
            )
        {
            this.env = env;
        }
        /// <summary>
        /// Descarga el manual de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet("descargaManual")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DescargaFacturaPdf()
        {
            var ruta = Path.Combine(env.WebRootPath, "Manual", "abc.pdf");
            var memory = new MemoryStream();
            using (var stream = new FileStream(ruta, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(ruta);
            var file = File(memory, contentType, fileName);
            return file;
        }
        /// <summary>
        /// Descarga el manual de usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet("descargaPlantilla")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> descargaPlantilla()
        {
            var ruta = Path.Combine(env.WebRootPath, "PlantillaBorrado", "uuids.xlsx");
            var memory = new MemoryStream();
            using (var stream = new FileStream(ruta, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(ruta);
            var file = File(memory, contentType, fileName);
            return file;
        }
    }
}
