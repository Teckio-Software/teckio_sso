using SistemaERP.DTO.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL
{
    public interface IDivisionService
    {
        Task<List<DivisionDTO>> ObtenTodos();
        Task<DivisionDTO> ObtenXId(int Id);
        Task<List<DivisionDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<DivisionDTO> CrearYObtener(DivisionDTO parametro);
        Task<bool> Editar(DivisionDTO parametro);
        Task<bool> Eliminar(int Id);
    }
}
