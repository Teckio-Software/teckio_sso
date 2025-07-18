using AutoMapper;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ServicioSSO
{
    public class MenuEmpresaService : IMenuEmpresaService
    {
        private readonly ISSORepositorio<MenuEmpresa> _Repositorio;
        private readonly IMapper _Mapper;

        public MenuEmpresaService(ISSORepositorio<MenuEmpresa> Repositorio, IMapper Mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = Mapper;
        }

        public async Task<MenuEmpresaDTO> Crear(MenuEmpresaDTO modelo)
        {
            var ParametrosEmpresaCreado = await _Repositorio.Crear(_Mapper.Map<MenuEmpresa>(modelo));

            if (ParametrosEmpresaCreado.Id == 0)
                return new MenuEmpresaDTO();

            return _Mapper.Map<MenuEmpresaDTO>(ParametrosEmpresaCreado);
        }

        public async Task<bool> Eliminar(int Id)
        {
            var rolEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

            if (rolEncontrado == null)
                return false;

            return await _Repositorio.Eliminar(rolEncontrado);
        }

        public async Task<List<MenuEmpresaDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<MenuEmpresaDTO>>(lista);
        }

        public async Task<MenuEmpresaDTO> ObtenXId(int Id)
        {
            var lista = await _Repositorio.Obtener(z => z.Id == Id);
            return _Mapper.Map<MenuEmpresaDTO>(lista);
        }

        public async Task<List<MenuEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _Mapper.Map<List<MenuEmpresaDTO>>(lista);
        }

        public async Task<MenuEmpresaDTO> ObtenXIdEmpresaYMenu(int IdEmpresa, int IdMenu)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            lista = lista.Where(z => z.IdMenu == IdMenu).ToList();
            if (lista.Count() <= 0)
            {
                return new MenuEmpresaDTO();
            }
            return _Mapper.Map<MenuEmpresaDTO>(lista[0]);
        }

        public async Task<List<MenuEmpresaDTO>> ObtenXIdMenu(int IdMenu)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdMenu == IdMenu);
            return _Mapper.Map<List<MenuEmpresaDTO>>(lista);
        }
    }
}
