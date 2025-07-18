
using SSO.Modelos;
using System;
using System.Collections.Generic;

namespace GuardarArchivos.DTO
{

    public class Archivo
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Ruta { get; set; } = null!;

        public int Tamaño { get; set; }

        public bool Borrado { get; set; }
        public virtual ICollection<ParametrosTimbrado> ArchivoCer { get; set; } = new List<ParametrosTimbrado>();

        public virtual ICollection<ParametrosTimbrado> ArchivoKey { get; set; } = new List<ParametrosTimbrado>();

    }
}