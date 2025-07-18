using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.SSO
{
    /// <summary>
    /// Para crear un usuario con relación a proveedor
    /// </summary>
    public class UsuarioProveedorCreacionDTO : UsuarioCreacionBaseDTO
    {
        public string Rfc { get; set; }
        public string NumeroProveedor { get; set; }
        public string IdentificadorFiscal { get; set; }
    }
    /// <summary>
    /// Relación del usuario con la información del proveedor
    /// </summary>
    public class UsuarioProveedorDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Rfc { get; set; }
        public string NumeroProveedor { get; set; }
        public string IdentificadorFiscal { get; set; }
    }

    public class UsuarioProveedorConsultaDTO : UsuarioBaseDTO
    {
        public string Rfc { get; set; }
        public string IdentificadorFiscal { get; set; }
        public string NumeroProveedor { get; set; }
    }
    /// <summary>
    /// Para la creación de un usuario proveedor con permisos
    /// </summary>
    public class UsuarioProveedorCreacionConPermisosDTO : UsuarioProveedorCreacionDTO {
        public List<UsuarioRolEmpresaPermisosCreacionDTO> ListaEmpresas { get; set; }
        public int IdCorporativo { get; set; }
    }
}
