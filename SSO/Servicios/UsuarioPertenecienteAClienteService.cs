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
    public class UsuarioPertenecienteAClienteService : IUsuarioPertenecienteAClienteService
    {
        private readonly ISSORepositorio<UsuarioPertenecienteACliente> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioPertenecienteAClienteService(
            ISSORepositorio<UsuarioPertenecienteACliente> Repositorio
            , IMapper mapper
            )
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioPertenecienteAClienteDTO> CrearYObtener(UsuarioPertenecienteAClienteDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioPertenecienteACliente>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioPertenecienteAClienteDTO();
            }
            return _Mapper.Map<UsuarioPertenecienteAClienteDTO>(objetoCreado);
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == Id);
            if (objetoEncontrado == null)
                return false;

            var respuesta = await _Repositorio.Eliminar(objetoEncontrado);
            return respuesta;
        }

        public async Task<List<UsuarioPertenecienteAClienteDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioPertenecienteAClienteDTO>>(lista);
        }

        public async Task<List<UsuarioPertenecienteAClienteDTO>> ObtenXIdCliente(int IdCorporativo)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdCorporativo == IdCorporativo);
            return _Mapper.Map<List<UsuarioPertenecienteAClienteDTO>>(lista);
        }

        public async Task<UsuarioPertenecienteAClienteDTO> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.Obtener(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<UsuarioPertenecienteAClienteDTO>(lista);
        }

        public async Task<UsuarioPertenecienteAClienteDTO> ObtenXIdUsuarioXIdCliente(int IdUsuario, int IdCorporativo)
        {
            var lista = await _Repositorio.Obtener(z => z.IdUsuario == IdUsuario && z.IdCorporativo == IdCorporativo);
            //lista = lista.Where(z => z.IdCorporativo == IdCorporativo).FirstOrDefault();
            return _Mapper.Map<UsuarioPertenecienteAClienteDTO>(lista);
        }
    }
}
