using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ContratoSSO
{
    public interface IMenuEmpresaService
    {
        Task<List<MenuEmpresaDTO>> ObtenTodos();
        Task<MenuEmpresaDTO> ObtenXId(int Id);
        Task<List<MenuEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<List<MenuEmpresaDTO>> ObtenXIdMenu(int IdMenu);
        Task<MenuEmpresaDTO> ObtenXIdEmpresaYMenu(int IdEmpresa, int IdMenu);
        Task<MenuEmpresaDTO> Crear(MenuEmpresaDTO modelo);
        Task<bool> Eliminar(int Id);
    }
}
