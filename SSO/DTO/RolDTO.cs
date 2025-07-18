using SistemaERP.DTO.Menu;
using SistemaERP.DTO.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO
{
    public class RolDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string IdAspNetRole { get; set; }
    }
    /// <summary>
    /// Relación del rol con la sección
    /// </summary>
    public class RolSeccionDTO
    {
        public int Id { get; set; }
        public int IdSeccion { get; set; }
        public int IdRol { get; set; }
    }
    /// <summary>
    /// Relación del rol con la actividad
    /// </summary>
    public class RolActividadDTO
    {
        public int Id { get; set; }
        public int IdRolSeccion { get; set; }
        public int IdActividad { get; set; }
        public int IdRol { get; set; }
    }
    /// <summary>
    /// Relación del rol con la empresa
    /// </summary>
    public class RolEmpresaDTO
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public int IdEmpresa { get; set; }
    }
    /// <summary>
    /// Para la creación de un rol en una empresa específica con los ids de los menus, secciones y actividades
    /// </summary>
    public class RolCreacionUnaEmpresaDTO
    {
        /// <summary>
        /// Nombre del rol
        /// </summary>
        public string? Nombre { get; set; }
        public int IdEmpresa { get; set; }
    }
    public class RolCreacionVarisEmpresasMismoCorporativoDTO
    {
        /// <summary>
        /// Nombre del rol
        /// </summary>
        public string? Nombre { get; set; }
        public List<CatalogoSeccionConMenuDTO> Secciones { get; set; }
        public List<EmpresaCreacionRolesDTO> Empresas { get; set; }
        public int IdCorporativo { get; set; }
    }

    public class RolMenuEstructuraDTO
    {
        //public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public int IdMenu { get; set; }
        public int IdSeccion { get; set; }
        public int IdActividad { get; set; }
        /// <summary>
        /// 1 = rol, 2 = menú, 3 = sección, 4 = actividad, 
        /// </summary>
        public int TipoMenu { get; set; }
        /// <summary>
        /// Descripción del rol, menú, sección, actividad
        /// </summary>
        public string Descripcion { get; set; }
        public bool EsActivo { get; set; }
        public List<RolMenuEstructuraDTO> Estructura { get; set; }
    }
    /// <summary>
    /// Para la vista de los roles existentes en cada empresa
    /// </summary>
    public class RolesActivosEnEmpresaDTO
    {
        public string NombreEmpresa { get; set; }
        public bool EsActivoEnEmpresa { get; set; }
        public int IdEmpresa { get; set; }
        public List<CatalogoSeccionConMenuDTO> Secciones { get; set; }
    }
}
