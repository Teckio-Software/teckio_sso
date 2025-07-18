using Microsoft.AspNetCore.Mvc;
using SistemaERP.DTO;
using SistemaERP.BLL.Contrato;
using SSO.DTO;

namespace SistemaERP.API.Controllers
{
    /// <summary>
    /// Es mayormente para proveedores para el cargado de facturas, creo que lo hizo Mario
    /// </summary>
    [Route("api/parametrosempresa")]
    [ApiController]
    public class ParametrosEmpresaController : ControllerBase
    {
        private readonly IParametrosEmpresaService _parametrosempresaService;

        public ParametrosEmpresaController(IParametrosEmpresaService parametrosempresaService)
        {
            _parametrosempresaService = parametrosempresaService;
        }
        /// <summary>
        /// Lista de los parametros de las empresas.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Lista/{id:int}")]
        public async Task<IActionResult> Lista(int id)
        {
            var rsp = new Response<List<ParametrosEmpresaDTO>>();

            try
            {
                rsp.status = true;
                rsp.value = await _parametrosempresaService.Lista(id);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }
            return Ok(rsp);

        }
        /// <summary>
        /// Para guardar un parametro de la empresa
        /// </summary>
        /// <param name="ParametrosEmpresa"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] ParametrosEmpresaDTO ParametrosEmpresa)
        {
            var rsp = new Response<ParametrosEmpresaDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _parametrosempresaService.Crear(ParametrosEmpresa);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }
        /// <summary>
        /// Para editar un parametro de la empresa
        /// </summary>
        /// <param name="ParametrosEmpresa"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] ParametrosEmpresaDTO ParametrosEmpresa)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                rsp.value = await _parametrosempresaService.Editar(ParametrosEmpresa);

            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }
    }
}
