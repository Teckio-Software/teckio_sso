 using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SistemaERP.DTO.SSO;
using SSO.DTO;
using SSO.Procesos;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Controlador para las empresas dependientes del corporativo
    /// </summary>
    [Route("api/empresa")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaService _empresaServicio;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteClienteService;
        private readonly ProcesoArchivosTimbradoEmpresa _AlmacenarTimbrado;
        public EmpresaController(
            IEmpresaService empresaServicio
            , IUsuarioService usuarioService
            , ProcesoArchivosTimbradoEmpresa procesoArchivosTimbradoEmpresa
            , IUsuarioPertenecienteAClienteService UsuarioPertenecienteClienteService
            )
        {
            _AlmacenarTimbrado = procesoArchivosTimbradoEmpresa;
            _empresaServicio = empresaServicio;
            _UsuarioService = usuarioService;
            _UsuarioPertenecienteClienteService = UsuarioPertenecienteClienteService;
        }
        /// <summary>
        /// Si es un usuario de rfacil va a mandar las empresas si es un usuario corporativo va a verificar que este dentro de ese corporativo primero
        /// </summary>
        /// <param name="IdCorporativo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ObtenEmpresasPorCorporativo/{IdCorporativo:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<List<EmpresaDTO>>> Lista(int IdCorporativo)
        {
            var empresas = await _empresaServicio.ObtenXIdCorporativo(IdCorporativo);
            var roleUsuario2 = HttpContext.User.IsInRole("Administrador");
            if (roleUsuario2)
            {
                return empresas;
            }
            //var roleUsuario = HttpContext.User.Claims.Where(z => z.Type == "role").Where(z => z.Value == "Administrador").ToList();
            //if (roleUsuario.Count() > 0)
            //{
            //    return empresas;
            //}
            var guidUsuario = HttpContext.User.Claims.Where(z => z.Type == "guid").ToList();
            if (guidUsuario.Count() <= 0)
            {
                return new List<EmpresaDTO>();
            }
            var usuario = await _UsuarioService.ObtenXGuidUsuario(guidUsuario[0].Value);
            var perteneceACorporativo = await _UsuarioPertenecienteClienteService.ObtenXIdUsuarioXIdCliente(usuario.Id, IdCorporativo);
            if (perteneceACorporativo.Id > 0)
            {
                return empresas;
            }
            return new List<EmpresaDTO>();
        }

        /// <summary>
        /// Si es un usuario de rfacil va a mandar las empresas si es un usuario corporativo va a verificar que este dentro de ese corporativo primero
        /// </summary>
        /// <param name="IdCorporativo"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ObtenEmpresasxid/{IdEmpresa:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<EmpresaDTO> Empresa(int IdEmpresa)
        {
            var empresas = await _empresaServicio.ObtenXId(IdEmpresa);
            
                return empresas;

        }

        [HttpGet]
        [Route("ObtenEmpresasxSociedad/{sociedad}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<EmpresaDTO> Empresa(string sociedad)
        {
            var empresas = await _empresaServicio.ObtenXSociedad(sociedad);

            return empresas;

        }

        [HttpGet]
        [Route("ObtenEmpresasxIdCorporativo/{idcorporativo:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<EmpresaDTO>> EmpresaxIdCorp(int idcorporativo)
        {
            var empresas = await _empresaServicio.ObtenXIdCorporativo(idcorporativo);
            return empresas;
        }


        /// <summary>
        /// Para crear una nueva empresa
        /// </summary>
        /// <param name="Empresa"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar(EmpresaDTO Empresa)
        {
            var resultado = await _empresaServicio.CrearYObtener(Empresa);
            if (resultado.Id > 0)
            {
                if (Empresa.CertificadoKey != null) 
                {
                    var alamacenarcertprocess = await _AlmacenarTimbrado.AlmacenarCertificadoYKey(Empresa.CertificadoKey, resultado.Id);
                    if (alamacenarcertprocess.Estatus) {
                        return Ok();
                    }
                }
                return Ok();
            }
            return NoContent();
        }
        /// <summary>
        /// Para editar una empresa
        /// </summary>
        /// <param name="Empresa"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] EmpresaDTO Empresa)
        {
            var rsp = new Response<bool>();
            rsp.status = true;
            rsp.value = await _empresaServicio.Editar(Empresa);
            return Ok(rsp);
        }
    }
}
