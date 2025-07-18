using SSO.DTO;
using SSO.Modelos;
using System.Linq.Expressions;

namespace SSO.Servicios.Contratos
{
    public interface IArchivosEmpresaService
    {
        Task<List<ParametrosTimbradoDTO>> ObtenTodos();
        Task<List<ParametrosTimbradoDTO>> ObtenenXEmpresa(int IdEmpresa);
        Task<ParametrosTimbradoDTO> ObtenenXId(int id);
        Task<RespuestaDTO> Crear(ParametrosTimbradoDTO parametro);
        Task<ParametrosTimbradoDTO> CrearYObtener(ParametrosTimbradoDTO parametro);
        Task<RespuestaDTO> Editar(ParametrosTimbradoDTO parametro);
        Task<RespuestaDTO> Eliminar(int Id);
        Task<List<ParametrosTimbradoDTO>> Filtar(Expression<Func<ParametrosTimbrado, bool>> filtro);


    }
}
