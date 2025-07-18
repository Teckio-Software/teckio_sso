using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.SubProcesoSSO;
using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ProcesoSSO
{
    public class ProcesoUsuarioCorporativo
    {
        private readonly ProcesoUsuarioCreacion _ProcesoUsuarioCreacion;
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RolManager;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly ICorporativoService _CorporativoService;

        public ProcesoUsuarioCorporativo(
            UserManager<IdentityUser> UserManager
            , RoleManager<IdentityRole> RoleManager
            , ProcesoUsuarioCreacion ProcesoUsuarioCreacion
            , IUsuarioService UsuarioService
            , IUsuarioCorporativoService UsuarioCorporativoService
            , ICorporativoService CorporativoService
            )
        {
            _UserManager = UserManager;
            _RolManager = RoleManager;
            _UsuarioService = UsuarioService;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _ProcesoUsuarioCreacion = ProcesoUsuarioCreacion;
            _CorporativoService = CorporativoService;
        }
        /// <summary>
        /// Dependiendo de los claims y de la relación es como va a mostrar los corporativos que le pertenecen a un usuario o si puede ver todos los corporativos
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task<List<CorporativoDTO>> ObtenCorporativosPertenecientes(List<Claim> claims)
        {
            var usernameClaim = claims.Where(z => z.Type == "username").ToList();
            var esActivoClaim = claims.Where(z => z.Type == "activo").ToList();
            var esRfacilClaim = claims.Where(z => z.Value == "Administrador").ToList();
            List<CorporativoDTO> corporativos = new List<CorporativoDTO>();
            if (esRfacilClaim.Count() > 0)
            {
                corporativos = await _CorporativoService.ObtenTodos();
                return corporativos;
            }
            else
            {
                var usuario = await _UsuarioService.ObtenXUsername(usernameClaim[0].Value);
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                var corporativo = await _CorporativoService.ObtenXId(usuarioCorporativo.IdCorporativo);
                if (corporativo.Id > 0)
                {
                    corporativos.Add(corporativo);
                }
                return corporativos;
            }
        }

        /// <summary>
        /// Para la edición de los usuarios corporativos
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<bool> EditaUsuarioCorporativo(UsuarioCorporativoEdicionDTO parametro)
        {
            if (parametro.IdCorporativo <= 0)
            {
                return false;
            }
            var corporativo = await _CorporativoService.ObtenXId(parametro.IdCorporativo);
            var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(parametro.Id);
            if (usuarioCorporativo.Id > 0)
            {
                var usuario = await _UsuarioService.ObtenXIdUsuario(parametro.Id);
                var usuarioIdentity = await _UserManager.FindByIdAsync(usuario.IdAspNetUser!);
                if (usuarioIdentity != null && corporativo.Id > 0)
                {
                    var resultadoUsuarioCorporativo = await _UsuarioCorporativoService.Crear(new UsuarioCorporativoDTO()
                    {
                        IdUsuario = usuario.Id,
                        IdCorporativo = parametro.IdCorporativo
                    });
                    if (resultadoUsuarioCorporativo)
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
