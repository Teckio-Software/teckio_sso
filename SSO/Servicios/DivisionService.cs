using AutoMapper;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO.SSO;
using SistemaERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ServicioSSO
{
    public class DivisionService : IDivisionService
    {
        private readonly ISSORepositorio<Division> _Repositorio;
        private readonly IMapper _Mapper;

        public DivisionService(
            ISSORepositorio<Division> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }
        public async Task<DivisionDTO> CrearYObtener(DivisionDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<Division>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new DivisionDTO();
            }
            return _Mapper.Map<DivisionDTO>(objetoCreado);
        }

        public async Task<bool> Editar(DivisionDTO parametro)
        {
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == parametro.Id);
            if (objetoEncontrado == null)
                return false;

            objetoEncontrado.Descripcion = parametro.Descripcion;
            var respuesta = await _Repositorio.Editar(objetoEncontrado);

            return respuesta;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == Id);
            if (objetoEncontrado == null)
                return false;

            var respuesta = await _Repositorio.Eliminar(objetoEncontrado);
            return respuesta;
        }

        public async Task<List<DivisionDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<DivisionDTO>>(lista);
        }

        public async Task<DivisionDTO> ObtenXId(int Id)
        {
            var lista = await _Repositorio.Obtener(z => z.Id == Id);
            return _Mapper.Map<DivisionDTO>(lista);
        }

        public async Task<List<DivisionDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _Mapper.Map<List<DivisionDTO>>(lista);
        }
    }
}
