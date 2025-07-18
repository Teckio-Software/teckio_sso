using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO;
using SistemaERP.Models;
using SSO.DTO;

namespace SSO.Controllers
{
    [Route("api/RpEmpresa")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RpEmpresaController : ControllerBase
    {
        private readonly IErpService _RpService;

        private readonly IEmpresaService _empresaServicio;

        public RpEmpresaController(IErpService rpService, IEmpresaService empresaService)
        {
            _RpService = rpService;
            _empresaServicio = empresaService;
        }

        [HttpGet("ObtenerRPIdEmpresa/{idEmpresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ErpCorporativoDTO> ObtenerRPIdEmpresa(int idEmpresa)
        {
            var empresa = await _empresaServicio.ObtenXId(idEmpresa);

            if (empresa != null)
            {
                var rpEmpresa = await _RpService.ObtenXIdCorporativo(empresa.IdCorporativo);

                if (rpEmpresa != null && rpEmpresa.Id > 0)
                {
                    return rpEmpresa;
                }
            }

            // Usar SAP por defecto
            return new ErpCorporativoDTO
            {
                Id = 0,
                IdCorporativo = 0,
                IdErp = 1
            };
        }

        [HttpGet("ObtenerTodosErp")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<ErpDTO>> ObtenerTodos()
        {
            var erps = await _RpService.ObtenErps();
            return erps;
        }
    }
}
