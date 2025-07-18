using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SistemaERP.DTO.SSO;
using SistemaERP.Model;
using System.Security.Claims;

namespace SistemaERP.BLL.ProcesoSSO
{
    public class ProcesoRelacionUsuarioPermisosPorEmpresa
    {
        private readonly UserManager<IdentityUser> _UsuarioManager;
        private readonly IUsuarioSeccionService _UsuarioSeccionService;
        private readonly IUsuarioActividadService _UsuarioActividadService;
        public ProcesoRelacionUsuarioPermisosPorEmpresa(
            UserManager<IdentityUser> UserManager
            , IUsuarioSeccionService UsuarioSeccionService
            , IUsuarioActividadService UsuarioActividadService
        )
        {
            _UsuarioManager = UserManager;
            _UsuarioSeccionService = UsuarioSeccionService;
            _UsuarioActividadService = UsuarioActividadService;
        }

        /// <summary>
        /// Proceso de los principales
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<bool> CreaRelacionUsuarioSecciones(int IdUsuario, EmpresaDTO empresa, CatalogoSeccionDTO seccion, IdentityUser usuarioIdentity)
        {
            if (seccion.Id <= 0 || IdUsuario <= 0 || empresa.Id <= 0 || usuarioIdentity == null)
            {
                return false;
            }
            var estaActivo = await _UsuarioSeccionService.ObtenXIdUsuarioXIdEmpresa(IdUsuario, empresa.Id);
            estaActivo = estaActivo.Where(z => z.IdSeccion == seccion.Id).ToList();
            if (estaActivo.Count() > 0)
            {
                var resultado3 = await _UsuarioSeccionService.ActivarPermisoUsuario(estaActivo[0]);
                var resultado4 = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim(seccion.DescripcionInterna + "-" + empresa.Id, seccion.CodigoSeccion + "-" + empresa.GuidEmpresa));
                if (resultado4.Succeeded == true && resultado3.Estatus == true)
                {
                    return true;
                }
                return false;
            }
            var resultado1 = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim(seccion.DescripcionInterna + "-" + empresa.Id, seccion.CodigoSeccion + "-" + empresa.GuidEmpresa));
            var resultado2 = await _UsuarioSeccionService.ActivarPermisoUsuario(new UsuarioSeccionDTO()
            {
                IdUsuario = IdUsuario,
                IdSeccion = seccion.Id,
                IdEmpresa = empresa.Id,
                EsActivo = true
            });
            if (resultado1.Succeeded == true && resultado2.Estatus == true)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Proceso de los principales
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<bool> CreaRelacionUsuarioActividades(int IdUsuario, EmpresaDTO empresa, CatalogoActividadDTO actividad, IdentityUser usuarioIdentity)
        {
            if (actividad.Id <= 0 || IdUsuario <= 0 || empresa.Id <= 0 || usuarioIdentity == null)
            {
                return false;
            }
            var estaActivo = await _UsuarioActividadService.ObtenXIdUsuarioXIdEmpresa(IdUsuario, empresa.Id);
            estaActivo = estaActivo.Where(z => z.IdActividad == actividad.Id).ToList();
            if (estaActivo.Count() > 0)
            {
                var resultado3 = await _UsuarioActividadService.ActivarPermisoUsuario(estaActivo[0]);
                var resultado4 = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim(actividad.DescripcionInterna + "-" + empresa.Id, actividad.CodigoActividad + "-" + empresa.GuidEmpresa));
                if (resultado4.Succeeded == true && resultado3.Estatus == true)
                {
                    return true;
                }
                return false;
            }
            var resultado1 = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim(actividad.DescripcionInterna + "-" + empresa.Id, actividad.CodigoActividad + "-" + empresa.GuidEmpresa));
            var resultado2 = await _UsuarioActividadService.ActivarPermisoUsuario(new UsuarioActividadDTO()
            {
                IdUsuario = IdUsuario,
                IdActividad = actividad.Id,
                IdEmpresa = empresa.Id,
                EsActivo = true
            });
            if (resultado1.Succeeded == true && resultado2.Estatus == true)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Para la parte de los permisos especiales con el usuario con su sección por empresa
        /// </summary>
        /// <param name="IdUsuario"></param>
        /// <param name="empresa"></param>
        /// <param name="seccion"></param>
        /// <param name="usuarioIdentity"></param>
        /// <returns></returns>
        public async Task<bool> QuitaRelacionUsuarioSecciones(int IdUsuario, EmpresaDTO empresa, CatalogoSeccionDTO seccion, IdentityUser usuarioIdentity)
        {
            if (seccion.Id <= 0 || IdUsuario <= 0 || empresa.Id <= 0 || usuarioIdentity == null)
            {
                return false;
            }
            var estaActivo = await _UsuarioSeccionService.ObtenXIdUsuarioXIdEmpresa(IdUsuario, empresa.Id);
            estaActivo = estaActivo.Where(z => z.IdSeccion == seccion.Id).ToList();
            if (estaActivo.Count() > 0)
            {
                var resultado3 = await _UsuarioSeccionService.DesactivarPermisoUsuario(estaActivo[0].Id);
                var claimsUsuarioIdentity = await _UsuarioManager.GetClaimsAsync(usuarioIdentity);
                var claimsUsuarioIdentityFiltrado = claimsUsuarioIdentity.Where(z => z.Type == seccion.DescripcionInterna + "-" + empresa.Id).ToList();
                if (claimsUsuarioIdentityFiltrado.Count() > 0)
                {
                    var resultado4 = await _UsuarioManager.RemoveClaimsAsync(usuarioIdentity, claimsUsuarioIdentityFiltrado);
                    if (resultado4.Succeeded == true && resultado3.Estatus == true)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public async Task<bool> QuitaRelacionUsuarioActividades(int IdUsuario, EmpresaDTO empresa, CatalogoActividadDTO actividad, IdentityUser usuarioIdentity)
        {
            if (actividad.Id <= 0 || IdUsuario <= 0 || empresa.Id <= 0 || usuarioIdentity == null)
            {
                return false;
            }
            var estaActivo = await _UsuarioActividadService.ObtenXIdUsuarioXIdEmpresa(IdUsuario, empresa.Id);
            estaActivo = estaActivo.Where(z => z.IdActividad == actividad.Id).ToList();
            if (estaActivo.Count() > 0)
            {
                var resultado3 = await _UsuarioActividadService.DesactivarPermisoUsuario(estaActivo[0].Id);
                var claimsUsuarioIdentity = await _UsuarioManager.GetClaimsAsync(usuarioIdentity);
                var claimsUsuarioIdentityFiltrado = claimsUsuarioIdentity.Where(z => z.Type == actividad.DescripcionInterna + "-" + empresa.Id).ToList();
                if (claimsUsuarioIdentityFiltrado.Count() > 0)
                {
                    var resultado4 = await _UsuarioManager.RemoveClaimsAsync(usuarioIdentity, claimsUsuarioIdentityFiltrado);
                    if (resultado4.Succeeded == true && resultado3.Estatus == true)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
    }
}
