using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SistemaERP.Model;
using SSO.DTO;
using SSO.Servicios.Contratos;
using System.Security.Claims;
using static System.Collections.Specialized.BitVector32;

namespace SistemaERP.BLL.ProcesoSSO
{
    public class ProcesoRol
    {
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IRolSeccionService _RolSeccionService;
        private readonly IRolActividadService _RolActividadService;
        private readonly IRolProyectoEmpresaUsuarioService _rolProyectoEmpresaUsuarioService;
        private readonly IRolEmpresaService _rolEmpresaService;
        private readonly ICatalogoSeccionService _catalogoSeccionService;
        private readonly IRolSeccionService _rolSeccionService;
        private readonly ProcesoRol _procesoRol;
        private readonly IUsuarioProyectoService _UsuarioProyectoService;
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRoleManager;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioSeccionService _UsuarioSeccionService;


        /// <summary>
        /// Administrador de los roles, para agregar, editar, eliminar o ver los roles
        /// </summary>
        private readonly RoleManager<IdentityRole> _RoleManager;
        /// <summary>
        /// Constructor de las cuentas controller
        /// </summary>
        /// <param name="UserManager">Administrador de los usuarios, para agregar, editar, eliminar o ver los usuarios</param>
        /// <param name="RoleManager">Administrador de los roles, para agregar, editar, eliminar o ver los roles</param>
        /// <param name="zConfiguration">Para acceder a la configuración de app.settings</param>
        /// <param name="zSignInManager">Para loguearse en el software</param>
        /// <param name="zContext">Para obtener los usuario, roles y claims de un usuario</param>
        /// <param name="zMapper">Convierte los registros nativos del dbContext a los DTO para enviar información</param>
        public ProcesoRol(
            RoleManager<IdentityRole> RoleManager
            , IEmpresaService EmpresaService
            , IRolService RolService
            , IRolEmpresaService RolEmpresaService
            , IRolSeccionService RolSeccionService
            , IRolActividadService RolActividadService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IRolProyectoEmpresaUsuarioService rolProyectoEmpresaUsuarioService
            , IRolEmpresaService rolEmpresaService
            , ICatalogoSeccionService catalogoSeccionService
            , IRolSeccionService rolSeccionService
            , IUsuarioProyectoService UsuarioProyectoService
            , UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , IUsuarioEmpresaService UsuarioEmpresaService
            , IUsuarioService UsuarioService
            , IUsuarioSeccionService UsuarioSeccionService

            )
        {
            _RoleManager = RoleManager;
            _EmpresaService = EmpresaService;
            _RolService = RolService;
            _RolEmpresaService = RolEmpresaService;
            _RolSeccionService = RolSeccionService;
            _RolActividadService = RolActividadService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
            _rolProyectoEmpresaUsuarioService = rolProyectoEmpresaUsuarioService;
            _rolEmpresaService = rolEmpresaService;
            _catalogoSeccionService = catalogoSeccionService;
            _rolSeccionService = rolSeccionService;
            _UsuarioProyectoService = UsuarioProyectoService;
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _UsuarioService = UsuarioService;
            _UsuarioSeccionService = UsuarioSeccionService;
        }
        /// <summary>
        /// Para la creación de un rol asignado directamente a UNA SOLA empresa
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<RolEmpresaDTO> ProcesoCrearRolEnEmpresa(RolCreacionUnaEmpresaDTO parametro)
        {
            var respuesta = new RolEmpresaDTO();
            if (string.IsNullOrEmpty(parametro.Nombre)
                || parametro.IdEmpresa <= 0
                )
            {
                return respuesta;
            }
            var empresa = await _EmpresaService.ObtenXId(parametro.IdEmpresa);
            if (empresa.Id <= 0)
            {
                return respuesta;
            }
            var resultadoIdAspNetRole = await CreaRolIdentityPorEmpresa(parametro.Nombre, parametro.IdEmpresa);
            if (resultadoIdAspNetRole == null)
            {
                return respuesta;
            }
            var resultado2 = await CreaRolEmpresaBasePropia(parametro.Nombre, parametro.IdEmpresa, resultadoIdAspNetRole);
            if (resultado2.Id <= 0)
            {
                return respuesta;
            }
            return resultado2;
        }
        /// <summary>
        /// Para la creación de varios roles (AspNetRoles) y su asignación a VARIAS empresas dentro de un mismo corporativo
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO> ProcesoCrearRolEnVariasEmpresa(RolCreacionVarisEmpresasMismoCorporativoDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            if (string.IsNullOrEmpty(parametro.Nombre)
                || parametro.Empresas.Count() <= 0
                || parametro.Secciones.Count() <= 0
                || parametro.IdCorporativo <= 0
                )
            {
                resp.Estatus = false;
                resp.Descripcion = "Debe tener un nombre, mínimo una empresa y permisos seleccionados";
                return resp;
            }
            var empresas = await _EmpresaService.ObtenXIdCorporativo(parametro.IdCorporativo);
            for (int i = 0; i < parametro.Empresas.Count(); i++)
            {
                var empresasFiltrado = empresas.Where(z => z.Id == parametro.Empresas[i].IdEmpresa).ToList();
                if (empresasFiltrado.Count() <= 0)
                {
                    continue;
                }
                var resultadoIdAspNetRole = await CreaRolIdentityPorEmpresa(parametro.Nombre, parametro.Empresas[i].IdEmpresa);
                if (resultadoIdAspNetRole == null)
                {
                    resp.Estatus = false;
                    resp.Descripcion = "Algo salió mal";
                    return resp;
                }
                var resultado2 = await CreaRolNoIdentityPorEmpresa(parametro.Nombre, parametro.Empresas[i].IdEmpresa, resultadoIdAspNetRole);
                if (resultado2.Id <= 0)
                {
                    resp.Estatus = false;
                    resp.Descripcion = "Algo salió mal";
                    return resp;
                }
                List<CatalogoSeccionConMenuDTO> seccionesFinal = new List<CatalogoSeccionConMenuDTO>();
                for (int j = 0; j < parametro.Secciones.Count(); j++)
                {
                    List<CatalogoActividadParaRolDTO> actividades = new List<CatalogoActividadParaRolDTO>();
                    for (int k = 0; k < parametro.Secciones[j].Actividades.Count(); k++)
                    {
                        actividades.Add(new CatalogoActividadParaRolDTO()
                        {
                            Id = parametro.Secciones[j].Actividades[k].Id,
                            CodigoActividad = parametro.Secciones[j].Actividades[k].CodigoActividad,
                            Descripcion = parametro.Secciones[j].Actividades[k].Descripcion,
                            DescripcionInterna = parametro.Secciones[j].Actividades[k].DescripcionInterna,
                            EsActividadUnica = parametro.Secciones[j].Actividades[k].EsActividadUnica,
                            IdSeccion = parametro.Secciones[j].Actividades[k].IdSeccion,
                            EsSeleccionado = parametro.Secciones[j].Actividades[k].EsSeleccionado
                        });
                    }
                    seccionesFinal.Add(new CatalogoSeccionConMenuDTO()
                    {
                        Actividades = actividades,
                        CodigoSeccion = parametro.Secciones[j].CodigoSeccion,
                        Descripcion = parametro.Secciones[j].Descripcion,
                        DescripcionInterna = parametro.Secciones[j].DescripcionInterna,
                        DescripcionMenu = parametro.Secciones[j].DescripcionMenu,
                        EsSeccionUnica = parametro.Secciones[j].EsSeccionUnica,
                        EsSeleccionado = parametro.Secciones[j].EsSeleccionado,
                        Id = parametro.Secciones[j].Id,
                        IdMenu = parametro.Secciones[j].IdMenu
                    });
                }
                if (parametro.Empresas[i].Secciones.Count() > 0)
                {
                    for (int j = 0; j < seccionesFinal.Count(); j++)
                    {
                        var existeSeccionRol = parametro.Empresas[i].Secciones.Where(z => z.Id == seccionesFinal[j].Id).ToList();
                        var existeSeccionRol2 = existeSeccionRol.Where(z => z.EsSeleccionado == true).ToList();
                        if (existeSeccionRol2.Count() > 0)
                        {
                            seccionesFinal[j].EsSeleccionado = true;
                        }
                        for (int k = 0; k < seccionesFinal[j].Actividades.Count(); k++)
                        {
                            var existeActividadRol = existeSeccionRol[0].Actividades.Where(z => z.Id == seccionesFinal[j].Actividades[k].Id).Where(z => z.EsSeleccionado == true).ToList();
                            if (existeActividadRol.Count() > 0)
                            {
                                seccionesFinal[j].Actividades[k].EsSeleccionado = true;
                            }
                        }
                    }
                }
                var seccionesFinalFiltrado = seccionesFinal.Where(z => z.EsSeleccionado == true).ToList();
                for (int j = 0; j < seccionesFinal.Count(); j++)
                {
                    var claimsRol = await _RoleManager.GetClaimsAsync(resultadoIdAspNetRole);
                    Claim claimAgregar;
                    if (seccionesFinal[j].EsSeccionUnica)
                    {
                        claimAgregar = new Claim(seccionesFinal[j].DescripcionInterna!, "EsSeccionUnica");
                    }
                    else
                    {
                        claimAgregar = new Claim(seccionesFinal[j].DescripcionInterna + "-" + parametro.Empresas[i].IdEmpresa, seccionesFinal[j].CodigoSeccion + "-" + empresasFiltrado[0].GuidEmpresa);
                    }
                    var claimMenuEmpresa = claimsRol.Where(z => z.Type == claimAgregar.Type).ToList();
                    var existeEnSeleccionados = seccionesFinalFiltrado.Where(z => z.Id == seccionesFinal[j].Id).ToList();
                    if (claimMenuEmpresa.Count() <= 0 && existeEnSeleccionados.Count() > 0)
                    {
                        var resultadoAddClaim = await _RoleManager.AddClaimAsync(resultadoIdAspNetRole, claimAgregar);
                    }
                    RolSeccionDTO rolMenuSeccion;
                    var existeRolMenuSeccion = await _RolSeccionService.ObtenTodosXIdSeccion(seccionesFinal[j].Id);
                    existeRolMenuSeccion = existeRolMenuSeccion.Where(z => z.IdRol == resultado2.Id).ToList();
                    if (existeRolMenuSeccion.Count() <= 0 && existeEnSeleccionados.Count() > 0)
                    {
                        var resultado = await _RolSeccionService.CrearYObtener(new RolSeccionDTO()
                        {
                            IdRol = resultado2.Id,
                            IdSeccion = seccionesFinal[j].Id
                        });
                    }
                    var actividadesFinalFiltrado = seccionesFinal[j].Actividades.Where(z => z.EsSeleccionado == true).ToList();
                    for (int k = 0; k < seccionesFinal[j].Actividades.Count(); k++)
                    {
                        Claim claimAgregarActividad;
                        if (seccionesFinal[j].Actividades[k].EsActividadUnica)
                        {
                            claimAgregar = new Claim(seccionesFinal[j].Actividades[k].DescripcionInterna, seccionesFinal[j].Actividades[k].CodigoActividad);
                        }
                        else
                        {
                            claimAgregar = new Claim(seccionesFinal[j].Actividades[k].DescripcionInterna + "-" + empresasFiltrado[0].Id, seccionesFinal[j].Actividades[k].CodigoActividad + "-" + empresasFiltrado[0].GuidEmpresa);
                        }
                        var claimActividadEmpresa = claimsRol.Where(z => z.Type == claimAgregar.Type).ToList();
                        if (claimActividadEmpresa.Count() <= 0 && actividadesFinalFiltrado.Count() > 0)
                        {
                            var resultadoAddClaim = await _RoleManager.AddClaimAsync(resultadoIdAspNetRole, claimAgregar);
                        }
                        var existeRolMenuSeccionActividad = await _RolActividadService.ObtenTodosXIdRolXIdActividad(resultado2.Id, seccionesFinal[j].Actividades[k].Id);
                        if (existeRolMenuSeccionActividad.Count() <= 0 && actividadesFinalFiltrado.Count() > 0)
                        {
                            var resultado = await _RolActividadService.CrearYObtener(new RolActividadDTO()
                            {
                                IdRol = resultado2.Id,
                                IdActividad = seccionesFinal[j].Actividades[k].Id
                            });
                        }
                    }
                }
            }
            resp.Estatus = true;
            resp.Descripcion = "Rol creado";
            return resp;
        }
        /// <summary>
        /// Para la creación de un rol en la table de AspNetRole (propia de identity)
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="IdEmpresa"></param>
        /// <returns></returns>
        private async Task<IdentityRole> CreaRolIdentityPorEmpresa(string nombre, int IdEmpresa)
        {
            if (string.IsNullOrEmpty(nombre) || IdEmpresa <= 0)
            {
                return null;
            }
            //Aqui sabemos si esta duplicado
            var existeRolAsp = await _RoleManager.FindByNameAsync(nombre + "-" + IdEmpresa);
            if (existeRolAsp != null)
            {
                return existeRolAsp;
            }
            var rolCreacion = new IdentityRole { Name = nombre + "-" + IdEmpresa };
            var resultado = await _RoleManager.CreateAsync(rolCreacion);
            if (resultado.Succeeded)
            {
                return rolCreacion;
            }
            return null;
        }
        /// <summary>
        /// Para dar de alta el rol y la relación del rol con la empresa
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="IdEmpresa"></param>
        /// <param name="IdAspNetRole"></param>
        /// <returns></returns>
        private async Task<RolEmpresaDTO> CreaRolEmpresaBasePropia(string nombre, int IdEmpresa, IdentityRole AspNetRole)
        {
            var rolEmpresa = new RolEmpresaDTO();
            if (string.IsNullOrEmpty(nombre) || IdEmpresa <= 0)
            {
                return rolEmpresa;
            }
            var existeRol = await _RolService.ObtenXNombre(nombre);
            var existeRolEmpresa = await _RolEmpresaService.ObtenXIdEmpresa(IdEmpresa);
            for (int i = 0; i < existeRolEmpresa.Count(); i++)
            {
                var siExisteRol = existeRol.Where(z => z.Id == existeRolEmpresa[i].IdRol).ToList();
                if (siExisteRol.Count() > 0)
                {
                    //El rol ya fue creado y tiene una relación con la empresa
                    return rolEmpresa;
                }
            }
            var rolNoIdentity = await _RolService.CrearYObtener(new RolDTO()
            {
                Nombre = nombre,
                FechaRegistro = DateTime.Now,
                IdAspNetRole = AspNetRole.Id
            });
            var rolEmpresaNoIdentity = await _RolEmpresaService.CrearYObtener(new RolEmpresaDTO()
            {
                IdRol = rolNoIdentity.Id,
                IdEmpresa = IdEmpresa
            });
            if (rolEmpresaNoIdentity.Id <= 0 || rolNoIdentity.Id <= 0)
            {
                return rolEmpresa;
            }
            return rolEmpresaNoIdentity;
        }
        /// <summary>
        /// Para dar de alta el rol y la relación del rol con la empresa
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="IdEmpresa"></param>
        /// <param name="IdAspNetRole"></param>
        /// <returns></returns>
        private async Task<RolDTO> CreaRolNoIdentityPorEmpresa(string nombre, int IdEmpresa, IdentityRole AspNetRole)
        {
            if (string.IsNullOrEmpty(nombre) || IdEmpresa <= 0)
            {
                return new RolDTO();
            }
            var existeRol = await _RolService.ObtenXIdAspNetRol(AspNetRole.Id);
            if (existeRol.Count() > 0)
            {
                return new RolDTO();
            }
            var rolNoIdentity = await _RolService.CrearYObtener(new RolDTO()
            {
                Nombre = nombre,
                FechaRegistro = DateTime.Now,
                IdAspNetRole = AspNetRole.Id
            });
            var rolEmpresaNoIdentity = await _RolEmpresaService.CrearYObtener(new RolEmpresaDTO()
            {
                IdRol = rolNoIdentity.Id,
                IdEmpresa = IdEmpresa
            });
            if (rolEmpresaNoIdentity.Id <= 0 || rolNoIdentity.Id <= 0)
            {
                return new RolDTO();
            }
            return rolNoIdentity;
        }

