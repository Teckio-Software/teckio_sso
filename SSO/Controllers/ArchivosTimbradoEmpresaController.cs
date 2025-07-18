using GuardarArchivos.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.Model;
using SSO.DTO;
using SSO.Procesos;

namespace SSO.Controllers
{
    [Route("api/ArchivosEmpresa")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ArchivosTimbradoEmpresaController : ControllerBase
    {

        private ProcesoArchivosTimbradoEmpresa _ProcesoArchivosTimbrado;

        public ArchivosTimbradoEmpresaController(ProcesoArchivosTimbradoEmpresa procesoArchivosTimbrado)
        {
            _ProcesoArchivosTimbrado = procesoArchivosTimbrado;
        }


        [HttpGet("ObtenerParametrosTimbradoEmpresa/{idempresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<ParametrosTimbradoDTO>> ObtenerTodosEmpresa(int idempresa)
        {
            var ArchivosEmpresa = await _ProcesoArchivosTimbrado.ObtenerArchivosEmpresa(idempresa);
            return ArchivosEmpresa;
        }

        [HttpGet("ObtenerParametroTimbrado/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ParametrosTimbradoDTO> ObtenerXId(int id)
        {
            var ArchivosEmpresa = await _ProcesoArchivosTimbrado.ObtenerParametro(id);
            return ArchivosEmpresa;
        }

        [HttpGet("ObtenerParametroDefault/{idempresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ParametrosTimbradoDTO> ObtenerParametroDefault(int idempresa)
        {
            var ArchivosEmpresa = await _ProcesoArchivosTimbrado.ObtenerParametro(idempresa, true);
            return ArchivosEmpresa;
        }

        [HttpPost("GuardarParametroTimbrado/{idempresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<RespuestaDTO> GuardarCertkey([FromForm] CertKeyDTO certKeyDTO, int idempresa)
        {
            RespuestaDTO Response = await _ProcesoArchivosTimbrado.AlmacenarCertificadoYKey(certKeyDTO, idempresa);
            return Response;
        }

        [HttpPut("EditarParametroTimbrado")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<RespuestaDTO> EditarArchivo([FromBody] ParametrosTimbradoDTO ParametrosTimbradoDTO)
        {
            RespuestaDTO Response = await _ProcesoArchivosTimbrado.EditarArchivoTimbrado(ParametrosTimbradoDTO);
            return Response;
        }

        [HttpDelete("EliminarParametroTimbrado/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<RespuestaDTO> EliminarArchivo(int id)
        {
            RespuestaDTO Response = await _ProcesoArchivosTimbrado.EliminarArchivoTimbrado(id);
            return Response;
        }

    }
}
