using AutoMapper;
using SistemaERP.BLL.Contrato;
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
    public class UsuarioDivisionService : IUsuarioDivisionService
    {
        private readonly ISSORepositorio<UsuarioDivision> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioDivisionService(
            ISSORepositorio<UsuarioDivision> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioDivisionDTO> Crear(UsuarioDivisionDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioDivision>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioDivisionDTO();
            }
            return _Mapper.Map<UsuarioDivisionDTO>(objetoCreado);
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objetoEncontrado = await _Repositorio.Obtener(z => z.Id == Id);
            if (objetoEncontrado.Id <= 0)
            {
                return true;
            }
            var resultado = await _Repositorio.Eliminar(objetoEncontrado);
            return resultado;
        }

        public async Task<List<UsuarioDivisionDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioDivisionDTO>>(lista);
        }

        public async Task<List<UsuarioDivisionDTO>> ObtenXIdDivision(int IdDivision)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdDivision == IdDivision);
            return _Mapper.Map<List<UsuarioDivisionDTO>>(lista);
        }

        public async Task<List<UsuarioDivisionDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioDivisionDTO>>(lista);
        }

        public async Task<List<UsuarioDivisionDTO>> ObtenXIdUsuarioXIdDivision(int IdUsuario, int IdDivision)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdDivision == IdDivision);
            var lista2 = lista.Where(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioDivisionDTO>>(lista2);
        }
    }
}
