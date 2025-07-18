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
    public class CatalogoActividadService : ICatalogoActividadService
    {
        private readonly ISSORepositorio<CatalogoActividad> _Repositorio;
        private readonly IMapper _Mapper;

        public CatalogoActividadService(
            ISSORepositorio<CatalogoActividad> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }
        public async Task<List<CatalogoActividadDTO>> ObtenTodos()
        {
            try
            {
                var lista = await _Repositorio.ObtenerTodos();
                return _Mapper.Map<List<CatalogoActividadDTO>>(lista);
            }
            catch (Exception ex)
            {
                return new List<CatalogoActividadDTO>();
            }
        }

        public async Task<CatalogoActividadDTO> ObtenTodosXCodigoActividad(string Codigo)
        {
            try
            {
                var lista = await _Repositorio.Obtener(z => z.CodigoActividad == Codigo);
                return _Mapper.Map<CatalogoActividadDTO>(lista);
            }
            catch (Exception ex)
            {
                return new CatalogoActividadDTO();
            }
        }

        public async Task<List<CatalogoActividadDTO>> ObtenTodosXIdSeccion(int IdSeccion)
        {
            try
            {
                var lista = await _Repositorio.ObtenerTodos(z => z.IdSeccion == IdSeccion);
                return _Mapper.Map<List<CatalogoActividadDTO>>(lista);
            }
            catch (Exception ex)
            {
                return new List<CatalogoActividadDTO>();
            }
        }

        public async Task<CatalogoActividadDTO> ObtenXId(int Id)
        {
            try
            {
                var lista = await _Repositorio.Obtener(z => z.Id == Id);
                return _Mapper.Map<CatalogoActividadDTO>(lista);
            }
            catch (Exception ex)
            {
                return new CatalogoActividadDTO();
            }
        }
    }
}
