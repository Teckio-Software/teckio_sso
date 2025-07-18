using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioEmpresaPorDefectoService : IUsuarioEmpresaPorDefectoService
    {
        private readonly ISSORepositorio<UsuarioEmpresaPorDefecto> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioEmpresaPorDefectoService(
            ISSORepositorio<UsuarioEmpresaPorDefecto> Repositorio
            , IMapper mapper
            )
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<bool> CambiaEmpresaPorDefecto(UsuarioEmpresaPorDefectoDTO parametro)
        {
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == parametro.Id);

            if (objetoEncontrado == null)
                return false;

            objetoEncontrado.IdEmpresa = parametro.IdEmpresa;
            var respuesta = await _Repositorio.Editar(objetoEncontrado);
            return respuesta;
        }

        public async Task<UsuarioEmpresaPorDefectoDTO> CrearYObtener(UsuarioEmpresaPorDefectoDTO parametro)
        {
            var existeRelacion = await ObtenXIdUsuario(parametro.IdUsuario);
            if (existeRelacion.Id > 0)
            {
                existeRelacion.IdEmpresa = parametro.IdEmpresa;
                var resultado = await CambiaEmpresaPorDefecto(existeRelacion);
                if (resultado)
                {
                    return existeRelacion;
                }
                return new UsuarioEmpresaPorDefectoDTO();
            }
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioEmpresaPorDefecto>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioEmpresaPorDefectoDTO();
            }
            return _Mapper.Map<UsuarioEmpresaPorDefectoDTO>(objetoCreado);
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objetoEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

            if (objetoEncontrado == null)
                return false;

            var respuesta = await _Repositorio.Eliminar(objetoEncontrado);
            return respuesta;
        }

        public async Task<List<UsuarioEmpresaPorDefectoDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioEmpresaPorDefectoDTO>>(lista);
        }

        public async Task<List<UsuarioEmpresaPorDefectoDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _Mapper.Map<List<UsuarioEmpresaPorDefectoDTO>>(lista);
        }

        public async Task<UsuarioEmpresaPorDefectoDTO> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.Obtener(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<UsuarioEmpresaPorDefectoDTO>(lista);
        }

        public async Task<UsuarioEmpresaPorDefectoDTO> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            lista = lista.Where(z => z.IdEmpresa == IdEmpresa).ToList();
            if (lista.Count() > 0)
            {
                return _Mapper.Map<UsuarioEmpresaPorDefectoDTO>(lista.FirstOrDefault());
            }
            return new UsuarioEmpresaPorDefectoDTO();
        }
    }
}
