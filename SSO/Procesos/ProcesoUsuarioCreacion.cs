using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using System.Security.Claims;

namespace SistemaERP.BLL.ProcesoSSO
{
    public class ProcesoUsuarioCreacion
    {
        private readonly ICorporativoService _CorporativoService;
        private readonly IEmpresaService _EmpresaService;
        private readonly UserManager<IdentityUser> _UsuarioManager;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioProveedorService _UsuarioProveedorService;
        private readonly IUsuarioGastosService _UsuarioGastosService;
        private readonly IUsuarioAutoFacService _UsuarioAutoFacService;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteAClienteService;

        public ProcesoUsuarioCreacion(
            ICorporativoService CorporativoService
            , IEmpresaService EmpresaService
            , UserManager<IdentityUser> UserManager
            , IUsuarioService UsuarioService
            , IUsuarioCorporativoService UsuarioCorporativoService
            , IUsuarioProveedorService UsuarioProveedorService
            , IUsuarioGastosService UsuarioGastosService
            , IUsuarioAutoFacService UsuarioAutoFacService
            , IUsuarioPertenecienteAClienteService UsuarioPertenecienteAClienteService
            )
        {
            _CorporativoService = CorporativoService;
            _EmpresaService = EmpresaService;
            _UsuarioManager = UserManager;
            _UsuarioService = UsuarioService;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _UsuarioProveedorService = UsuarioProveedorService;
            _UsuarioPertenecienteAClienteService = UsuarioPertenecienteAClienteService;
            _UsuarioGastosService = UsuarioGastosService;
            _UsuarioAutoFacService = UsuarioAutoFacService;
        }

        public async Task<UsuarioBaseDTO> CreaUsuarioBase(UsuarioCreacionBaseDTO parametro)
        {
            if (string.IsNullOrEmpty(parametro.NombreCompleto) || string.IsNullOrEmpty(parametro.NombreUsuario)
                || string.IsNullOrEmpty(parametro.Correo)
                || string.IsNullOrEmpty(parametro.Password))
            {
                return new UsuarioDTO();
            }
            var usuarioNuevoIdentity = new IdentityUser { Email = parametro.Correo, UserName = parametro.NombreUsuario };
            var resultado = await _UsuarioManager.CreateAsync(usuarioNuevoIdentity, parametro.Password);
            if (resultado.Succeeded)
            {
                var usuarioIdentity = await _UsuarioManager.FindByIdAsync(usuarioNuevoIdentity.Id);
                if (usuarioIdentity == null)
                {
                    return new UsuarioDTO();
                }
                //Creamos el usuario
                var usuarioNoIdentity = await _UsuarioService.CrearYObtener(new UsuarioDTO()
                {
                    NombreCompleto = parametro.NombreCompleto,
                    Apaterno = parametro.APaterno,
                    Amaterno = parametro.AMaterno,
                    NombreUsuario = parametro.NombreUsuario,
                    Correo = parametro.Correo,
                    Activo = true,
                    IdAspNetUser = usuarioIdentity!.Id
                });

                return usuarioNoIdentity;
            }
            else
            {
                return new UsuarioDTO();
            }
        }
        public async Task<bool> CreaUsuarioPertenecienteACliente(UsuarioPertenecienteAClienteDTO parametro)
        {
            if (parametro.IdUsuario <= 0 || parametro.IdCorporativo <= 0)
            {
                return false;
            }
            var resultado = await _UsuarioPertenecienteAClienteService.CrearYObtener(parametro);
            if (resultado.Id > 0)
            {
                return true;
            }
            return false;
        }
        