        public async Task<ActionResult<RolSeccionDTO>> autorizaSeccionARol([FromBody] RolSeccionDTO parametro)
        {
            var resultado = new RolSeccionDTO();

            var seccion = await _CatalogoSeccionService.ObtenXId(parametro.IdSeccion);
            if (seccion.Id <= 0)
            {
                return resultado;
            }
            var rolNoIdentity = await _RolService.ObtenXId(parametro.IdRol);
            if (rolNoIdentity.Id <= 0)
            {
                return resultado;
            }
            var rolEmpresa = await _RolEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            if (rolEmpresa.Count() <= 0)
            {
                return resultado;
            }
            var empresa = await _EmpresaService.ObtenXId(rolEmpresa[0].IdEmpresa);
            if (empresa.Id <= 0)
            {
                return resultado;
            }
            var rolIdentity = await zvRoleManager.FindByIdAsync(rolNoIdentity.IdAspNetRole);
            if (rolIdentity == null)
            {
                return resultado;
            }
            var claimsRol = await zvRoleManager.GetClaimsAsync(rolIdentity);
            Claim claimAgregar;
            if (seccion.EsSeccionUnica)
            {
                claimAgregar = new Claim(seccion.DescripcionInterna!, "EsSeccionUnica");
            }
            else
            {
                claimAgregar = new Claim(seccion.DescripcionInterna + "-" + empresa.Id, seccion.CodigoSeccion + "-" + empresa.GuidEmpresa);
            }
            var claimMenuEmpresa = claimsRol.Where(z => z.Type == claimAgregar.Type).ToList();
            if (claimMenuEmpresa.Count() <= 0)
            {
                var resultadoAddClaim = await zvRoleManager.AddClaimAsync(rolIdentity, claimAgregar);
            }
            RolSeccionDTO rolMenuSeccion;
            var existeRolMenuSeccion = await _RolSeccionService.ObtenTodosXIdSeccion(seccion.Id);
            existeRolMenuSeccion = existeRolMenuSeccion.Where(z => z.IdRol == rolNoIdentity.Id).ToList();
            if (existeRolMenuSeccion.Count() <= 0)
            {
                resultado = await _RolSeccionService.CrearYObtener(new RolSeccionDTO()
                {
                    IdRol = rolNoIdentity.Id,
                    IdSeccion = seccion.Id
                });
            }
            //Como uno o varios usuarios estan ligados a un rol se le agregan a los usuarios el nuevo permiso
            var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdRol(rolNoIdentity.Id);
            foreach (var usuarioNoIdentity in usuarioEmpresa)
            {
                var usuario = await _UsuarioService.ObtenXIdUsuario(usuarioNoIdentity.IdUsuario);
                var usuarioIdentity = await zvUserManager.FindByIdAsync(usuario.IdAspNetUser!);
                if (usuarioIdentity == null)
                {
                    continue;
                }
                var claimsUsuario = await zvUserManager.GetClaimsAsync(usuarioIdentity!);
                var claimsUsuarioFiltradoActividad = claimsUsuario.Where(z => z.Type == claimAgregar.Type).ToList();
                if (claimsUsuarioFiltradoActividad.Count() <= 0)
                {
                    var resultado3 = await zvUserManager.AddClaimAsync(usuarioIdentity, claimAgregar);
                }
                var resultado2 = await _UsuarioSeccionService.ActivarPermisoUsuario(new UsuarioSeccionDTO()
                {
                    IdUsuario = usuarioNoIdentity.IdUsuario,
                    EsActivo = true,
                    IdSeccion = seccion.Id,
                    IdEmpresa = usuarioNoIdentity.IdEmpresa,
                });
            }
            return resultado;
        }

