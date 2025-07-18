using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO.SSO;
using System.Security.Claims;

namespace SistemaERP.BLL.ProcesoSSO
{
    /// <summary>
    /// Para la relación de un usuario con la empresa
    /// </summary>
    public class ProcesoRelacionUsuarioEmpresa
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RolManager;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioEmpresaService _UsuarioEmpresaService;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteClienteService;

        public ProcesoRelacionUsuarioEmpresa(
            UserManager<IdentityUser> UserManager
            , RoleManager<IdentityRole> RoleManager
            , IUsuarioService UsuarioService
            , IUsuarioEmpresaService UsuarioEmpresaService
            , IEmpresaService EmpresaService
            , IUsuarioCorporativoService UsuarioCorporativoService
            , IUsuarioPertenecienteAClienteService UsuarioPertenecienteClienteService
            )
        {
            _UserManager = UserManager;
            _RolManager = RoleManager;
            _UsuarioService = UsuarioService;
            _UsuarioEmpresaService = UsuarioEmpresaService;
            _EmpresaService = EmpresaService;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _UsuarioPertenecienteClienteService = UsuarioPertenecienteClienteService;
        }
        /// <summary>
        /// Para obtener las empresas a las que puede acceder dependiendo si esta en una u otra
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task<List<EmpresaDTO>> ObtenEmpresasPertenecientes(List<Claim> claims)
        {
            var guid = claims.Where(z => z.Type == "guid").ToList();
            List <EmpresaDTO> empresas = new List<EmpresaDTO>();
            if (guid.Count() <= 0)
            {
                return empresas;
            }
            var usuarioNoidentity = await _UsuarioService.ObtenXGuidUsuario(guid[0].Value);
            if (usuarioNoidentity.Id <= 0)
            {
                return new List<EmpresaDTO>();
            }
            if (!usuarioNoidentity.Activo)
            {
                return new List<EmpresaDTO>();
            }
            var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuarioNoidentity.Id);
            if (usuarioCorporativo.Id > 0)
            {
                var empresasCorporativo = await _EmpresaService.ObtenXIdCorporativo(usuarioCorporativo.IdCorporativo);
                return empresasCorporativo;
            }
            var usuarioCliente = await _UsuarioPertenecienteClienteService.ObtenXIdUsuario(usuarioNoidentity.Id);
            if (usuarioCliente.Id <= 0)
            {
                return empresas;
            }
            var empresas2 = await _EmpresaService.ObtenXIdCorporativo(usuarioCliente.IdCorporativo);

            var usuarioEmpresa = await _UsuarioEmpresaService.ObtenXIdUsuario(usuarioNoidentity.Id);
            usuarioEmpresa = usuarioEmpresa.Where(z => z.Activo == true).ToList();
            foreach (var empresa in usuarioEmpresa)
            {
                var empresasFiltrado = empresas2.Where(z => z.Id == empresa.IdEmpresa).ToList();
                var existeYaEnEmpresa = empresas.Where(z => z.Id == empresa.IdEmpresa).ToList();
                if (empresasFiltrado.Count() > 0 && existeYaEnEmpresa.Count() <= 0)
                {
                    empresas.Add(empresasFiltrado[0]);
                }
            }
            return empresas;
        }
    }
}
