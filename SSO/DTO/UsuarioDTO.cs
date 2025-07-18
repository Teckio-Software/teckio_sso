


namespace SistemaERP.DTO
{
    public abstract class UsuarioBaseDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public bool Activo { get; set; }
        public string? IdAspNetUser { get; set; }
        public string? Apaterno { get; set; }
        public string? Amaterno { get; set; }
    }
    /// <summary>
    /// Principal, tabla de los usuarios
    /// </summary>
    public class UsuarioDTO : UsuarioBaseDTO
    {
    }
    /// <summary>
    /// Relación del usuario con la vista (Actividad para el Ing. Luis)
    /// </summary>
    public class UsuarioSeccionDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdSeccion { get; set; }
        public int IdEmpresa { get; set; }
        public bool EsActivo { get; set; }
    }
    /// <summary>
    /// Relación del usuario con la actividad (Accion o derivados de la actividad para el Ing. Luis)
    /// </summary>
    public class UsuarioActividadDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdActividad { get; set; }
        public int IdEmpresa { get; set; }
        public bool EsActivo { get; set; }
    }
    /// <summary>
    /// Relación del usuario con el corporativo
    /// </summary>
    public class UsuarioCorporativoDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdCorporativo { get; set; }
    }
    /// <summary>
    /// Para la relación entre el usuario y la empresa
    /// </summary>
    public class UsuarioEmpresaDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }
        public bool Activo { get; set; }
    }
    public class UsuarioEmpresaPorDefectoDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdUsuario { get; set; }
    }
    public class UsuarioPertenecienteAClienteDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdCorporativo { get; set; }
    }
    /// <summary>
    /// Más para gastos, relación del usuario con la sucursal o división
    /// </summary>
    public class UsuarioDivisionDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdDivision { get; set; }
    }
    
    /// <summary>
    /// Relación del usuario con el sistema de gastos
    /// </summary>
    public class UsuarioGastosDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string? apellidoMaterno { get; set; }
        public int estatus { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime? fecha_baja { get; set; }
        public string numeroEmpleado { get; set; }
    }
    /// <summary>
    /// Relación del usuario con el sistema de AutoFacturacion
    /// </summary>
    public class UsuarioAutoFacDTO
    {
        public int id { get; set; }
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string? apellidoMaterno { get; set; }
        public int estatus { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime? fechaBaja { get; set; }
    }
    public class UsuarioGastosConsultaDTO : UsuarioBaseDTO
    {
        public string numeroEmpleado { get; set; }
        public int estatus { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime? fecha_baja { get; set; }
    }
    public class UsuarioAutoFacConsultaDTO : UsuarioBaseDTO
    {
        public int estatus { get; set; }
        public DateTime fechaAlta { get; set; }
        public DateTime? fechaBaja { get; set; }
    }
    /// <summary>
    /// De teckio, relación del usuario con su proyecto
    /// </summary>
    public class UsuarioProyectoDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public bool Estatus { get; set; }
    }

    public class UsuarioProyectoParametrosBusquedaDTO
    {
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
    }

    /// <summary>
    /// Relación del usuario con la clasificación del CFDI
    /// </summary>
    public class UsuarioClasificacionCfdiDTO
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdClasificacionCfdi { get; set; }
    }




    /// <summary>
    /// Para la creación de un usuario nuevo
    /// </summary>
    public abstract class UsuarioCreacionBaseDTO
    {
        public string NombreCompleto { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    public class UsuarioCreacionBaseDTO2 : UsuarioCreacionBaseDTO
    {
    }

    public class UsuarioCorporativoCreacionDTO : UsuarioCreacionBaseDTO
    {
        public int IdCorporativo { get; set; }
    }
    public class UsuarioCorporativoEdicionDTO : UsuarioBaseDTO
    {
        public int IdCorporativo { get; set; }
    }

    public class UsuarioSeccionesYActividadesCreacionDTO : UsuarioCreacionBaseDTO
    {
        public List<int> ListaIdSecciones { get; set; }
        public List<int> ListaIdActividades { get; set; }
    }
    
    public class UsuarioGastosEnVariasEmpresasCreacionDTO : UsuarioCreacionBaseDTO
    {
        public int estatus { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime? fecha_baja { get; set; }
        public string numeroEmpleado { get; set; }
    }
    public class UsuarioAutoFacEnVariasEmpresasCreacionDTO : UsuarioCreacionBaseDTO
    {
        public int estatus { get; set; }
        public DateTime fecha_alta { get; set; }
        public DateTime? fecha_baja { get; set; }
    }

    public class UsuarioProyectoCreacionDTO : UsuarioCreacionBaseDTO
    {
        public int IdEmpresa { get; set; }
        public int IdProyecto { get; set; }
    }

    public class UsuarioRolMenuEstructuraDTO
    {
        public int IdUsuario { get; set; }
        public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public int IdMenu { get; set; }
        public int IdSeccion { get; set; }
        public int IdActividad { get; set; }
        /// <summary>
        /// 1 = usuario, 2 = rol/empresa, 3 = menú, 4 = sección, 5 = actividad
        /// </summary>
        public int TipoMenu { get; set; }
        public string Descripcion { get; set; } //Se usa el rol y no el nombre de la empresa
        public bool EsActivo { get; set; }
        public List<UsuarioRolMenuEstructuraDTO> Estructura { get; set; }
    }
    /// <summary>
    /// Según YO es para mostrar los tipos de usuarios existentes en el corporativo y el tipo de usuario que se muestra
    /// </summary>
    public class UsuarioEstructuraCorporativoDTO : UsuarioBaseDTO
    {
        public int IdUsuario { get; set; }
        public bool EsActivo { get; set; }
        public int Tipo { get; set; }
        public string Rfc { get; set; }
    }
    public class UsuarioEmpresaEstructura
    {
        public int IdEmpresa { get; set; }
        public int IdRol { get; set; }
        public string NombreEmpresa { get; set; }
        public bool ActivoEmpresa { get; set; }
        public List<RolDTO> Roles { get; set; }

        #region Fuera de tabla en base

            public bool esUsuarioGastos { get; set; }

        #endregion
    }


    public class UsuariosUltimaSeccionDTO
    {
        public int Id { get; set; }

        public int IdProyecto { get; set; }

        public int IdEmpresa { get; set; }

        public int IdUsuario { get; set; }
    }
}
