using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaERP.Model;
using SistemaERP.DAL.Repositorios.Contrato;
using SSO.DTO;
using SSO.Servicios.Contratos;
using SistemaERP.DAL.DBContext;

namespace SistemaERP.BLL.Servicios.Utilidades
{
    public class LogService : ILogService
    {
        //private readonly ISSORepositorio<LogRegistro> _repository;
        //private readonly IMapper _Mapper;
        private readonly SSOContext _dbContext;

        public LogService(SSOContext dbContext)
        {
            //_repository = repository;
            //_Mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<List<LogDTO>> ObtenerXIdEmpresa(int IdEmpresa)
        {
            //var lista = await _repository.ObtenerTodos(l => l.IdEmpresa == IdEmpresa);
            var lista = _dbContext.Logs
                .Include(l => l.IdUsuarioNavigation)
                .Include(l => l.IdEmpresaNavigation)
                .Select(l=>new LogDTO
                {
                    Id = l.Id,
                    Fecha = l.Fecha,
                    Nivel = l.Nivel,
                    Metodo = l.Metodo,
                    Descripcion = l.Descripcion,
                    DbContext = l.DbContext,
                    IdUsuario = l.IdUsuario,
                    NombreUsuario = l.IdUsuarioNavigation.NombreUsuario,
                    IdEmpresa = l.IdEmpresa,
                    NombreEmpresa = l.IdEmpresaNavigation.NombreComercial
                })
                .ToList();
            if(lista.Count > 0)
            {
                return lista;
            }
            else
            {
                return new List<LogDTO>();
            }
        }
    }
}
