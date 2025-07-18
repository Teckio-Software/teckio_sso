using SistemaERP.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.Servicios.Contrato
{
    public interface ICatalogoMenuService
    {
        Task<List<CatalogoMenuDTO>> ObtenTodos();
        Task<CatalogoMenuDTO> ObtenXId(int Id);
    }

    public interface ICatalogoSeccionService
    {
        Task<List<CatalogoSeccionDTO>> ObtenTodos();
        Task<List<CatalogoSeccionDTO>> ObtenTodosXIdMenu(int IdMenu);
        Task<CatalogoSeccionDTO> ObtenTodosXCodigoSeccion(string Codigo);
        Task<CatalogoSeccionDTO> ObtenXId(int Id);
    }

    public interface ICatalogoActividadService
    {
        Task<List<CatalogoActividadDTO>> ObtenTodos();
        Task<List<CatalogoActividadDTO>> ObtenTodosXIdSeccion(int IdSeccion);
        Task<CatalogoActividadDTO> ObtenTodosXCodigoActividad(string Codigo);
        Task<CatalogoActividadDTO> ObtenXId(int Id);
    }
}
