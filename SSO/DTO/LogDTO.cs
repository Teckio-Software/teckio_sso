using SistemaERP.Model;

namespace SSO.DTO
{
    public class LogDTO
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Nivel { get; set; } = null!;

        public string Metodo { get; set; } = null!;

        public string? Descripcion { get; set; }

        public string DbContext { get; set; } = null!;

        public int IdUsuario { get; set; }

        public string? NombreUsuario { get; set; }

        public int IdEmpresa { get; set; }

        public string? NombreEmpresa { get; set; }

    }
}
