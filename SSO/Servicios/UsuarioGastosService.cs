using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model.Gastos;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioGastosService : IUsuarioGastosService
    {
        private readonly ISSORepositorio<UsuarioGastos> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioGastosService(
            ISSORepositorio<UsuarioGastos> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioGastosDTO> Crear(UsuarioGastosDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioGastos>(parametro));
            if (objetoCreado.id <= 0)
            {
                return new UsuarioGastosDTO();
            }
            return _Mapper.Map<UsuarioGastosDTO>(objetoCreado);
        }

        public async Task<bool> Editar(UsuarioGastosDTO parametro)
        {
            var objeto = _Mapper.Map<UsuarioGastos>(parametro);
            var ObjetoEncontrado = await _Repositorio.Obtener(u => u.id == objeto.id);
            if (ObjetoEncontrado == null)
                return false;
            ObjetoEncontrado.numeroEmpleado = objeto.numeroEmpleado;
            ObjetoEncontrado.fecha_baja = objeto.fecha_baja;
            ObjetoEncontrado.estatus = objeto.estatus;
            var respuesta = await _Repositorio.Editar(ObjetoEncontrado);
            return respuesta;
        }

        public async Task<List<UsuarioGastosDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioGastosDTO>>(lista);
        }

        public async Task<UsuarioGastosDTO> ObtenXId(int Id)
        {
            var lista = await _Repositorio.Obtener(z => z.id == Id);
            return _Mapper.Map<UsuarioGastosDTO>(lista);
        }


        public async Task<UsuarioGastosDTO> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.Obtener(z => z.idUsuario == IdUsuario);
            return _Mapper.Map<UsuarioGastosDTO>(lista);
        }
    }
}
