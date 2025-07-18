using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.Menu
{
    internal class PermisosMenuMultiEmpresaDTO
    {
    }
    

    /// <summary>
    /// Para la creación de un rol en una empresa específica con los ids de los menus, secciones y actividades
    /// </summary>
    public class UsuarioRolCreacionEnEmpresaDTO
    {
        public string? NombreCompleto { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Correo { get; set; }
        public string? Password { get; set; }
        public string? Rfc { get; set; }
        public bool EsActivo { get; set; }
        public string? IdAspNetUser { get; set; }
        public string? NumeroProveedor { get; set; }
        public string? IdentificadorFiscal { get; set; }
        public List<int> ListaIdRoles { get; set; }
    }
    /// <summary>
    /// Para cambiar el rol de un usuario en una empresa
    /// </summary>
    public class CambiarRolAUsuarioEnEmpresaDTO
    {
        /// <summary>
        /// Identificador único del usuario
        /// </summary>
        public int IdUsuario { get; set; }
        /// <summary>
        /// Identificador único del nuevo rol (No identity)
        /// </summary>
        public int IdRol { get; set; }
        public int IdEmpresa { get; set; }
    }
    public class AsignarRolAUsuarioEnEmpresaPorPoryectoDTO: CambiarRolAUsuarioEnEmpresaDTO
    {
        public int IdProyecto { get; set; }
    }
    /// <summary>
    /// Para quitar o agregar un menu a un usuario dependiendo de su rol por empresa
    /// </summary>
    public class CambiaPermisoMenu
    {
        /// <summary>
        /// Identificador de la empresa
        /// </summary>
        public int IdEmpresa { get; set; }
        /// <summary>
        /// Identificador del usuario (No identity)
        /// </summary>
        public int IdUsuario { get; set; }
        /// <summary>
        /// Identificador del menu (pertenciente a la empresa)
        /// </summary>
        public int IdMenu { get; set; }
    }
    /// <summary>
    /// Para quitar o agregar una seccion a un usuario dependiendo de su rol por empresa
    /// </summary>
    public class CambiaPermisoSeccion
    {
        /// <summary>
        /// Identificador de la empresa
        /// </summary>
        public int IdEmpresa { get; set; }
        /// <summary>
        /// Identificador del usuario (No identity)
        /// </summary>
        public int IdUsuario { get; set; }
        /// <summary>
        /// Identificador de la sección
        /// </summary>
        public int IdSeccion { get; set; }
    }
    /// <summary>
    /// Para quitar o agregar una actividad a un usuario dependiendo de su rol por empresa
    /// </summary>
    public class CambiaPermisoActividad
    {
        /// <summary>
        /// Identificador de la empresa
        /// </summary>
        public int IdEmpresa { get; set; }
        /// <summary>
        /// Identificador del usuario (No identity)
        /// </summary>
        public int IdUsuario { get; set; }
        /// <summary>
        /// Identificador de la actividad de la empresa
        /// </summary>
        public int IdActividad { get; set; }
    }
}
