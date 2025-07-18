
using GuardarArchivos;
using GuardarArchivos.DTO;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.DTO.SSO;
using SSO.DTO;



namespace AutoFactClientesController
{
    [Route("api/AutoFact/Empresa/")]
    [ApiController]
    public class AutoFactClientesController : ControllerBase
    {
        private readonly EmpresaService _EmpresaService;



        public AutoFactClientesController(EmpresaService EmpresaService)
        {
            _EmpresaService = EmpresaService;
        }


        [HttpGet("obtenerEmpresaxRFC/{RFC}")]

        public async Task<ActionResult<EmpresaDTO>> ObtenerEmpresaxRFC(string RFC)
        {
            var respuesta = await _EmpresaService.ObtenXRFC(RFC);
            return respuesta;
        }



    }
}