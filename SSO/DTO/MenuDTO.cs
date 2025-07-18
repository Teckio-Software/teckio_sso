using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO
{



    public class MenuEstructuraDTO
    {
        public int IdMenu { get; set; }
        public int IdSeccion { get; set; }
        public int IdActividad { get; set; }
        /// <summary>
        /// 1 = Menú, 2 = Sección, 3 = Actividad
        /// </summary>
        public int TipoMenu { get; set; }
        public string Descripcion { get; set; }
        public bool EsAutorizado { get; set; }
        public List<MenuEstructuraDTO> Estructura { get; set; }
    }
    /// <summary>
    /// Para la activación de los menus a una empresa
    /// </summary>
    public class AutorizaMenuDTO
    {
        public int IdEmpresa { get; set; }
        public int IdMenu { get; set; }
    }
    /// <summary>
    /// Para la activación de las secciones a una empresa
    /// </summary>
    public class AutorizaSeccionDTO
    {
        public int IdEmpresa { get; set; }
        public int IdMenu { get; set; }
        public int IdSeccion { get; set; }
    }
}
