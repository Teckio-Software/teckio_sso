using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO.Menu;

namespace SistemaERP.BLL.SubProcesoSSO
{
    /// <summary>
    /// Para tener permisos de un rol dentro de un corporativo
    /// </summary>
    public class PermisosARolCorporativo
    {
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IRolSeccionService _RolSeccionService;
        private readonly IRolActividadService _RolActividadService;
        /// <summary>
        /// Administrador de los roles, para agregar, editar, eliminar o ver los roles
        /// </summary>
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly UserManager<IdentityUser> _UsuarioManager;
        /// <summary>
        /// Constructor de las cuentas controller
        /// </summary>
        /// <param name="UserManager">Administrador de los usuarios, para agregar, editar, eliminar o ver los usuarios</param>
        /// <param name="RoleManager">Administrador de los roles, para agregar, editar, eliminar o ver los roles</param>
        /// <param name="zConfiguration">Para acceder a la configuración de app.settings</param>
        /// <param name="zSignInManager">Para loguearse en el software</param>
        /// <param name="zContext">Para obtener los usuario, roles y claims de un usuario</param>
        /// <param name="zMapper">Convierte los registros nativos del dbContext a los DTO para enviar información</param>
        public PermisosARolCorporativo(
            RoleManager<IdentityRole> RoleManager
            , UserManager<IdentityUser> UsuarioManager
            , IEmpresaService EmpresaService
            , IRolService RolService
            , IRolEmpresaService RolEmpresaService
            , IRolSeccionService RolSeccionService
            , IRolActividadService RolActividadService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            )
        {
            _RoleManager = RoleManager;
            _UsuarioManager = UsuarioManager;
            _EmpresaService = EmpresaService;
            _RolService = RolService;
            _RolEmpresaService = RolEmpresaService;
            _RolSeccionService = RolSeccionService;
            _RolActividadService = RolActividadService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
        }
        /// <summary>
        /// Esto es para agregar los permisos 
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="listaSecciones"></param>
        /// <returns></returns>
        public async Task<bool> AgregaSeccionesARolCorporativo(IdentityRole rol, List<CatalogoSeccionConMenuDTO> listaSecciones, List<int> listaEmpresas)
        {
            var rolNoIdentity = await _RolService.ObtenXIdAspNetRol(rol.Id);
            if (rolNoIdentity.Count() <= 0)
            {
                return false;
            }
            var secciones = await _CatalogoSeccionService.ObtenTodos();
            for (int i = 0; i < listaSecciones.Count(); i++)
            {
                var seccionFiltrada = secciones.Where(z => z.Id == listaSecciones[i].Id).ToList();
                var claims = await _RoleManager.GetClaimsAsync(rol);
                if (seccionFiltrada.Count() > 0)
                {
                    for (int j = 0; j < listaEmpresas.Count(); j++)
                    {
                        string permisoArmado = seccionFiltrada[0].CodigoSeccion + "-" + listaEmpresas[j];
                        var claimFiltrado = claims.Where(z => z.Type == permisoArmado).ToList();
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Este es para declarar un rol con sus actividades a cada empresa dentro de un corporativo
        /// </summary>
        /// <param name="rol"></param>
        /// <param name="listaActividades"></param>
        /// <param name="listaEmpresas"></param>
        /// <returns></returns>
        public async Task<bool> AgregaActividadesARolCorporativo(IdentityRole rol, List<int> listaActividades, List<int> listaEmpresas)
        {
            var secciones = await _CatalogoActividadService.ObtenTodos();
            for (int i = 0; i < listaActividades.Count(); i++)
            {
                var seccionFiltrada = secciones.Where(z => z.Id == listaActividades[i]).ToList();
                var claims = await _RoleManager.GetClaimsAsync(rol);
                if (seccionFiltrada.Count() > 0)
                {
                    for (int j = 0; j < listaEmpresas.Count(); j++)
                    {
                        string permisoArmado = seccionFiltrada[0].CodigoActividad + "-" + listaEmpresas[j];
                        var claimFiltrado = claims.Where(z => z.Type == permisoArmado).ToList();
                    }
                }
            }
            return true;
        }
    }
}
