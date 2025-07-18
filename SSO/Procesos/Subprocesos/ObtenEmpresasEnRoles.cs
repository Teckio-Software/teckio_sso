using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.SubProcesoSSO
{
    /// <summary>
    /// Para obtener las empresas que tienen los roles
    /// </summary>
    public class ObtenEmpresasEnRoles
    {
        private readonly ICorporativoService _CorporativoService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IRolService _RolService;
        private readonly IRolEmpresaService _RolEmpresaService;
        private readonly IRolSeccionService _RolSeccionService;
        private readonly IRolActividadService _RolActividadService;
        private readonly EstructuraServicios _EstructuraServicios;
        private readonly UserManager<IdentityUser> zvUserManager;
        private readonly RoleManager<IdentityRole> zvRoleManager;
        public ObtenEmpresasEnRoles(
            UserManager<IdentityUser> zUserManager
            , RoleManager<IdentityRole> zRoleManager
            , ICorporativoService CorporativoService
            , IEmpresaService EmpresaService
            , IRolService RolService
            , IRolSeccionService RolSeccionService
            , IRolActividadService RolActividadService
            , IRolEmpresaService RolEmpresaService
            , EstructuraServicios EstructuraServicios
            )
        {
            zvUserManager = zUserManager;
            zvRoleManager = zRoleManager;
            _EmpresaService = EmpresaService;
            _RolService = RolService;
            _RolSeccionService = RolSeccionService;
            _RolActividadService = RolActividadService;
            _RolEmpresaService = RolEmpresaService;
            _CorporativoService = CorporativoService;
            _EstructuraServicios = EstructuraServicios;
        }
        /// <summary>
        /// Para la vista de las empresas que tienen relación con un rol dentro de un corporativo
        /// </summary>
        /// <param name="idCorporativo"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public async Task<List<RolesActivosEnEmpresaDTO>> ObtenEmpresasConRolActivo(int idCorporativo, int idRol)
        {
            var corporativo = await _CorporativoService.ObtenXId(idCorporativo);
            var empresas = await _EmpresaService.ObtenXIdCorporativo(corporativo.Id);
            var estructuraDeServicios = await _EstructuraServicios.ObtenEstructuraSerivicios();
            var rolesSecciones = await _RolSeccionService.ObtenTodos();
            var rolesActividades = await _RolActividadService.ObtenTodos();
            List<RolesActivosEnEmpresaDTO> rolesAMostrar = new List<RolesActivosEnEmpresaDTO>();
            var rol = await _RolService.ObtenXId(idRol);
            var rolesConMismoNombre = await _RolService.ObtenXNombre(rol.Nombre);
            var rolesEmpresas = await _RolEmpresaService.ObtenTodos();
            for (int i = 0; i < empresas.Count(); i++)
            {
                var rolesFiltradoEmpresa = rolesEmpresas.Where(z => z.IdEmpresa == empresas[i].Id).ToList();
                List<CatalogoSeccionConMenuDTO> estructuraServiciosCopia = await _EstructuraServicios.ObtenEstructuraSerivicios();
                int existeRolEnEmpresaConMismoNombre = 0;
                for (int j = 0; j < rolesFiltradoEmpresa.Count(); j++)
                {
                    var rolesFiltradoEmpresaNombre = rolesConMismoNombre.Where(z => z.Id == rolesFiltradoEmpresa[j].IdRol).ToList();
                    if (rolesFiltradoEmpresaNombre.Count() > 0)
                    {
                        existeRolEnEmpresaConMismoNombre = existeRolEnEmpresaConMismoNombre + 1;
                    }
                    for (int l = 0; l < estructuraServiciosCopia.Count(); l++)
                    {
                        if (rolesFiltradoEmpresaNombre.Count() > 0)
                        {
                            var rolesSeccionesFiltrado = rolesSecciones.Where(z => z.IdRol == rolesFiltradoEmpresaNombre[0].Id).ToList();
                            rolesSeccionesFiltrado = rolesSeccionesFiltrado.Where(z => z.IdSeccion == estructuraServiciosCopia[l].Id).ToList();
                            if (rolesSeccionesFiltrado.Count() > 0)
                            {
                                estructuraServiciosCopia[l].EsSeleccionado = true;
                            }
                            for (int k = 0; k < estructuraServiciosCopia[l].Actividades.Count(); k++)
                            {
                                var rolesActividadesFiltrado = rolesActividades.Where(z => z.IdRol == rolesFiltradoEmpresaNombre[0].Id)
                                    .Where(z => z.IdActividad == estructuraServiciosCopia[l].Actividades[k].Id).ToList();
                                if (rolesActividadesFiltrado.Count() > 0)
                                {
                                    estructuraServiciosCopia[l].Actividades[k].EsSeleccionado = true;
                                }
                            }
                        }
                    }
                }
                var rolesNombreFiltrado = rolesFiltradoEmpresa.Where(z => z.IdEmpresa == empresas[i].Id).ToList();
                bool existeConMismoNombre = (rolesNombreFiltrado.Count() > 0 && existeRolEnEmpresaConMismoNombre > 0) ? true : false;
                rolesAMostrar.Add(new RolesActivosEnEmpresaDTO()
                {
                    EsActivoEnEmpresa = existeConMismoNombre,
                    IdEmpresa = empresas[i].Id,
                    NombreEmpresa = empresas[i].NombreComercial,
                    Secciones = estructuraServiciosCopia
                });
            }
            return rolesAMostrar;
        }
        /// <summary>
        /// Para obtener el rol verdadero de la empresa a la que selecciona dentro de un mismo corporativo
        /// </summary>
        /// <param name="idEmpresa"></param>
        /// <param name="idRol"></param>
        /// <returns></returns>
        public async Task<RolDTO> EncuentraIdRolXOtroIdRol(int idEmpresa, int idRol)
        {
            var empresas = await _EmpresaService.ObtenXIdCorporativo(idEmpresa);
            var rol = await _RolService.ObtenXId(idRol);
            var rolesConMismoNombre = await _RolService.ObtenXNombre(rol.Nombre);
            var rolesEmpresas = await _RolEmpresaService.ObtenXIdEmpresa(idEmpresa);
            for (int i = 0; i < rolesEmpresas.Count(); i++)
            {
                var primerFiltro = rolesConMismoNombre.Where(z => z.Id == rolesEmpresas[i].IdRol).ToList();
                if (primerFiltro.Count() > 0)
                {
                    return primerFiltro[0];
                }
            }
            return new RolDTO();
        }
    }
}
