namespace SistemaERP.Model;

public partial class Division
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdEmpresa { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    public virtual ICollection<UsuarioDivision> UsuarioDivisions { get; set; } = new List<UsuarioDivision>();
    
}
