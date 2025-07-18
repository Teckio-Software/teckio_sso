using Microsoft.AspNetCore.Identity;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ServicioSSO;
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
    /// <summary>
    /// Para la busqueda de los usuarios dependiendo de las necesidades
    /// </summary>
    public class ProcesoUsuariosBusqueda
    {
        private readonly IUsuarioPertenecienteAClienteService _UsuarioPertenecienteService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IUsuarioCorporativoService _UsuarioCorporativoService;
        private readonly IUsuarioProveedorService _UsuarioProveedorService;
        private readonly IUsuarioGastosService _UsuarioGastosService;

        public ProcesoUsuariosBusqueda(
            IUsuarioPertenecienteAClienteService UsuarioPertenecienteService
            , IUsuarioService UsuarioService
            , IUsuarioCorporativoService UsuarioCorporativoService
            , IUsuarioProveedorService UsuarioProveedorService
            , IUsuarioGastosService UsuarioGastosService
            )
        {
            _UsuarioPertenecienteService = UsuarioPertenecienteService;
            _UsuarioService = UsuarioService;
            _UsuarioCorporativoService = UsuarioCorporativoService;
            _UsuarioProveedorService = UsuarioProveedorService;
            _UsuarioGastosService = UsuarioGastosService;
        }

        public async Task<List<UsuarioProveedorConsultaDTO>> ObtenUsuariosProveedores(List<Claim> Claims)
        {
            List<UsuarioProveedorConsultaDTO> usuariosProveedores = new List<UsuarioProveedorConsultaDTO>();
            var username = Claims.Where(z => z.Type == "username").ToList();
            if (username.Count() <= 0)
            {
                return usuariosProveedores;
            }
            var usuario = await _UsuarioService.ObtenXUsername(username[0].Value);
            if (usuario.Id <= 0)
            {
                return usuariosProveedores;
            }
            var usuariosAwait = await _UsuarioService.ObtenTodos();
            var usuariosProveedorAwait = await _UsuarioProveedorService.ObtenTodos();
            int idCorporativo = 0;
            var usuarioPertenecienteACliente = await _UsuarioPertenecienteService.ObtenXIdUsuario(usuario.Id);
            if (usuarioPertenecienteACliente.Id <= 0)
            {
                var usuarioCorporativo = await _UsuarioCorporativoService.ObtenXIdUsuario(usuario.Id);
                if (usuarioCorporativo.Id <= 0)
                {
                    return usuariosProveedores;
                }
                idCorporativo = usuarioCorporativo.IdCorporativo;
            }
            else
            {
                idCorporativo = usuarioPertenecienteACliente.IdCorporativo;
            }
            var usuariosPertenecientesAClientes = await _UsuarioPertenecienteService.ObtenXIdCliente(idCorporativo);
            for (int i = 0; i < usuariosPertenecientesAClientes.Count(); i++)
            {
                var usuarioActual = usuariosAwait.Where(z => z.Id == usuariosPertenecientesAClientes[i].IdUsuario).ToList();
                if (usuarioActual.Count() <= 0)
                {
                    continue;
                }
                var usuarioActualProveedor = usuariosProveedorAwait.Where(z => z.IdUsuario == usuarioActual[0].Id).ToList();
                if (usuarioActualProveedor.Count() <= 0)
                {
                    continue;
                }
                usuariosProveedores.Add(new UsuarioProveedorConsultaDTO()
                {
                    IdAspNetUser = usuarioActual[0].IdAspNetUser,
                    NombreCompleto = usuarioActual[0].NombreCompleto,
                    NombreUsuario = usuarioActual[0].NombreUsuario,
                    Rfc = usuarioActualProveedor[0].Rfc,
                    IdentificadorFiscal = usuarioActualProveedor[0].IdentificadorFiscal
                });
            }
            return usuariosProveedores;
        }
        public async Task<UsuarioGastosDTO> ObtenerUsuarioGastosxIdUsuario(int idUsuario)
        {
            UsuarioGastosDTO usuarioGastos = new UsuarioGastosDTO();

            var usuario = await _UsuarioGastosService.ObtenXIdUsuario(idUsuario);
            usuarioGastos = usuario;

            return usuarioGastos;


        }
    }
}
