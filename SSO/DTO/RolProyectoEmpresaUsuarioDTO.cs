namespace SSO.DTO
{
    public class RolProyectoEmpresaUsuarioDTO
    {
        public int Id { get; set; }
        public int IdProyecto { get; set; }
        public int IdRol {  get; set; }
        public string DescripcionRol {  get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public bool Estatus {  get; set; }
    }
}
