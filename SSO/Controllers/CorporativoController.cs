using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SistemaERP.DTO.SSO;
using SSO.DTO;
using SistemaERP.Model;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// El corporativo son los agrupadores para las empresas
    /// </summary>
    [Route("api/Corporativo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CorporativoController : ControllerBase
    {
        private readonly ICorporativoService _CorporativoServicio;
        private readonly IErpService _ErpServicio;

        public CorporativoController(ICorporativoService corporativoServicio, IErpService erpServicio)
        {
            _CorporativoServicio = corporativoServicio;
            _ErpServicio = erpServicio;
        }
        /// <summary>
        /// Aqui enlista los registros de los corporativos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<List<CorporativoDTO>>> Lista()
        {
            var corporativos = await _CorporativoServicio.ObtenTodos();
            return corporativos;
        }

        [HttpGet]
        [Route("ObtenCorporativo/{id:int}")]
        public async Task<CorporativoDTO> ObtenCoporporativoXId(int id)
        {
            var corporativos = await _CorporativoServicio.ObtenXId(id);
            return corporativos;
        }

        /// <summary>
        /// Da de alta un nuevo corporativo
        /// </summary>
        /// <param name="Corporativo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] CorporativoDTO Corporativo)
        {
            var rsp = new Response<CorporativoDTO>();

            try
            {
                rsp.status = true;
                rsp.value = await _CorporativoServicio.Crear(Corporativo);
                if (Corporativo.IdERP.HasValue) 
                {
                    var corporativoerp = new ErpCorporativoDTO { IdCorporativo = rsp.value.Id, IdErp = Corporativo.IdERP.Value };
                    var ecorp = await _ErpServicio.Crear(corporativoerp);
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;

            }
            return Ok(rsp);
        }
        /// <summary>
        /// Edita el registro de un corporativo
        /// </summary>
        /// <param name="Corporativo"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] CorporativoDTO Corporativo)
        {
            var rsp = new Response<bool>();
            try
            {
                rsp.status = true;
                rsp.value = await _CorporativoServicio.Editar(Corporativo);
                if (Corporativo.IdERP.HasValue)
                {
                    var corporativoErp = await _ErpServicio.ObtenXIdCorporativo(Corporativo.Id);
                    var erpDto = new ErpCorporativoDTO
                    {
                        IdCorporativo = Corporativo.Id,
                        IdErp = Corporativo.IdERP.Value,
                        Id = corporativoErp.Id > 0 ? corporativoErp.Id : 0
                    };
                    if (corporativoErp.Id > 0)
                        await _ErpServicio.Editar(erpDto);
                    else
                        await _ErpServicio.Crear(erpDto);
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = ex.Message;
            }

            return Ok(rsp);
        }
        /// <summary>
        /// Elimina el registro de un corporativo
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Eliminar/{Id:int}")]
        public async Task<IActionResult> Eliminar(int Id)
        {
            var rsp = new Response<bool>();

            try
            {
                rsp.status = true;
                var corporativoErp = await _ErpServicio.ObtenXIdCorporativo(Id);
                if (corporativoErp.IdCorporativo > 0) {
                    var rs = await _ErpServicio.Eliminar(corporativoErp.Id);
                    rsp.status = rs.Estatus;
                }
                if (rsp.status)
                {
                    rsp.value = await _CorporativoServicio.Eliminar(Id);
                }
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
