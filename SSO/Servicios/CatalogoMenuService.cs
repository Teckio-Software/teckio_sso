using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SistemaERP.Model;

namespace SistemaERP.BLL.ServicioSSO
{
    public class CatalogoMenuService : ICatalogoMenuService
    {
        private readonly ISSORepositorio<CatalogoMenu> _Repositorio;
        private readonly IMapper _Mapper;

        public CatalogoMenuService(
            ISSORepositorio<CatalogoMenu> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }
        public async Task<List<CatalogoMenuDTO>> ObtenTodos()
        {
            try
            {
                var lista = await _Repositorio.ObtenerTodos();
                return _Mapper.Map<List<CatalogoMenuDTO>>(lista);
            }
            catch (Exception ex)
            {
                return new List<CatalogoMenuDTO>();
            }
        }

        public async Task<CatalogoMenuDTO> ObtenXId(int Id)
        {
            try
            {
                var lista = await _Repositorio.Obtener(z => z.Id == Id);
                return _Mapper.Map<CatalogoMenuDTO>(lista);
            }
            catch (Exception ex)
            {
                return new CatalogoMenuDTO();
            }
        }
    }
}
