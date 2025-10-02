using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.DTO;
using SSO.Servicios.Contratos;

namespace SSO.Controllers
{
    [Route("api/Logs")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LogController : ControllerBase
    {
        private readonly ILogService _service;

        public LogController(ILogService service)
        {
            _service = service;
        }

        [HttpGet("ObtenerLogsEmpresa/{idempresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<LogDTO>> ObtenerTodosEmpresa(int idempresa)
        {
            var lista = await _service.ObtenerXIdEmpresa(idempresa);
            return lista;
        }
    }
}
