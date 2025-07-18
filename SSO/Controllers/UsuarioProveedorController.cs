using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.DTO.SSO;
using SSO.DTO;
using System.Text.RegularExpressions;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Controlador para la relacion del usuario con la información complementaria de un proveedor
    /// </summary>
    [Route("api/usuarioProveedor")]
    [ApiController]
    public class UsuarioProveedorController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly ProcesoUsuariosBusqueda _ProcesoUsuariosBusqueda;
        private readonly ProcesoUsuarioFormaPagoPermitidaPorEmpresa _ProcesoUsuarioFormaPagoPermitidaPorEmpresa;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IUsuarioProveedorService _UsuarioProveedorService;
        private readonly ProcesoUsuarioProveedorCrearConPermisos _ProcesoUsuarioProveedorCrearConPermisos;
        private readonly UserManager<IdentityUser> zvUserManager;
        public UsuarioProveedorController(
            IUsuarioService usuarioService
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , IUsuarioCorporativoService UsuarioCorporativoService
            , ProcesoUsuariosBusqueda ProcesoUsuariosBusqueda
            , IUsuarioProveedorService UsuarioProveedorService
            , UserManager<IdentityUser> zUserManager
            , ProcesoUsuarioFormaPagoPermitidaPorEmpresa ProcesoUsuarioFormaPagoPermitidaPorEmpresa
            , ProcesoUsuarioProveedorCrearConPermisos ProcesoUsuarioProveedorCrearConPermisos
            )
        {
            _UsuarioService = usuarioService;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _ProcesoUsuariosBusqueda = ProcesoUsuariosBusqueda;
            _UsuarioProveedorService = UsuarioProveedorService;
            zvUserManager = zUserManager;
            _ProcesoUsuarioFormaPagoPermitidaPorEmpresa = ProcesoUsuarioFormaPagoPermitidaPorEmpresa;
            _ProcesoUsuarioProveedorCrearConPermisos = ProcesoUsuarioProveedorCrearConPermisos;
        }
        /// <summary>
        /// Para crear un usuario con la relación a proveedor
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("creaRelacionUsuarioProveedor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> CreaRelacionUsuarioEmpresaProveedor(UsuarioProveedorCreacionDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXUsername(zvUsernameClaim);
            bool ProcesoUsuarioEmpresa = false;
            if (usuario.Id > 0)
            {
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                var ProcesoUsuarioEmpresa2 = await _ProcesoUsuarioCreacion.CreaRelacionUsuarioEmpresaProveedor(parametro, usuarioCorporativo.IdCorporativo);
                ProcesoUsuarioEmpresa = ProcesoUsuarioEmpresa2.Id <= 0 ? false: true;
            }
            resp.Estatus = ProcesoUsuarioEmpresa;
            if (ProcesoUsuarioEmpresa)
            {
                resp.Descripcion = "Usuario proveedor creado";
            }
            else
            {
                resp.Descripcion = "Algo salió mal";
            }
            return resp;
        }
        private bool ComparaStringConCodigoASCII(string stringToVerify)
        {
            for (int i = 0; i < stringToVerify.Length; i++)
            {
                //A=65 Z=90 and a=97 z=122
                if ((int)stringToVerify[i] < 65 || ((int)stringToVerify[i] > 90
                    && (int)stringToVerify[i] < 97) || (int)stringToVerify[i] > 122)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Actualiza la información del usuario proveedor
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPut("ActualizaUsuarioProveedor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> ActualizaUsuarioProveedor([FromBody] UsuarioProveedorConsultaDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            if (
                parametro.Id <= 0
                || string.IsNullOrEmpty(parametro.Correo)
                || string.IsNullOrEmpty(parametro.NombreCompleto)
                || string.IsNullOrEmpty(parametro.NombreUsuario)
                || string.IsNullOrEmpty(parametro.Rfc)
                || string.IsNullOrEmpty(parametro.NumeroProveedor)
                )
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture toda la información";
                return resp;
            }
            if (parametro.Rfc.Length > 13 || parametro.Rfc.Length <= 11
                || parametro.NumeroProveedor.Length > 10
                || parametro.IdentificadorFiscal.Length > 30
                )
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture toda la información";
                return resp;
            }
            var usuario = await _UsuarioService.ObtenXUsername(parametro.NombreUsuario);
            if (usuario.Id <= 0)
            {
                resp.Descripcion = "No existe un usuario";
                return resp;
            }
            //var facturasEmeAwait = await _FacturaEMEService.ObtenXRfcEmisor(parametro.Rfc);
            //if (facturasEmeAwait.Count() > 0)
            //{
            //    resp.Descripcion = "El proveedor cuenta con facturas relacionadas";
            //    return resp;
            //}
            //var ordenesEmeAwait = await _OrdenCompraAGA_EMEService.ObtenXRfcEmisor(parametro.Rfc);
            //if (ordenesEmeAwait.Count() > 0)
            //{
            //    resp.Descripcion = "El proveedor cuenta con ordenes de compra relacionadas";
            //    return resp;
            //}
            
            var respuesta2 = Regex.IsMatch(parametro.Correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            if (!respuesta2)
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture un correo electrónico válido";
                return resp;
            }
            string rfcPrimeras4 = "";
            string rfcDel4Al10 = "";
            string rfcUltimasLetras = "";
            if (parametro.Rfc.Length == 13)
            {
                rfcPrimeras4 = parametro.Rfc.Substring(0, 4);
                rfcDel4Al10 = parametro.Rfc.Substring(4, 6);
                rfcUltimasLetras = parametro.Rfc.Substring(10);
            }
            if (parametro.Rfc.Length == 12)
            {
                rfcPrimeras4 = parametro.Rfc.Substring(0, 3);
                rfcDel4Al10 = parametro.Rfc.Substring(3, 6);
                rfcUltimasLetras = parametro.Rfc.Substring(9);
            }
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(rfcDel4Al10) || !ComparaStringConCodigoASCII(rfcPrimeras4))
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture un RFC válido";
                return resp;
            }
            usuario.Correo = parametro.Correo;
            usuario.NombreUsuario = parametro.NombreUsuario;
            usuario.NombreCompleto = parametro.NombreCompleto;
            var resultado = await _UsuarioService.Editar(usuario);
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser);
            usuarioIdentity.NormalizedEmail = parametro.Correo;
            usuarioIdentity.Email = parametro.Correo;
            usuarioIdentity.UserName = parametro.NombreUsuario;
            usuarioIdentity.NormalizedUserName = parametro.NombreUsuario;
            var resultado3 = await zvUserManager.UpdateAsync(usuarioIdentity);
            var usuarioProveedor = await _UsuarioProveedorService.ObtenXIdUsuario(usuario.Id);
            resp.Estatus = await _UsuarioProveedorService.Editar(new UsuarioProveedorDTO()
            {
                Id = usuarioProveedor[0].Id,
                IdentificadorFiscal = parametro.IdentificadorFiscal,
                NumeroProveedor = parametro.NumeroProveedor,
                Rfc = parametro.Rfc,
                IdUsuario = usuario.Id,
            });
            if (resp.Estatus)
            {
                resp.Descripcion = "Usuario proveedor editado";
                return resp;
            }
            else
            {
                resp.Descripcion = "Algo salió mal en la edición del usuario proveedor";
                return resp;
            }
            
        }
        /// <summary>
        /// Obtiene la relación de los usuarios proveedores dependiendo un usuario corporativo
        /// </summary>
        /// <returns></returns>
        [HttpGet("UsuariosProveedores")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<UsuarioProveedorConsultaDTO>>> ObtenUsuariosProveedores()
        {
            var authen = HttpContext.User;
            var claims = authen.Claims.ToList();
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usuarioPertenecienteACliente = await _ProcesoUsuariosBusqueda.ObtenUsuariosProveedores(claims);
            return usuarioPertenecienteACliente;
        }
        /// <summary>
        /// Obtiene la relación de un usuario proveedor a través de su Id
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenInfoUsuarioProveedor/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<UsuarioProveedorConsultaDTO>> ObtenInfoUsuarioProveedor(int IdUsuario)
        {
            UsuarioProveedorConsultaDTO usuarioProveedorConsultaDTO = new UsuarioProveedorConsultaDTO();
            var authen = HttpContext.User;
            var claims = authen.Claims.ToList();
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            if (usuario.Id <= 0)
            {
                return usuarioProveedorConsultaDTO;
            }
            var usuarioProveedor = await _UsuarioProveedorService.ObtenXIdUsuario(IdUsuario);
            if (usuarioProveedor.Count() <= 0)
            {
                return usuarioProveedorConsultaDTO;
            }
            usuarioProveedorConsultaDTO.Id = usuario.Id;
            usuarioProveedorConsultaDTO.IdAspNetUser = usuario.IdAspNetUser;
            usuarioProveedorConsultaDTO.NombreUsuario = usuario.NombreUsuario;
            usuarioProveedorConsultaDTO.NombreCompleto = usuario.NombreCompleto;
            usuarioProveedorConsultaDTO.Activo = usuario.Activo;
            usuarioProveedorConsultaDTO.Correo = usuario.Correo;
            usuarioProveedorConsultaDTO.Rfc = usuarioProveedor[0].Rfc;
            usuarioProveedorConsultaDTO.IdentificadorFiscal = usuarioProveedor[0].IdentificadorFiscal;
            usuarioProveedorConsultaDTO.NumeroProveedor = usuarioProveedor[0].NumeroProveedor;
            return usuarioProveedorConsultaDTO;
        }
        /// <summary>
        /// Para crear un usuario con la relación a proveedor
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpGet("ObtenXRfc/{rfc}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UsuarioProveedorDTO> ObtenerXRFC(string rfc)
        {
            var resp = await _UsuarioProveedorService.ObtenXRfc(rfc);
            return resp;
        }
        [HttpGet("ObtenXIdUsuario/{idusuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<UsuarioProveedorDTO>> ObtenXIdUsuario(int idusuario)
        {
            var resp = await _UsuarioProveedorService.ObtenXIdUsuario(idusuario);
            return resp;
        }
        [HttpGet("ObtenTodos")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<List<UsuarioProveedorDTO>> ObtenTodos()
        {
            var resp = await _UsuarioProveedorService.ObtenTodos();
            return resp;
        }

    } 
}
