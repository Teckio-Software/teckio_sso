using SistemaERP.Model;

namespace SSO.Modelos
{
    public class RolProyectoEmpresaUsuario
    {
        public int Id { get; set; }
        public int IdProyecto { get; set; }
        public int IdRol { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public bool Estatus { get; set; }
        public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

        public virtual Rol IdRolNavigation { get; set; } = null!;
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
