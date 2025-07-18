namespace SSO.DTO
{
    public class EmpleadoGastosDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string? numeroEmpleadoSAP { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string? apellidoMaterno { get; set; }
        public int estatus { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime? fecha_baja { get; set; }
        public string? seguroSocial { get; set; }
        public string? rfc { get; set; }
        public string? curp { get; set; }
        public string? codigoPostal { get; set; }
        public string? numeroEmpleado { get; set; }
        public DateTime? fechaRelacionLaboral { get; set; }
        public Decimal? salarioDiario { get; set; }
        public int? claveContrato { get; set; }
        public int? claveRegimen { get; set; }
        public int? claveJornada { get; set; }
        public int? claveRiesgoPuesto { get; set; }
        public int? claveEstado { get; set; }
        public string? nombreContrato { get; set; }
        public string? nombreRegimen { get; set; }
        public string? nombrejornada { get; set; }
        public string? nombreRiesgo { get; set; }
        public string? nombreEstado { get; set; }
    }
}
