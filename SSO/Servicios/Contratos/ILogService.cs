using Microsoft.EntityFrameworkCore;
using SSO.DTO;

namespace SSO.Servicios.Contratos
{
    public interface ILogService
    {
        Task<List<LogDTO>> ObtenerXIdEmpresa(int IdEmpresa);
    }
}
