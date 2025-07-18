using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.DTO;
using SSO.DTO;
using System.Text.RegularExpressions;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Para la relacion de los usuarios con el sistema de AutoFac
    /// </summary>
    [Route("api/usuarioAutoFac")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioAutoFacController : ControllerBase
    {
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly IEmpresaService _empresaServicio;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly ProcesoRelacionUsuarioEmpresa _ProcesoUsuarioEmpresa;
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IUsuarioAutoFacService _usuarioAutoFacService;

        public UsuarioAutoFacController(
            IEmpresaService empresaServicio
            , IUsuarioService usuarioService
            , UserManager<IdentityUser> userManager
            , IUsuarioEmpresaService usuarioEmpresaService
            , ProcesoRelacionUsuarioEmpresa ProcesoUsuarioEmpresa
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , IUsuarioCorporativoService UsuarioCorporativoService
            , IUsuarioAutoFacService usuarioAutoFacService
            )
        {
            zvUserManager = userManager;
            _empresaServicio = empresaServicio;
            _UsuarioService = usuarioService;
            _UsuarioEmpresaService = usuarioEmpresaService;
            _ProcesoUsuarioEmpresa = ProcesoUsuarioEmpresa;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _usuarioAutoFacService = usuarioAutoFacService;
        }
        /// <summary>
        /// Para obtener la relación de los usuarios-AutoFactura por el id del usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenerUsuarioAutoFacxidUsuario/{idUsuario}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<UsuarioAutoFacDTO> ObtenerUsuarioAutoFacxId(int idUsuario)
        {
            UsuarioAutoFacDTO usuarioAutoFacDTO = new UsuarioAutoFacDTO();
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;

            var usuario = await _usuarioAutoFacService.ObtenXIdUsuario(idUsuario);
            usuarioAutoFacDTO = usuario;

            return usuarioAutoFacDTO;
        }
        /// <summary>
        /// Crea la relación del usuario-AutoFac (tabla)
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("creaRelacionUsuarioEmpresaAutoFac")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> CrearRelacionEmpresas(UsuarioAutoFacEnVariasEmpresasCreacionDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            var authen = HttpContext.User;
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            bool ProcesoUsuarioEmpresa = false;
            var usuario = await _UsuarioService.ObtenXUsername(zvUsernameClaim);
            if (usuario.Id > 0)
            {
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                ProcesoUsuarioEmpresa = await _ProcesoUsuarioCreacion.CreaUsuarioAutoFac(parametro, usuarioCorporativo.IdCorporativo);
            }
            resp.Estatus = ProcesoUsuarioEmpresa;
            if (ProcesoUsuarioEmpresa)
            {
                resp.Descripcion = "Usuario AutoFactura creado";
            }
            else
            {
                resp.Descripcion = "Algo salió mal";
            }
            return resp;
        }
        /// <summary>
        /// Actualiza la información complementaria del usuario
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPut("ActualizaUsuarioAutoFac")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "VisorPermiso")]
        public async Task<ActionResult<RespuestaDTO>> ActualizaUsuarioAutoFacs(UsuarioAutoFacConsultaDTO parametro)
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

            var respuesta2 = Regex.IsMatch(parametro.Correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            if (!respuesta2)
            {
                resp.Estatus = false;
                resp.Descripcion = "Capture un correo electrónico válido";
                return resp;
            }
            
            usuario.Correo = parametro.Correo;
            usuario.NombreUsuario = parametro.NombreUsuario;
            usuario.NombreCompleto = parametro.NombreCompleto;
            usuario.Apaterno = parametro.Apaterno;
            usuario.Amaterno = parametro.Amaterno;
            var resultado = await _UsuarioService.Editar(usuario);
            var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser);
            usuarioIdentity.NormalizedEmail = parametro.Correo;
            usuarioIdentity.Email = parametro.Correo;
            usuarioIdentity.UserName = parametro.NombreUsuario;
            usuarioIdentity.NormalizedUserName = parametro.NombreUsuario;
            await zvUserManager.UpdateAsync(usuarioIdentity);

            var usuarioAutoFac = await _usuarioAutoFacService.ObtenXIdUsuario(usuario.Id);
            if (usuarioAutoFac.id > 0)
            {
                resp.Estatus = await _usuarioAutoFacService.Editar(new UsuarioAutoFacDTO()
                {
                    id = usuarioAutoFac.id,
                    idUsuario = usuario.Id,
                    fechaAlta = parametro.fechaAlta,
                    fechaBaja = parametro.fechaBaja,
                    estatus = parametro.estatus,
                });
                    if (resp.Estatus)
                    {
                        resp.Descripcion = "Usuario AutoFac editado";
                        return resp;
                    }
                    else
                    {
                        resp.Descripcion = "Algo salió mal en la edición del usuario AutoFac";
                        return resp;
                    }
            }
            return resp;

        }
        /// <summary>
        /// Obtiene la información de la tabla de usuarios-AutoFac por el usuario
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("ObtenInfoUsuarioAutoFac/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UsuarioAutoFacConsultaDTO>> ObtenInfoUsuarioAutoFac(int IdUsuario)
        {
            UsuarioAutoFacConsultaDTO usuarioAutoFacConsultaDTO = new UsuarioAutoFacConsultaDTO();
            var authen = HttpContext.User;
            var claims = authen.Claims.ToList();
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            if (usuario.Id <= 0)
            {
                return usuarioAutoFacConsultaDTO;
            }
            var usuarioAutoFac = await _usuarioAutoFacService.ObtenXIdUsuario(IdUsuario);
            if (usuarioAutoFac.id <= 0)
            {
                return usuarioAutoFacConsultaDTO;
            }
            usuarioAutoFacConsultaDTO.Id = usuario.Id;
            usuarioAutoFacConsultaDTO.IdAspNetUser = usuario.IdAspNetUser;
            usuarioAutoFacConsultaDTO.NombreUsuario = usuario.NombreUsuario;
            usuarioAutoFacConsultaDTO.NombreCompleto = usuario.NombreCompleto;
            usuarioAutoFacConsultaDTO.Apaterno = usuario.Apaterno;
            usuarioAutoFacConsultaDTO.Amaterno = usuario.Amaterno;
            usuarioAutoFacConsultaDTO.Activo = usuario.Activo;
            usuarioAutoFacConsultaDTO.Correo = usuario.Correo;
            return usuarioAutoFacConsultaDTO;
        }
    }
}
