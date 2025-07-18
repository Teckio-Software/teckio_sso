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
    public class UsuarioCorporativoService : IUsuarioCorporativoService
    {
        private readonly ISSORepositorio<UsuarioCorporativo> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioCorporativoService(
            ISSORepositorio<UsuarioCorporativo> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<bool> Crear(UsuarioCorporativoDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioCorporativo>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return false;
            }
            return true;
        }

        public async Task<UsuarioCorporativoDTO> CrearYObtener(UsuarioCorporativoDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioCorporativo>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioCorporativoDTO();
            }
            return _Mapper.Map<UsuarioCorporativoDTO>(objetoCreado);
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objeto = await _Repositorio.Obtener(z => z.Id == Id);
            var resultado = await _Repositorio.Eliminar(objeto);
            return resultado;
        }

        public async Task<List<UsuarioCorporativoDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioCorporativoDTO>>(lista);
        }

        public async Task<List<UsuarioCorporativoDTO>> ObtenXIdCorporativo(int IdCorporativo)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdCorporativo == IdCorporativo);
            return _Mapper.Map<List<UsuarioCorporativoDTO>>(lista);
        }

        public async Task<UsuarioCorporativoDTO> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.Obtener(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<UsuarioCorporativoDTO>(lista);
        }

        public async Task<List<UsuarioCorporativoDTO>> ObtenXIdUsuarioXIdCorporativo(int IdCorporativo, int IdUsuario)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            lista = lista.Where(z => z.IdCorporativo == IdCorporativo).ToList();
            return _Mapper.Map<List<UsuarioCorporativoDTO>>(lista);
        }
    }
}
