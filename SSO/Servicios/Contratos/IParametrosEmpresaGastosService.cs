using SistemaERP.DTO.SSO;

namespace SistemaERP.BLL.ContratoSSO
{
    /// <summary>
    /// Interfaz que contiene todos los campos para usar la tabla de Banco
    /// </summary>
    public interface IParametrosEmpresaGastosService
    {
        Task<List<ParametrosEmpresaGastosDTO>> ObtenTodosxEmpresa(int idEmpresa);
        Task<ParametrosEmpresaGastosDTO> ObtenXId(int id);

        Task<ParametrosEmpresaGastosDTO> ObtenXPermiso(string clave);

        Task<ParametrosEmpresaGastosDTO> Crear(ParametrosEmpresaGastosDTO banco);

        Task<ParametrosEmpresaGastosDTO> Editar(ParametrosEmpresaGastosDTO banco);

    }

}
