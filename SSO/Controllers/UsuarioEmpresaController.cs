using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.DAL.DBContext;
using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.API.Controllers.SSO
{
    /// <summary>
    /// Para la creación de un usuario operativo, ya sea normal, proveedor o empleado
    /// </summary>
    [Route("api/usuarioempresas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuarioEmpresaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRolManager;
        private readonly IEmpresaService _empresaServicio;
        private readonly IUsuarioService _UsuarioService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly ProcesoRelacionUsuarioEmpresa _ProcesoUsuarioEmpresa;
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IUsuarioSeccionService _UsuarioSeccionService;
        private readonly IUsuarioActividadService _UsuarioActividadService;
        private readonly IUsuarioGastosService _UsuarioGastosService;
        private readonly IUsuarioAutoFacService _UsuarioAutoFacService;
        private readonly IUsuarioProveedorService _UsuarioProveedorService;

        public UsuarioEmpresaController(
            IEmpresaService empresaServicio
            , IUsuarioService usuarioService
            , UserManager<IdentityUser> userManager
            , RoleManager<IdentityRole> rolManager
            , IUsuarioEmpresaService usuarioEmpresaService
            , ProcesoRelacionUsuarioEmpresa ProcesoUsuarioEmpresa
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , IUsuarioCorporativoService UsuarioCorporativoService
            , IUsuarioPertenecienteAClienteService UsuarioPertenecienteService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IUsuarioSeccionService UsuarioSeccionService
            , IUsuarioActividadService UsuarioActividadService
            , IRolService RolService
            , IRolEmpresaService RolEmpresaService
            , IUsuarioGastosService usuarioGastosService
            , IUsuarioAutoFacService usuarioAutoFacService
            , IUsuarioProveedorService UsuarioProveedorService
            )
        {
            zvUserManager = userManager;
            zvRolManager = rolManager;
            _empresaServicio = empresaServicio;
            _UsuarioService = usuarioService;
            _UsuarioEmpresaService = usuarioEmpresaService;
            _ProcesoUsuarioEmpresa = ProcesoUsuarioEmpresa;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _UsuarioPertenecienteService = UsuarioPertenecienteService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
            _UsuarioSeccionService = UsuarioSeccionService;
            _UsuarioActividadService = UsuarioActividadService;
            _RolService = RolService;
            _RolEmpresaService = RolEmpresaService;
            _UsuarioGastosService = usuarioGastosService;
            _UsuarioAutoFacService = usuarioAutoFacService;
            _UsuarioProveedorService = UsuarioProveedorService;
        }
        /// <summary>
        /// Cuando un usuario entra al sistema tiene que tener acceso a un determinado número de empresas de un corporativo
        /// Este método te permite saber a que empresas tienes acceso dependiendo a una sección (pantalla)
        /// </summary>
        /// <param name="empresaFiltradoDTO"></param>
        /// <returns>Lista de <see cref="EmpresaDTO"/></returns>
        [HttpGet("empresasPertenecientes")]
        public async Task<List<EmpresaDTO>> obtenerEmpresasPertenecientes()
        {
            var zvUsernameClaim = HttpContext.User.Claims.FirstOrDefault()!.Value;
            var ProcesoUsuarioEmpresa = await _ProcesoUsuarioEmpresa.ObtenEmpresasPertenecientes(HttpContext.User.Claims.ToList());
            return ProcesoUsuarioEmpresa;
        }


        [HttpGet("ObtenXIdUsuarioXIdEmpresa/{idusuario:int}/{idempresa:int}")]
        public async Task<List<UsuarioEmpresaDTO>> ObtenXIdUsuarioXIdEmpresa(int idusuario, int idempresa)
        {
            var lista = await _UsuarioEmpresaService.ObtenXIdUsuarioXIdEmpresa(idusuario, idempresa);
            return lista;
        }

        /// <summary>
        /// Para la activación de un usuario en una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("activarEmpresaEnUsuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Administrador")]
        public async Task<ActionResult<RespuestaDTO>> activarEmpresaEnUsuario([FromBody] UsuarioEmpresaDTO parametro)
        {
            RespuestaDTO respuestaDTO = new RespuestaDTO();
            respuestaDTO.Estatus = false;
            if (parametro.IdEmpresa <= 0 && parametro.IdUsuario <= 0)
            {
                respuestaDTO.Descripcion = "Capture los campos necesarios";
                return respuestaDTO;
            }
            var zvUsernameClaim = HttpContext.User.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXUsername(zvUsernameClaim);
            if (usuario.Id > 0)
            {
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                var empresas = await _empresaServicio.ObtenXIdCorporativo(usuarioCorporativo.IdCorporativo);
                empresas = empresas.Where(z => z.Id == parametro.IdEmpresa).ToList();
                if (empresas.Count() <= 0)
                {
                    respuestaDTO.Descripcion = "La empresa no corresponde al corporativo";
                    return respuestaDTO;
                }
                var resultado = await _UsuarioEmpresaService.Activar(parametro);
                if (parametro.IdRol > 0)
                {
                    var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
                    var rolIdentity = await zvRolManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
                    var claimsRol = await zvRolManager.GetClaimsAsync(rolIdentity);
                    var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser);
                    var resultado1 = await zvUserManager.AddClaimsAsync(usuarioIdentity, claimsRol);
                    if (resultado.Estatus && resultado1.Succeeded)
                    {
                        return resultado;
                    }
                    respuestaDTO.Descripcion = "Algo salió mal";
                    return respuestaDTO;
                }
            }
            respuestaDTO.Descripcion = "Usuario no permitido";
            return respuestaDTO;
        }
        /// <summary>
        /// Para desactivar un usuario dentro de una empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        [HttpPost("desactivarEmpresaEnUsuario")]
        public async Task<ActionResult<RespuestaDTO>> desactivarEmpresaEnUsuario([FromBody] UsuarioEmpresaDTO parametro)
        {
            RespuestaDTO respuestaDTO = new RespuestaDTO();
            respuestaDTO.Estatus = false;
            if (parametro.IdEmpresa <= 0 && parametro.IdUsuario <= 0)
            {
                if (parametro.Id <= 0)
                {
                    respuestaDTO.Descripcion = "Capture los campos necesarios";
                    return respuestaDTO;
                }
            }
            var zvUsernameClaim = HttpContext.User.Claims.FirstOrDefault()!.Value;
            var usuario = await _UsuarioService.ObtenXUsername(zvUsernameClaim);
            if (usuario.Id > 0)
            {
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                var empresas = await _empresaServicio.ObtenXIdCorporativo(usuarioCorporativo.IdCorporativo);
                empresas = empresas.Where(z => z.Id == parametro.IdEmpresa).ToList();
                if (empresas.Count() <= 0)
                {
                    respuestaDTO.Descripcion = "La empresa no corresponde al corporativo";
                    return respuestaDTO;
                }
                var resultado = await _UsuarioEmpresaService.Desactivar(parametro);
                if (parametro.IdRol > 0)
                {
                    var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
                    var rolIdentity = await zvRolManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
                    var claimsRol = await zvRolManager.GetClaimsAsync(rolIdentity);
                    var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser);
                    var resultado1 = await zvUserManager.RemoveClaimsAsync(usuarioIdentity, claimsRol);
                    if (resultado.Estatus && resultado1.Succeeded)
                    {
                        return resultado;
                    }
                    respuestaDTO.Descripcion = "Algo salió mal";
                    return respuestaDTO;
                }
            }
            respuestaDTO.Descripcion = "Usuario no permitido";
            return respuestaDTO;
        }

        /// <summary>
        /// Para la relacion de los usuarios con sus empresas y sus permisos 
        /// </summary>
        /// <param name="IdCorporativo"></param>
        /// <returns></returns>
        [HttpGet("usuarioXEmpresa/{IdCorporativo:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "AdministradorYAdminRoles")]
        public async Task<List<UsuarioEstructuraCorporativoDTO>> ObtenUsuariosDeCliente(int IdCorporativo)
        {
            var authen = HttpContext.User;
            //Al construir el token el primer claim que se le pasa es el nombre de usuario (username).
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usernameClaim = HttpContext.User.Claims.Where(z => z.Type == "username").ToList();
            var esActivoClaim = HttpContext.User.Claims.Where(z => z.Type == "activo").ToList();
            List<UsuarioEstructuraCorporativoDTO> usuariosFinal = new List<UsuarioEstructuraCorporativoDTO>();
            var usuariosCorporativos = await _UsuarioCorporativoService.ObtenXIdCorporativo(IdCorporativo);
            var secciones = await _CatalogoSeccionService.ObtenTodos();
            var actividades = await _CatalogoActividadService.ObtenTodos();
            var usuariosCliente = await _UsuarioPertenecienteService.ObtenXIdCliente(IdCorporativo);
            var usuarios = await _UsuarioService.ObtenTodos();
            var empresasPorCorporativo = await _empresaServicio.ObtenXIdCorporativo(IdCorporativo);
            var usuariosEmpresas = await _UsuarioEmpresaService.ObtenTodos();
            var usuariosSecciones = await _UsuarioSeccionService.ObtenTodos();
            var usuariosActividades = await _UsuarioActividadService.ObtenTodos();
            var usuariosProveedores = await _UsuarioProveedorService.ObtenTodos();
            var usuariosGastos = await _UsuarioGastosService.ObtenTodos();
            var usuariosAutoFac = await _UsuarioAutoFacService.ObtenTodos();

            var esAdminRoles = HttpContext.User.IsInRole("AdministradorRoles");
            if (esAdminRoles) {
                var usuario = await _UsuarioService.ObtenXUsername(usernameClaim[0].Value);
                var usuariosAsignados = new List<UsuarioEmpresaDTO>();
                foreach (var userEmpresa in usuariosEmpresas) {
                    if (userEmpresa.IdUsuario == usuario.Id && userEmpresa.Activo) {
                        var usuariosEncontrados = usuariosEmpresas.Where(z => z.IdEmpresa == userEmpresa.IdEmpresa && z.Activo == true);
                        usuariosAsignados.AddRange(usuariosEncontrados);
                    }
                }

                var nuevosUsuariosClientes = new List<UsuarioPertenecienteAClienteDTO>();

                foreach (var asignados in usuariosAsignados) {
                    var existeUsuario = usuariosCliente.FirstOrDefault(z => z.IdUsuario == asignados.IdUsuario);
                    var existeUsuarioClientes = nuevosUsuariosClientes.FirstOrDefault(z => z.IdUsuario == asignados.IdUsuario);
                    if (existeUsuarioClientes == null) {
                        nuevosUsuariosClientes.Add(existeUsuario);
                    }
                }

                usuariosCliente = nuevosUsuariosClientes;
            }
            foreach (var usuarioCliente in usuariosCliente)
            {
                var esCorporativo = usuariosCorporativos.Where(z => z.IdUsuario == usuarioCliente.IdUsuario).ToList();
                if (esCorporativo.Count() > 0)
                {
                    continue;
                }
                var usuarioActual = usuarios.Where(z => z.Id == usuarioCliente.IdUsuario).ToList();
                if (usuarioActual[0].NombreUsuario.ToLower() == zvUsernameClaim.ToLower())
                {
                    continue;
                }
                var usuarioEmpresas = usuariosEmpresas.Where(z => z.IdUsuario == usuarioCliente.IdUsuario).ToList();
                if (usuarios.Where(z => z.Id == usuarioCliente.IdUsuario).Count() > 0)
                {
                    int varTipo = 1;
                    string varRfc = "";
                    var usuario = usuarios.Where(z => z.Id == usuarioCliente.IdUsuario).FirstOrDefault()!;
                    var esUsuarioProveedor = usuariosProveedores.Where(z => z.IdUsuario == usuario.Id).ToList();
                    var esUsuarioGastos = usuariosGastos.Where(z => z.idUsuario == usuario.Id).ToList();
                    var esUsuarioAutoFac = usuariosAutoFac.Where(z => z.idUsuario == usuario.Id).ToList();
                    if (esUsuarioProveedor.Count() > 0)
                    {
                        varTipo = 2;
                        varRfc = esUsuarioProveedor[0].Rfc;
                    }
                    if (esUsuarioGastos.Count() > 0)
                    {
                        varTipo = 3;
                        varRfc = esUsuarioGastos[0].numeroEmpleado;
                    }
                    if (esUsuarioAutoFac.Count() > 0)
                    {
                        varTipo = 4;
                        
                    }
                    usuariosFinal.Add(new UsuarioEstructuraCorporativoDTO()
                    {
                        IdUsuario = usuario.Id,
                        NombreUsuario = usuario.NombreUsuario,
                        Id = usuario.Id,
                        NombreCompleto = usuario.NombreCompleto,
                        Apaterno = usuario.Apaterno,
                        Amaterno = usuario.Amaterno,
                        Correo = usuario.Correo,
                        Activo = usuario.Activo,
                        Tipo = varTipo,
                        Rfc = varRfc,
                        IdAspNetUser = usuario.IdAspNetUser,
                    });
                }
            }
            return usuariosFinal;
        }

        /// <summary>
        /// Para obtener la relación de los usuarios que tienen relacion con las empresas dentro de un mismo corporativo
        /// </summary>
        /// <param name="IdCorporativo"></param>
        /// <param name="IdUsuario"></param>
        /// <returns></returns>
        [HttpGet("empresasXUsuario/{IdCorporativo:int}/{IdUsuario:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "AdministradorYAdminRoles")]

        public async Task<List<UsuarioEmpresaEstructura>> obtenerEmpresasDeCorporativoEnClientes(int IdCorporativo, int IdUsuario)
        {
            var authen = HttpContext.User;
            //Al construir el token el primer claim que se le pasa es el nombre de usuario (username).
            var zvUsernameClaim = authen.Claims.FirstOrDefault()!.Value;
            var usernameClaim = HttpContext.User.Claims.Where(z => z.Type == "username").ToList();
            var esActivoClaim = HttpContext.User.Claims.Where(z => z.Type == "activo").ToList();
            List<UsuarioEmpresaEstructura> empresasFinal = new List<UsuarioEmpresaEstructura>();
            var empresasPorCorporativo = await _empresaServicio.ObtenXIdCorporativo(IdCorporativo);
            var usuario = await _UsuarioService.ObtenXIdUsuario(IdUsuario);
            var usuarioGastos = await _UsuarioGastosService.ObtenXIdUsuario(usuario.Id);
            var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuario(IdUsuario);

            var esAdminRoles = HttpContext.User.IsInRole("AdministradorRoles");
            if (esAdminRoles)
            {
                var usuarioAdminRoles = await _UsuarioService.ObtenXUsername(usernameClaim[0].Value);
                var usuariosEmpresas = await _UsuarioEmpresaService.ObtenTodos();
                var empresasAdminRoles = new List<EmpresaDTO>();
                foreach (var userEmpresa in usuariosEmpresas)
                {
                    if (userEmpresa.IdUsuario == usuarioAdminRoles.Id && userEmpresa.Activo)
                    {
                        var existeEmpresa = empresasPorCorporativo.FirstOrDefault(z => z.Id == userEmpresa.IdEmpresa);
                        empresasAdminRoles.Add(existeEmpresa);
                    }
                }

                empresasPorCorporativo = empresasAdminRoles;

            }

            var rolesEmpresa = await _RolEmpresaService.ObtenTodos();
            var roles = await _RolService.ObtenTodos();
            foreach (var empresa in empresasPorCorporativo)
            {
                List<RolDTO> rolesFinal = new List<RolDTO>();
                var usuarioEmpresaFiltrado = usuarioEmpresa.Where(z => z.IdEmpresa == empresa.Id).ToList();
                var rolesEmpresaFiltrado = rolesEmpresa.Where(z => z.IdEmpresa == empresa.Id);
                foreach (var rolEmpresaFiltrado in rolesEmpresaFiltrado)
                {
                    var rol = roles.Where(z => z.Id == rolEmpresaFiltrado.IdRol).ToList();
                    rolesFinal.Add(rol[0]);
                }
                if (usuarioGastos.id > 0)
                {
                    empresasFinal.Add(new UsuarioEmpresaEstructura()
                    {
                        IdEmpresa = empresa.Id,
                        IdRol = usuarioEmpresaFiltrado.Count() > 0 ? usuarioEmpresaFiltrado[0].IdRol : 0,
                        ActivoEmpresa = usuarioEmpresaFiltrado.Count() > 0 ? usuarioEmpresaFiltrado[0].Activo : false,
                        NombreEmpresa = empresa.NombreComercial,
                        Roles = rolesFinal,
                        esUsuarioGastos = true
                    });
                }
                else{
                    empresasFinal.Add(new UsuarioEmpresaEstructura()
                    {
                        IdEmpresa = empresa.Id,
                        IdRol = usuarioEmpresaFiltrado.Count() > 0 ? usuarioEmpresaFiltrado[0].IdRol : 0,
                        ActivoEmpresa = usuarioEmpresaFiltrado.Count() > 0 ? usuarioEmpresaFiltrado[0].Activo : false,
                        NombreEmpresa = empresa.NombreComercial,
                        Roles = rolesFinal,
                        esUsuarioGastos = false
                    });  
                }
            }
            return empresasFinal;
        }
    }
}