        public async Task<RespuestaDTO> asignarPermisosProyectoAnterior(UsuarioProyectoDTO parametros)
        {
            var respuesta = new RespuestaDTO();
            var asignarProyecto = await _UsuarioProyectoService.CrearYObtener(parametros);

            var rolesEmpresa = await _rolEmpresaService.ObtenXIdEmpresa(parametros.IdEmpresa);
            var secciones = await _catalogoSeccionService.ObtenTodosXIdMenu(2);
            var roles = new List<RolEmpresaDTO>();

            foreach (var rol in rolesEmpresa)
            {
                foreach (var seccion in secciones)
                {
                    var seccionXRol = await _rolSeccionService.ObtenTodosXIdRolXIdSeccion(rol.IdRol, seccion.Id);
                    if (seccionXRol.Count > 0)
                    {
                        roles.Add(rol);
                    }
                }
            }

            if (roles.Count <= 0)
            {
                //crear el rol y sus relaciones
                var nuevoRol = new RolCreacionUnaEmpresaDTO();
                nuevoRol.Nombre = "Gestion de Presupuestos";
                nuevoRol.IdEmpresa = parametros.IdEmpresa;

                var crearRol = await ProcesoCrearRolEnEmpresa(nuevoRol);
                if (crearRol.Id != 0) {
                    foreach (var seccion in secciones) {
                        var nuevoRolSeccion = new RolSeccionDTO();
                        nuevoRolSeccion.IdRol = crearRol.Id;
                        nuevoRolSeccion.IdSeccion = seccion.Id;
                        var rolSeccion = await autorizaSeccionARol(nuevoRolSeccion);
                    }
                }
            }
            else
            {
                foreach (var rol in roles)
                {
                    var NuevaRelacion = new RolProyectoEmpresaUsuarioDTO();
                    NuevaRelacion.IdProyecto = parametros.IdProyecto;
                    NuevaRelacion.IdEmpresa = parametros.IdEmpresa;
                    NuevaRelacion.IdRol = rol.IdRol;
                    NuevaRelacion.IdUsuario = parametros.IdUsuario;
                    NuevaRelacion.Estatus = true;
                    var NuevaRelacionCreada = await _rolProyectoEmpresaUsuarioService.Crear(NuevaRelacion);
                }
            }

            return respuesta;
        }

