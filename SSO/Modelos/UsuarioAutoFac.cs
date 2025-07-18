namespace SistemaERP.Model.Gastos
{
    public class UsuarioAutoFac
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string  nombre {get; set;}
        public string apellidoPaterno {  get; set; }
        public string? apellidoMaterno {get; set; }
        public int estatus { get; set; }
        public DateTime fechaAlta  {get; set;}
        public DateTime? fechaBaja { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
