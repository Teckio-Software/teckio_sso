using Microsoft.EntityFrameworkCore;
using SistemaERP.DTO.SSO;

namespace SistemaERP.BLL.Contrato
{
    public interface IEmpresaService
    {
        Task<List<EmpresaDTO>> ObtenTodos();
        Task<List<EmpresaDTO>> ObtenXIdCorporativo(int IdCorporativo);
        Task<EmpresaDTO> ObtenXId(int Id);
        Task<EmpresaDTO> ObtenXSociedad(string sociedad);
        Task<EmpresaDTO> ObtenXRFC(string rfc);
        Task<bool> Crear(EmpresaDTO modelo);
        Task<EmpresaDTO> CrearYObtener(EmpresaDTO modelo);
        Task<bool> Editar(EmpresaDTO modelo);
    }
}
