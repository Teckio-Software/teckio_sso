using Microsoft.EntityFrameworkCore;
using SistemaERP.DTO.SSO;

namespace SistemaERP.BLL.Contrato
{
    public interface ICorporativoService
    {
        Task<List<CorporativoDTO>> ObtenTodos();
        Task<CorporativoDTO> ObtenXId(int IdCorporativo);
        //Task<SesionDTO> ValIdarCredenciales(string correo, string clave);
        Task<CorporativoDTO> Crear(CorporativoDTO modelo);
        Task<bool> Editar(CorporativoDTO modelo);
        Task<bool> Eliminar(int Id);
    }
}
