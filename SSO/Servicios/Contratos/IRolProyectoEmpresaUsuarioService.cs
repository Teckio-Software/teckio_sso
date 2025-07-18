using Microsoft.Win32;
using SSO.DTO;

namespace SSO.Servicios.Contratos
{
    public interface IRolProyectoEmpresaUsuarioService
    {
        Task<List<RolProyectoEmpresaUsuarioDTO>> ObtenXIdEmpresaYIdUsuario(int IdEmpresa, int IdUsuario);
        Task<List<RolProyectoEmpresaUsuarioDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<List<RolProyectoEmpresaUsuarioDTO>> ObtenXIdUsuario(int IdEmpresa);
        Task<RolProyectoEmpresaUsuarioDTO> ActivarDesactivar(RolProyectoEmpresaUsuarioDTO registro);
        Task<RolProyectoEmpresaUsuarioDTO> Crear(RolProyectoEmpresaUsuarioDTO registro);
    }
}
