using Microsoft.EntityFrameworkCore;
using SistemaERP.DTO;
using SSO.DTO;

namespace SistemaERP.BLL.Contrato
{
    public interface IErpService
    {
        Task<List<ErpCorporativoDTO>> ObtenTodos();
        Task<List<ErpDTO>> ObtenErps();
        Task<ErpCorporativoDTO> ObtenXIdCorporativo(int IdCorporativo);
        Task<RespuestaDTO> Crear(ErpCorporativoDTO parametro);
        Task<ErpCorporativoDTO> CrearYObtener(ErpCorporativoDTO parametro);
        Task<RespuestaDTO> Editar(ErpCorporativoDTO parametro);
        Task<RespuestaDTO> Eliminar(int Id);
    }

}
