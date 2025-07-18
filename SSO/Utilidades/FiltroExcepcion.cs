using Microsoft.AspNetCore.Mvc.Filters;

namespace Utilidades
{
    /// <summary>
    /// Esta clase nos sirve para obtener los errores que ocurren en los controladores
    /// </summary>
    public class FiltroExcepcion : ExceptionFilterAttribute
    {
        /// <summary>
        /// Extiende de <see cref="ILogger"/> que es la que captura en cada Controlador el error
        /// </summary>
        private readonly ILogger<FiltroExcepcion> _Logger;
        /// <summary>
        /// Aqui captura el error en el controlador
        /// </summary>
        /// <param name="zLogger"></param>
        public FiltroExcepcion(ILogger<FiltroExcepcion> zLogger)
        {
            this._Logger = zLogger;
        }
        /// <summary>
        /// Aqui muestra el error producIdo en el controlador
        /// </summary>
        /// <param name="zContext"></param>
        public override void OnException(ExceptionContext zContext)
        {
            _Logger.LogError(zContext.Exception, zContext.Exception.Message);
            base.OnException(zContext);
        }
    }
}
