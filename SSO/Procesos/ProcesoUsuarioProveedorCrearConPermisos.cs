using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.BLL.SubProcesoSSO;
using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using SSO.DTO;

namespace SistemaERP.BLL.ProcesoSSO
{
    public class ProcesoUsuarioProveedorCrearConPermisos
    {
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IRolSeccionService _RolSeccionService;
        private readonly IRolActividadService _RolActividadService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IUsuarioSeccionService _UsuarioSeccionService;
        private readonly IUsuarioActividadService _UsuarioActividadService;
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly ObtenEmpresasEnRoles _EmpresasEnRoles;
        private readonly ProcesoRelacionUsuarioPermisosPorEmpresa _RelacionUsuarioEmpresaPermisos;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<IdentityUser> _UserManager;
        public ProcesoUsuarioProveedorCrearConPermisos(
            RoleManager<IdentityRole> RoleManager
            , UserManager<IdentityUser> UserManager
            , IEmpresaService EmpresaService
            , IRolService RolService
            , IRolEmpresaService RolEmpresaService
            , IRolSeccionService RolSeccionService
            , IRolActividadService RolActividadService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IUsuarioEmpresaService UsuarioEmpresaService
            , IUsuarioSeccionService UsuarioSeccionService
            , IUsuarioActividadService UsuarioActividadService
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , ObtenEmpresasEnRoles EmpresasEnRoles
            , ProcesoRelacionUsuarioPermisosPorEmpresa RelacionUsuarioEmpresaPermisos
            )
        {
            _RoleManager = RoleManager;
            _UserManager = UserManager;
            _EmpresaService = EmpresaService;
            _RolService = RolService;
            _RolEmpresaService = RolEmpresaService;
            _RolSeccionService = RolSeccionService;
            _RolActividadService = RolActividadService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _UsuarioSeccionService = UsuarioSeccionService;
            _UsuarioActividadService = UsuarioActividadService;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _EmpresasEnRoles = EmpresasEnRoles;
            _RelacionUsuarioEmpresaPermisos = RelacionUsuarioEmpresaPermisos;
        }
        /// <summary>
        /// Para la creación de un rol 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<RespuestaDTO> CrearUsuarioProveedorConPermisos(UsuarioProveedorCreacionConPermisosDTO parametro)
        {
            RespuestaDTO resp = new RespuestaDTO();
            resp.Estatus = false;
            //Aquí creamos al usuario proveedor perteneciente a un corporativo
            var resultadoUsuario = await _ProcesoUsuarioCreacion.CreaRelacionUsuarioEmpresaProveedor(parametro, parametro.IdCorporativo);
            if (resultadoUsuario.Id <= 0)
            {
                resp.Descripcion = "Algo salió mal en la creación del usuario";
                return resp;
            }
            var roles = await _RolService.ObtenTodos();
            var rolesEmpresa = await _RolEmpresaService.ObtenTodos();
            var rolesSecciones = await _RolSeccionService.ObtenTodos();
            var rolesActividades = await _RolActividadService.ObtenTodos();
            var secciones = await _CatalogoSeccionService.ObtenTodos();
            var actividades = await _CatalogoActividadService.ObtenTodos();
            var empresas = await _EmpresaService.ObtenXIdCorporativo(parametro.IdCorporativo);
            for (int i = 0; i < parametro.ListaEmpresas.Count(); i++)
            {
                var resultadoRol = await _EmpresasEnRoles.EncuentraIdRolXOtroIdRol(parametro.ListaEmpresas[i].IdEmpresa, parametro.ListaEmpresas[i].IdRol);
                if (resultadoRol.Id <= 0)
                {
                    continue;
                }
                var resultado1 = await _UsuarioEmpresaService.Activar(new UsuarioEmpresaDTO()
                {
                    IdEmpresa = parametro.ListaEmpresas[i].IdEmpresa,
                    IdRol = resultadoRol.Id,
                    IdUsuario = resultadoUsuario.Id,
                    Activo = true
                });
                var rolSeccionesFiltrado = rolesSecciones.Where(z => z.IdRol == resultadoRol.Id).ToList();
                var empresaActual = empresas.Where(z => z.Id == parametro.ListaEmpresas[i].IdEmpresa).ToList();
                var usuarioIdentity = await _UserManager.FindByIdAsync(resultadoUsuario.IdAspNetUser!);
                for (int j = 0; j < rolSeccionesFiltrado.Count(); j++)
                {
                    var seccionActual = secciones.Where(z => z.Id == rolSeccionesFiltrado[i].IdSeccion).ToList();
                    var resultadoSecciones = await _RelacionUsuarioEmpresaPermisos.CreaRelacionUsuarioSecciones(resultadoUsuario.Id, empresaActual[0], seccionActual[0], usuarioIdentity!);
                }
                var rolActividadesFiltrado = rolesActividades.Where(z => z.IdRol == resultadoRol.Id).ToList();
                for (int j = 0; j < rolActividadesFiltrado.Count(); j++)
                {
                    var actividadActual = actividades.Where(z => z.Id == rolActividadesFiltrado[i].IdActividad).ToList();
                    var resultadoSecciones = await _RelacionUsuarioEmpresaPermisos.CreaRelacionUsuarioActividades(resultadoUsuario.Id, empresaActual[0], actividadActual[0], usuarioIdentity!);
                }
            }
            resp.Estatus = true;
            resp.Descripcion = "Usuario proveedor creado con exito";
            return resp;
        }
    }
}
