using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using System.Linq;
using System.Security.Claims;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Para la creación de un usuario corporativo
    /// </summary>
    [Route("api/usuarioCorporativo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioCorporativoController : ControllerBase
    {
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly ICorporativoService _corporativoServicio;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly ProcesoUsuarioCorporativo _UsuarioCorporativoProceso;
        private readonly ProcesoUsuarioCreacion _UsuarioCreacionProceso;
        private readonly ICorporativoService _CorporativoService;
        private readonly IErpService _ErpService;
        public UsuarioCorporativoController(
            ICorporativoService corporativoServicio
            , IUsuarioService usuarioService
            , UserManager<IdentityUser> userManager
            , IUsuarioCorporativoService usuarioCorporativoService
            , ProcesoUsuarioCorporativo UsuarioCorporativoProceso
            , ProcesoUsuarioCreacion UsuarioCreacionProceso
            , ICorporativoService CorporativoService
            , IErpService erpService
            )
        {
            zvUserManager = userManager;
            _corporativoServicio = corporativoServicio;
            _UsuarioService = usuarioService;
            _UsuarioCorporativoService = usuarioCorporativoService;
            _UsuarioCorporativoProceso = UsuarioCorporativoProceso;
            _UsuarioCreacionProceso = UsuarioCreacionProceso;
            _CorporativoService = CorporativoService;
            _ErpService = erpService;
        }
        /// <summary>
        /// Cuando un usuario entra al sistema llega a tener una tiene que tener acceso a un determinado número de empresas de un corporativo
        /// Este método te permite saber a que empresas tienes acceso dependiendo a una sección (pantalla)
        /// </summary>
        /// <param name="empresaFiltradoDTO"></param>
        /// <returns>Lista de <see cref="CorporativoDTO"/></returns>
        [HttpGet("usuariosXCorporativo/{idCorporativo:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task<List<UsuarioDTO>> obtenerCorporativosPertenecientes(int idCorporativo)
        {
            List<UsuarioDTO> usuariosFinal = new List<UsuarioDTO>();
            var usuariosCorporativo = await _UsuarioCorporativoService.ObtenXIdCorporativo(idCorporativo);
            var usuarios = await _UsuarioService.ObtenTodos();
            foreach (var usuario in usuariosCorporativo)
            {
                if (usuarios.Where(z => z.Id == usuario.IdUsuario).Count() > 0)
                {
                    usuariosFinal.Add(usuarios.Where(z => z.Id == usuario.IdUsuario).FirstOrDefault()!);
                }
            }
            return usuariosFinal;
        }
        [HttpGet("ObtenXIdUsuario/{idusuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UsuarioCorporativoDTO> ObtenXIdUsuario(int idusuario)
        {
            var usuariosCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(idusuario);
            return usuariosCorporativo;
        }

        /// <summary>
        /// Cuando un usuario entra al sistema llega a tener una tiene que tener acceso a un determinado número de empresas de un corporativo
        /// Este método te permite saber a que empresas tienes acceso dependiendo a una sección (pantalla)
        /// </summary>
        /// <param name="empresaFiltradoDTO"></param>
        /// <returns>Lista de <see cref="CorporativoDTO"/></returns>
        [HttpGet("corporativosPertenecientes")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[Authorize(Policy = "RoleRFACIL")]
        [Authorize(Policy = "Administrador")]
        public async Task<List<CorporativoDTO>> obtenerCorporativosPertenecientes()
        {
            var authen = HttpContext.User;
            //Al construir el token el primer claim que se le pasa es el nombre de usuario (username).
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usernameClaim = HttpContext.User.Claims.Where(z => z.Type == "username").ToList();
            var esActivoClaim = HttpContext.User.Claims.Where(z => z.Type == "activo").ToList();
            var esRfacilClaim = HttpContext.User.IsInRole("Administrador");
            List<CorporativoDTO> corporativos = new List<CorporativoDTO>();
            if (esRfacilClaim)
            {
                corporativos = await _CorporativoService.ObtenTodos();
            }
            else
            {
                var usuario = await _UsuarioService.ObtenXUsername(usernameClaim[0].Value);
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                var corporativo = await _CorporativoService.ObtenXId(usuarioCorporativo.IdCorporativo);
                if (corporativo.Id > 0)
                {
                    corporativos.Add(corporativo);
                }
            }
            var ERPCorporativos = await _ErpService.ObtenTodos();
            var erpDict = ERPCorporativos.ToDictionary(e => e.IdCorporativo, e => e.IdErp);
            foreach (var corporativo in corporativos)
            {
                if (erpDict.TryGetValue(corporativo.Id, out var idErp))
                {
                    corporativo.IdERP = idErp;
                }
            }
            return corporativos;

        }
        /// <summary>
        /// Para la creación de un usuario corporativo
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("CreaUsuarioCorporativo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task CreaUsuarioCorporativo([FromBody] UsuarioCorporativoCreacionDTO parametro)
        {
            var authen = HttpContext.User;
            //Al construir el token el primer claim que se le pasa es el nombre de usuario (username).
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var corporativoRespuesta = await _UsuarioCreacionProceso.CreaRelacionUsuarioCorporativo(parametro);
        }
        /// <summary>
        /// Para la edición de un usuario corporativo
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("EditaUsuarioCorporativo")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrador")]
        public async Task EditaUsuarioCorporativo([FromBody] UsuarioCorporativoEdicionDTO parametro)
        {
            var authen = HttpContext.User;
            //Al construir el token el primer claim que se le pasa es el nombre de usuario (username).
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var corporativoRespuesta = await _UsuarioCorporativoProceso.EditaUsuarioCorporativo(parametro);
        }
    }
}
