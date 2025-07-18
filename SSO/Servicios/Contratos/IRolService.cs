using Microsoft.EntityFrameworkCore;
using SistemaERP.DTO;
using SSO.DTO;

namespace SistemaERP.BLL.Contrato
{
    public interface IRolService
    {
        Task<List<RolDTO>> ObtenTodos();
        Task<List<RolDTO>> ObtenXNombre(string Nombre);
        Task<RolDTO> ObtenXId(int Id);
        Task<List<RolDTO>> ObtenXIdAspNetRol(string Id);
        Task<RespuestaDTO> Crear(RolDTO parametro);
        Task<RolDTO> CrearYObtener(RolDTO parametro);
        Task<RespuestaDTO> Editar(RolDTO parametro);
        Task<RespuestaDTO> Eliminar(int Id);
    }

    public interface IRolEmpresaService
    {
        Task<List<RolEmpresaDTO>> ObtenTodos();
        Task<List<RolEmpresaDTO>> ObtenXIdRol(int IdRol);
        Task<List<RolEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<RolEmpresaDTO> ObtenXId(int Id);
        Task<RespuestaDTO> Crear(RolEmpresaDTO parametro);
        Task<RolEmpresaDTO> CrearYObtener(RolEmpresaDTO parametro);
        Task<RespuestaDTO> Eliminar(int Id);
    }

    public interface IRolSeccionService
    {
        Task<List<RolSeccionDTO>> ObtenTodos();
        Task<List<RolSeccionDTO>> ObtenTodosXIdSeccion(int IdSeccion);
        Task<List<RolSeccionDTO>> ObtenTodosXIdRol(int IdRol);
        Task<List<RolSeccionDTO>> ObtenTodosXIdRolXIdSeccion(int IdRol, int IdSeccion);
        Task<RespuestaDTO> Crear(RolSeccionDTO parametro);
        Task<RolSeccionDTO> CrearYObtener(RolSeccionDTO parametro);
        Task<RespuestaDTO> Eliminar(int Id);
    }

    public interface IRolActividadService
    {
        Task<List<RolActividadDTO>> ObtenTodos();
        Task<List<RolActividadDTO>> ObtenTodosXIdRol(int IdRol);
        Task<List<RolActividadDTO>> ObtenTodosXIdActividad(int IdActividad);
        Task<List<RolActividadDTO>> ObtenTodosXIdRolXIdActividad(int IdRol, int IdActividad);
        Task<RespuestaDTO> Crear(RolActividadDTO parametro);
        Task<RolActividadDTO> CrearYObtener(RolActividadDTO parametro);
        Task<RespuestaDTO> Eliminar(int Id);
    }
}
