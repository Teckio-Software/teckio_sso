using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioClasificacionCfdiService : IUsuarioClasificacionCfdiService
    {
        private readonly ISSORepositorio<UsuarioClasificacionCfdi> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioClasificacionCfdiService(
            ISSORepositorio<UsuarioClasificacionCfdi> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioClasificacionCfdiDTO> Crear(UsuarioClasificacionCfdiDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioClasificacionCfdi>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioClasificacionCfdiDTO();
            }
            return _Mapper.Map<UsuarioClasificacionCfdiDTO>(objetoCreado);
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objeto = await _Repositorio.Obtener(z => z.Id == Id);
            var resultado = await _Repositorio.Eliminar(objeto);
            return resultado;
        }

        public async Task<List<UsuarioClasificacionCfdiDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioClasificacionCfdiDTO>>(lista);
        }

        public async Task<List<UsuarioClasificacionCfdiDTO>> ObtenXIdClasificacionCfdi(int IdClasificacionCfdi)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdClasificacionCfdi == IdClasificacionCfdi);
            return _Mapper.Map<List<UsuarioClasificacionCfdiDTO>>(lista);
        }

        public async Task<List<UsuarioClasificacionCfdiDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioClasificacionCfdiDTO>>(lista);
        }

        public async Task<List<UsuarioClasificacionCfdiDTO>> ObtenXIdUsuarioXIdClasificacionCfdi(int IdUsuario, int IdClasificacionCfdi)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            lista = lista.Where(z => z.IdClasificacionCfdi == IdClasificacionCfdi).ToList();
            return _Mapper.Map<List<UsuarioClasificacionCfdiDTO>>(lista);
        }
    }
}
