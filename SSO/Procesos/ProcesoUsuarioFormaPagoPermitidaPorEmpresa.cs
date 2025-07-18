using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ProcesoSSO
{
    public class ProcesoUsuarioFormaPagoPermitidaPorEmpresa
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioProveedorFormasPagoXEmpresaService _UsuarioProveedorFormasPagoService;

        public ProcesoUsuarioFormaPagoPermitidaPorEmpresa(
            UserManager<IdentityUser> UserManager
            , IUsuarioService UsuarioService
            , IUsuarioProveedorFormasPagoXEmpresaService UsuarioProveedorFormasPagoService
            )
        {
            _UserManager = UserManager;
            _UsuarioService = UsuarioService;
            _UsuarioProveedorFormasPagoService = UsuarioProveedorFormasPagoService;
        }

        public async Task<bool> CreaRelacionUsuarioFormaPagoEmpresa(UsuarioFormasPagoXEmpresaDTO parametro)
        {
            bool resultadoEditar = false;
            var relacionUsuario = await _UsuarioProveedorFormasPagoService.ObtenXIdUsuarioEIdEmpresa(parametro.IdUsuario, parametro.IdEmpresa);
            if (relacionUsuario.Id <= 0)
            {
                relacionUsuario.IdUsuario = parametro.IdUsuario;
                relacionUsuario.IdEmpresa = parametro.IdEmpresa;
                relacionUsuario.EsPpd = parametro.EsPpd;
                relacionUsuario.EsPue = parametro.EsPue;
                relacionUsuario = await _UsuarioProveedorFormasPagoService.CrearYObtener(relacionUsuario);
            }
            else
            {
                parametro.Id = relacionUsuario.Id;
                resultadoEditar = await _UsuarioProveedorFormasPagoService.Editar(parametro);
            }
            if (relacionUsuario.Id <= 0)
            {
                return false;
            }
            var usuarioNoidentity = await _UsuarioService.ObtenXIdUsuario(parametro.IdUsuario);
            var usuarioIdentity = await _UserManager.FindByIdAsync(usuarioNoidentity.IdAspNetUser!);
            var claimsUsuarioIdentity = await _UserManager.GetClaimsAsync(usuarioIdentity!);
            var tieneClaimPueEmpresa = claimsUsuarioIdentity.Where(z => z.Type.ToLower() == "pue-" + parametro.IdEmpresa).ToList();
            bool resultadoPueManagerBool = await CambiaPermisosPueAUsuario(usuarioIdentity, parametro.EsPue, parametro.IdEmpresa);
            bool resultadoPpdManagerBool = await CambiaPermisosPpdAUsuario(usuarioIdentity, parametro.EsPpd, parametro.IdEmpresa);
            if (resultadoPpdManagerBool == true && resultadoPueManagerBool == true && (resultadoEditar == true || relacionUsuario.Id > 0))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CambiaPermisosPueAUsuario(IdentityUser usuarioIdentity, bool esPpd, int idEmpresa)
        {
            bool resultadoPueManagerBool = false;
            var claimsUsuarioIdentity = await _UserManager.GetClaimsAsync(usuarioIdentity!);
            var claimEliminar = claimsUsuarioIdentity.Where(z => z.Type.ToLower() == "pue-" + idEmpresa).ToList();
            if (!esPpd && claimEliminar.Count() > 0)
            {
                var resultadoEliminarPue = await _UserManager.RemoveClaimAsync(usuarioIdentity, claimEliminar[0]);
                resultadoPueManagerBool = resultadoEliminarPue.Succeeded;
            }
            else if(esPpd && claimEliminar.Count() <= 0)
            {
                var resultadoEliminarPue = await _UserManager.AddClaimAsync(usuarioIdentity, new Claim("Pue-" + idEmpresa, "1"));
                resultadoPueManagerBool = resultadoEliminarPue.Succeeded;
            }
            if (resultadoPueManagerBool)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CambiaPermisosPpdAUsuario(IdentityUser usuarioIdentity, bool esPpd, int idEmpresa)
        {
            bool resultadoPueManagerBool = false;
            var claimsUsuarioIdentity = await _UserManager.GetClaimsAsync(usuarioIdentity!);
            var claimEliminar = claimsUsuarioIdentity.Where(z => z.Type.ToLower() == "ppd-" + idEmpresa).ToList();
            if (!esPpd && claimEliminar.Count() > 0)
            {
                var resultadoEliminarPue = await _UserManager.RemoveClaimAsync(usuarioIdentity, claimEliminar[0]);
                resultadoPueManagerBool = resultadoEliminarPue.Succeeded;
            }
            else if (esPpd && claimEliminar.Count() <= 0)
            {
                var resultadoEliminarPue = await _UserManager.AddClaimAsync(usuarioIdentity, new Claim("Ppd-" + idEmpresa, "1"));
                resultadoPueManagerBool = resultadoEliminarPue.Succeeded;
            }
            if (resultadoPueManagerBool)
            {
                return true;
            }
            return false;
        }
    }
}
