using GuardarArchivos.DTO;
using SistemaERP.Model;

namespace SSO.Modelos
{
    public class ParametrosTimbrado
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int IdArchivoCer { get; set; }

        public int IdArchivoKey { get; set; }
        public string KeyPassword { get; set; }

        public DateTime? FechaExpedicion { get; set; }

        public DateTime? FechaVigencia { get; set; }

        public int Predeterminado { get; set; }

        public int Estatus { get; set; }

        public virtual Archivo? ContenidoCER { get; set; } = null!;

        public virtual Archivo? ContenidoKEY { get; set; } = null!;

        public virtual Empresa? Empresa { get; set; } = null!;

    }
}
