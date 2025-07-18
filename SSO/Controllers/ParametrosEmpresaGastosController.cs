using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.DTO.SSO;
using SSO.DTO;

namespace SistemaERP.API.Controllers
{
    [Route("api/ParametrosEmpresaGastos")]
    [ApiController]
    public class ParametrosEmpresaGastosController : ControllerBase
    {
        private readonly IParametrosEmpresaGastosService _parametrosempresaService;

        public ParametrosEmpresaGastosController(IParametrosEmpresaGastosService parametrosempresaService)
        {
            _parametrosempresaService = parametrosempresaService;
        }

        [HttpGet]
        [Route("ObtenerTodosParametrosxEmpresa/{idEmpresa}")]
        public async Task<IActionResult> ObtenerTodosxEmpresa(int idEmpresa)
        {
            var rsp = new Response<List<ParametrosEmpresaGastosDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _parametrosempresaService.ObtenTodosxEmpresa(idEmpresa);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);

        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<ParametrosEmpresaGastosDTO> Guardar([FromBody] ParametrosEmpresaGastosDTO ParametrosEmpresa)
        {
            var respuesta = new ParametrosEmpresaGastosDTO();

            try
            {
                respuesta = await _parametrosempresaService.Crear(ParametrosEmpresa);

            }
            catch (Exception ex)
            {
                return new ParametrosEmpresaGastosDTO();

            }
            return respuesta;
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<ParametrosEmpresaGastosDTO> Editar([FromBody] ParametrosEmpresaGastosDTO ParametrosEmpresa)
        {
            var rsp = new ParametrosEmpresaGastosDTO();

            try
            {
                rsp = await _parametrosempresaService.Editar(ParametrosEmpresa);

            }
            catch (Exception ex)
            {
                return new ParametrosEmpresaGastosDTO();

            }
            return rsp;
        }
    }
}
