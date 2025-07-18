using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model.Gastos;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioAutoFacService : IUsuarioAutoFacService
    {
        private readonly ISSORepositorio<UsuarioAutoFac> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioAutoFacService(
            ISSORepositorio<UsuarioAutoFac> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioAutoFacDTO> Crear(UsuarioAutoFacDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioAutoFac>(parametro));
            if (objetoCreado.id <= 0)
            {
                return new UsuarioAutoFacDTO();
            }
            return _Mapper.Map<UsuarioAutoFacDTO>(objetoCreado);
        }

        public async Task<bool> Editar(UsuarioAutoFacDTO parametro)
        {
            var objeto = _Mapper.Map<UsuarioAutoFac>(parametro);
            var ObjetoEncontrado = await _Repositorio.Obtener(u => u.id == objeto.id);
            if (ObjetoEncontrado == null)
                return false;
            ObjetoEncontrado.fechaBaja = objeto.fechaBaja;
            ObjetoEncontrado.estatus = objeto.estatus;
            var respuesta = await _Repositorio.Editar(ObjetoEncontrado);
            return respuesta;
        }

        public async Task<List<UsuarioAutoFacDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioAutoFacDTO>>(lista);
        }

        public async Task<UsuarioAutoFacDTO> ObtenXId(int Id)
        {
            var lista = await _Repositorio.Obtener(z => z.id == Id);
            return _Mapper.Map<UsuarioAutoFacDTO>(lista);
        }


        public async Task<UsuarioAutoFacDTO> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.Obtener(z => z.idUsuario == IdUsuario);
            return _Mapper.Map<UsuarioAutoFacDTO>(lista);
        }
    }
}
