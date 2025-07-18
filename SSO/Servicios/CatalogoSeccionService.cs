using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO.Menu;
using SistemaERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ServicioSSO
{
    public class CatalogoSeccionService : ICatalogoSeccionService
    {
        private readonly ISSORepositorio<CatalogoSeccion> _Repositorio;
        private readonly IMapper _Mapper;

        public CatalogoSeccionService(
            ISSORepositorio<CatalogoSeccion> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }
        public async Task<List<CatalogoSeccionDTO>> ObtenTodos()
        {
            try
            {
                var lista = await _Repositorio.ObtenerTodos();
                return _Mapper.Map<List<CatalogoSeccionDTO>>(lista);
            }
            catch (Exception ex)
            {
                return new List<CatalogoSeccionDTO>();
            }
        }

        public async Task<CatalogoSeccionDTO> ObtenTodosXCodigoSeccion(string Codigo)
        {
            try
            {
                var lista = await _Repositorio.Obtener(z => z.CodigoSeccion == Codigo);
                return _Mapper.Map<CatalogoSeccionDTO>(lista);
            }
            catch (Exception ex)
            {
                return new CatalogoSeccionDTO();
            }
        }

        public async Task<List<CatalogoSeccionDTO>> ObtenTodosXIdMenu(int IdMenu)
        {
            try
            {
                var lista = await _Repositorio.ObtenerTodos(z => z.IdMenu == IdMenu);
                return _Mapper.Map<List<CatalogoSeccionDTO>>(lista);
            }
            catch (Exception ex)
            {
                return new List<CatalogoSeccionDTO>();
            }
        }

        public async Task<CatalogoSeccionDTO> ObtenXId(int Id)
        {
            try
            {
                var lista = await _Repositorio.Obtener(z => z.Id == Id);
                return _Mapper.Map<CatalogoSeccionDTO>(lista);
            }
            catch (Exception ex)
            {
                return new CatalogoSeccionDTO();
            }
        }
    }
}
