using SistemaERP.DTO.Menu;
using SSO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.SSO
{

    public class EmpresaDTO
    {
        public int Id { get; set; }
        public string? NombreComercial { get; set; } = null!;
        public string? Rfc { get; set; } = null!;
        public string? CodigoPostal { get; set; } = "";
        public bool? Estatus { get; set; }
        public int IdCorporativo { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? Sociedad { get; set; }
        public string? GuidEmpresa { get; set; }
        public CertKeyDTO? CertificadoKey {get;set;}
    }
    /// <summary>
    /// De la empresa resultan las divisiones o sucursales
    /// </summary>
    public class DivisionDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public int IdEmpresa { get; set; }
    }

    public class EmpresaFiltradoDTO
    {
        public string? seccionFiltro { get; set; }
        public string? usuarioFiltro { get; set; }
    }

    public class EmpresaCreacionRolesDTO
    {
        public int IdEmpresa { get; set; }
        public List<CatalogoSeccionConMenuDTO> Secciones { get; set; }
    }
}