        public async Task<RespuestaDTO> asignarRolDefault(UsuarioProyectoDTO parametros, List<System.Security.Claims.Claim> claims)
        {
            var respuesta = new RespuestaDTO();

            var usuarioNombre = claims.Where(z => z.Type == "username").ToList();

            var usuario = await _UsuarioService.ObtenXUsername(usuarioNombre[0].Value);


            var empresaXUsuario = await _UsuarioEmpresaService.ObtenXIdUsuarioXIdEmpresa(usuario.Id, parametros.IdEmpresa);
            if (empresaXUsuario.Count() <= 0) {
                respuesta.Estatus = false;
                respuesta.Descripcion = "La empresa no se ha asignado al usuario";
                return respuesta;
            }

            var NuevaRelacion = new RolProyectoEmpresaUsuarioDTO();
            NuevaRelacion.IdProyecto = parametros.IdProyecto;
            NuevaRelacion.IdEmpresa = parametros.IdEmpresa;
            NuevaRelacion.IdRol = empresaXUsuario[0].IdRol;
            NuevaRelacion.IdUsuario = usuario.Id;
            NuevaRelacion.Estatus = true;
            var NuevaRelacionCreada = await _rolProyectoEmpresaUsuarioService.Crear(NuevaRelacion);

            if (NuevaRelacionCreada.Id <= 0) {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se asigno el rol";
                return respuesta;
            }

            respuesta.Estatus = true;
            respuesta.Descripcion = "Se asigno el rol";
            return respuesta;
        }
    }
}
