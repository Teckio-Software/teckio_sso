using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO.Menu
{
    /// <summary>
    /// Esto es lo equivalente a los sistemas (proveedores, gastos, nomina, contabilidad)
    /// </summary>
    public class CatalogoMenuDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string CodigoMenu { get; set; }
    }
    /// <summary>
    /// Relación de las actividades (Para el ing. Luis), para las vistas del usuario
    /// </summary>
    public class CatalogoSeccionDTO
    {
        public int Id { get; set; }
        public int IdMenu { get; set; }
        public string Descripcion { get; set; }
        public string CodigoSeccion { get; set; }
        public string? DescripcionInterna { get; set; }
        public bool EsSeccionUnica { get; set; }
    }
    /// <summary>
    /// Actividades o acciones derivadas de las secciones
    /// </summary>
    public class CatalogoActividadDTO
    {
        public int Id { get; set; }
        public int IdSeccion { get; set; }
        public string Descripcion { get; set; }
        public string CodigoActividad { get; set; }
        public string? DescripcionInterna { get; set; }
        public bool EsActividadUnica { get; set; }
    }
    /// <summary>
    /// Actividades o acciones derivadas de las secciones
    /// </summary>
    public class CatalogoActividadParaRolDTO : CatalogoActividadDTO
    {
        public bool EsSeleccionado { get; set; }
    }
    /// <summary>
    /// Para mostrar las secciones con los menus
    /// </summary>
    public class CatalogoSeccionConMenuDTO : CatalogoSeccionDTO
    {
        public string DescripcionMenu { get; set; }
        public bool EsSeleccionado { get; set; }
        public List<CatalogoActividadParaRolDTO> Actividades { get; set; }
    }
}
