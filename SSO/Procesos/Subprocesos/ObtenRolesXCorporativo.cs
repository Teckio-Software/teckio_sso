using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.SubProcesoSSO
{
    /// <summary>
    /// Para obtener el equivalente en roles
    /// </summary>
    public class ObtenRolesXCorporativo
    {
        private readonly IEmpresaService _EmpresaService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IRolSeccionService _RolSeccionService;
        private readonly IRolActividadService _RolActividadService;
        /// <summary>
        /// Administrador de los usuarios, para agregar, editar, eliminar o ver los usuarios
        /// </summary>
        private readonly UserManager<IdentityUser> zvUserManager;
        /// <summary>
        /// Administrador de los roles, para agregar, editar, eliminar o ver los roles
        /// </summary>
        private readonly RoleManager<IdentityRole> zvRoleManager;
        /// <summary>
        /// Constructor de las cuentas controller
        /// </summary>
        /// <param name="zUserManager">Administrador de los usuarios, para agregar, editar, eliminar o ver los usuarios</param>
        /// <param name="zRoleManager">Administrador de los roles, para agregar, editar, eliminar o ver los roles</param>
        /// <param name="zConfiguration">Para acceder a la configuración de app.settings</param>
        /// <param name="zSignInManager">Para loguearse en el software</param>
        /// <param name="zContext">Para obtener los usuario, roles y claims de un usuario</param>
        /// <param name="zMapper">Convierte los registros nativos del dbContext a los DTO para enviar información</param>
        public ObtenRolesXCorporativo(
            UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , IEmpresaService EmpresaService
            , IRolService RolService
            , IRolSeccionService RolSeccionService
            , IRolActividadService RolActividadService
            , IRolEmpresaService RolEmpresaService
            )
        {
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            _EmpresaService = EmpresaService;
            _RolService = RolService;
            _RolSeccionService = RolSeccionService;
            _RolActividadService = RolActividadService;
            _RolEmpresaService = RolEmpresaService;
        }
        /// <summary>
        /// Obtiene los roles existentes en el corporativo (es una vista)
        /// </summary>
        /// <param name="idCorporativo"></param>
        /// <returns></returns>
        public async Task<List<RolDTO>> ProcesoObtenRolesXCorporativo(int idCorporativo)
        {
            var empresas = await _EmpresaService.ObtenXIdCorporativo(idCorporativo);
            var roles = await _RolService.ObtenTodos();
            var rolesEmpresas = await _RolEmpresaService.ObtenTodos();
            var rolesDeCorporativo = new List<RolDTO>();
            var rolesDeCorporativoInt = new List<int>();
            var rolesEmpresasFiltradas = new List<RolEmpresaDTO>();
            for (int i = 0; i < empresas.Count(); i++)
            {
                var rolesEmpresasFiltrado = rolesEmpresas.Where(z => z.IdEmpresa == empresas[i].Id).ToList();
                rolesEmpresasFiltradas.AddRange(rolesEmpresasFiltrado);
            }
            var rolesEmpresasAgrupadas = rolesEmpresasFiltradas.GroupBy(z => z.IdRol).ToList();
            for (int i = 0; i < rolesEmpresasAgrupadas.Count(); i++)
            {
                rolesDeCorporativoInt.Add(rolesEmpresasAgrupadas[i].Key);
            }
            for (int i = 0; i < rolesDeCorporativoInt.Count(); i++)
            {
                var rolFiltrado = roles.Where(z => z.Id == rolesDeCorporativoInt[i]).ToList();
                if (rolFiltrado.Count() > 0)
                {
                    var rolesDeCorporativoFiltrado = rolesDeCorporativo.Where(z => z.Nombre.ToLower() == rolFiltrado[0].Nombre.ToLower());
                    if (rolesDeCorporativoFiltrado.Count() <= 0)
                    {
                        rolesDeCorporativo.Add(rolFiltrado[0]);
                    }
                }
            }
            return rolesDeCorporativo;
        }
    }
}