        public async Task<bool> CreaUsuarioNormal(UsuarioCreacionBaseDTO2 parametro, List<System.Security.Claims.Claim> claims)
        {
            var username = claims.Where(z => z.Type == "username").ToList();
            int IdCorporativo = 0;
            var usuario = await _UsuarioService.ObtenXUsername(username[0].Value);
            var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
            if (usuarioCorporativo.Id > 0)
            {
                IdCorporativo = usuarioCorporativo.IdCorporativo;
            }
            else
            {
                var UsuarioPerteneciente = await _UsuarioPertenecienteAClienteService.ObtenXIdUsuario(usuario.Id);
                IdCorporativo = UsuarioPerteneciente.IdCorporativo;
            }
            if (IdCorporativo <= 0)
            {
                return false;
            }
            var resultado1 = await CreaUsuarioBase(parametro);
            if (resultado1.Id <= 0)
            {
                return false;
            }
            var resultado2 = await CreaUsuarioPertenecienteACliente(new UsuarioPertenecienteAClienteDTO()
            {
                IdUsuario = resultado1.Id,
                IdCorporativo = IdCorporativo
            });
            return true;
        }
        /// <summary>
        /// Proceso de los principales
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public async Task<bool> CreaRelacionUsuarioCorporativo(UsuarioCorporativoCreacionDTO parametro)
        {
            if (parametro.IdCorporativo <= 0)
            {
                return false;
            }
            var corporativo = await _CorporativoService.ObtenXId(parametro.IdCorporativo);
            var usuario = await CreaUsuarioBase(parametro);
            if (usuario.Id > 0)
            {
                var usuarioIdentity = await _UsuarioManager.FindByIdAsync(usuario.IdAspNetUser!);
                if (usuarioIdentity != null && corporativo.Id > 0)
                {
                    var resultadoUsuarioManager = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim("VisorCorporativo", "Codigo-J05tg8!"));
                    var resultadoUsuarioCorporativo = await _UsuarioCorporativoService.Crear(new UsuarioCorporativoDTO()
                    {
                        IdUsuario = usuario.Id,
                        IdCorporativo = parametro.IdCorporativo
                    });
                    if (resultadoUsuarioCorporativo && resultadoUsuarioManager.Succeeded)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        public async Task<UsuarioBaseDTO> CreaRelacionUsuarioEmpresaProveedor(UsuarioProveedorCreacionDTO parametro, int IdCorporativo)
        {
            if ( IdCorporativo <= 0)
            {
                return new UsuarioDTO();
            }
            if (parametro.Rfc.ToLower() != "xexx010101000" && parametro.Rfc.ToLower() != "xaxx010101000")
            {
                var existeRfc = await _UsuarioProveedorService.ObtenXRfc(parametro.Rfc);
                if (existeRfc.Id > 0)
                {
                    return new UsuarioDTO();
                }
            }
            var usuario = await CreaUsuarioBase(parametro);
            if (usuario.Id > 0)
            {
                var usuarioPertenecienteACliente = await CreaUsuarioPertenecienteACliente(new UsuarioPertenecienteAClienteDTO()
                {
                    IdUsuario = usuario.Id,
                    IdCorporativo = IdCorporativo
                });
                var usuarioIdentity = await _UsuarioManager.FindByIdAsync(usuario.IdAspNetUser!);
                var usuarioProveedor = await _UsuarioProveedorService.Crear(new UsuarioProveedorDTO()
                {
                    IdUsuario = usuario.Id,
                    Rfc = parametro.Rfc,
                    IdentificadorFiscal = parametro.IdentificadorFiscal,
                    NumeroProveedor = parametro.NumeroProveedor
                });
                var resultadoUsuarioManager = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim("Proveedor", "3sUsu4r10Pr0v33d0r"));

                return usuario;
            }
            return new UsuarioDTO();
        }

        public async Task<bool> CreaUsuarioGastos(UsuarioGastosEnVariasEmpresasCreacionDTO parametro, int IdCorporativo)
        {
            if (IdCorporativo <= 0)
            {
                return false;
            }
            var usuario = await CreaUsuarioBase(parametro);
            var usuarioPertenecienteACliente = await CreaUsuarioPertenecienteACliente(new UsuarioPertenecienteAClienteDTO()
            {
                IdUsuario = usuario.Id,
                IdCorporativo = IdCorporativo
            });
            if (usuario.Id > 0)
            {
                var usuarioIdentity = await _UsuarioManager.FindByIdAsync(usuario.IdAspNetUser!);
                var usuarioGastos = await _UsuarioGastosService.Crear(new UsuarioGastosDTO()
                {
                    idUsuario = usuario.Id,
                    nombre = parametro.NombreCompleto,
                    apellidoPaterno = parametro.APaterno,
                    apellidoMaterno = parametro.AMaterno,
                    numeroEmpleado = parametro.numeroEmpleado,
                    estatus = 1,
                    fecha_alta = DateTime.Now,
                    fecha_baja = null,

                });
                var resultadoUsuarioManager = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim("UGastos", "3sUsu4r10G45t05"));

                return true;
            }
            return false;
        }
        public async Task<bool> CreaUsuarioAutoFac(UsuarioAutoFacEnVariasEmpresasCreacionDTO parametro, int IdCorporativo)
        {
            try
            {
                if (IdCorporativo <= 0)
                {
                    return false;
                }
                var usuario = await CreaUsuarioBase(parametro);
                var usuarioPertenecienteACliente = await CreaUsuarioPertenecienteACliente(new UsuarioPertenecienteAClienteDTO()
                {
                    IdUsuario = usuario.Id,
                    IdCorporativo = IdCorporativo
                });
                if (usuario.Id > 0)
                {
                    var usuarioIdentity = await _UsuarioManager.FindByIdAsync(usuario.IdAspNetUser!);
                    var usuarioAutoFac = await _UsuarioAutoFacService.Crear(new UsuarioAutoFacDTO()
                    {
                        idUsuario = usuario.Id,
                        nombre = parametro.NombreCompleto,
                        apellidoPaterno = parametro.APaterno,
                        apellidoMaterno = parametro.AMaterno,
                        estatus = 1,
                        fechaAlta = DateTime.Now,
                        fechaBaja = null,

                    });
                    var resultadoUsuarioManager = await _UsuarioManager.AddClaimAsync(usuarioIdentity, new Claim("UAutoFac", "3sUsu4r104ut0f4c"));

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }


    }
}
