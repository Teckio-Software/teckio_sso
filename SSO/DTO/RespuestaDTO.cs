namespace SSO.DTO
{
    /// <summary>
    /// Clase para confirmar las acciones dentro del Teckio
    /// </summary>
    public class RespuestaDTO
    {
        /// <summary>
        /// Para saber si se completo la acción
        /// </summary>
        public bool Estatus { get; set; }
        /// <summary>
        /// Descripción del mensaje
        /// </summary>
        public string Descripcion { get; set; }
    }
    /// <summary>
    /// Creo que para lo que hizo en algun momento mario
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Response<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string msg { get; set; }
    }
}
