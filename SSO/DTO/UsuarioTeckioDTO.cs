using System.ComponentModel.DataAnnotations;

namespace SistemaERP.DTO
{

    #region JWT
    /// <summary>
    /// Para devolver la autenticación de un usuario con un toke y su fecha de expiración
    /// </summary>
    public class RespuestaAutenticacionDTO
    {
        /// <summary>
        /// Token temporal para ingresar al software
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// Fecha de expiración del token para entrar al sistema
        /// </summary>
        public DateTime FechaExpiracion { get; set; }
    }
    /// <summary>
    /// Para entrar en sistema y para la creación de un nuevo Usuario
    /// </summary>
    public class CredencialesUsuarioDTO
    {
        /// <summary>
        /// Correo electrónico del usuario
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
    #endregion

    #region roles
    /// <summary>
    /// Para cambiar el nombre a un rol
    /// </summary>
    public class RolCambiaNombre
    {
        /// <summary>
        /// Identificador único del rol que no es identity
        /// </summary>
        public int IdRol { get; set; }
        /// <summary>
        /// Nuevo nombre del rol
        /// </summary>
        public string DescripcionRol { get; set; }
    }
    #endregion

    #region usuarios
    /// <summary>
    /// Para crear una nueva contraseña para los usuarios
    /// </summary>
    public class ReestablecerContraseniaDTO
    {
        /// <summary>
        /// Identificador del usuario (idAspNetUser)
        /// </summary>
        public string idUsuario { get; set; }
        /// <summary>
        /// Contraseña nueva
        /// </summary>
        public string nuevaContrasenia { get; set; }
        /// <summary>
        /// Confirmación de la nueva contraseña
        /// </summary>
        public string nuevaContraseniaConfirma { get; set; }
    }
    #endregion
}
