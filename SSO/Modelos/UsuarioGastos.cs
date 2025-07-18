namespace SistemaERP.Model.Gastos
{
    public class UsuarioGastos
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string  nombre {get; set;}
        public string apellidoPaterno {  get; set; }
        public string? apellidoMaterno {get; set; }
        public int estatus { get; set; }
        public DateTime fecha_alta  {get; set;}
        public DateTime? fecha_baja { get; set; }
        public string? numeroEmpleado { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
    }
}
