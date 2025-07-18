using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;
using SSO.Modelos;
using SSO.Servicios.Contratos;

namespace SSO.Servicios
{
    public class RolProyectoEmpresaUsuarioService: IRolProyectoEmpresaUsuarioService
    {
        private readonly ISSORepositorio<RolProyectoEmpresaUsuario> _Repositorio;
        private readonly IMapper _Mapper;

        public RolProyectoEmpresaUsuarioService(ISSORepositorio<RolProyectoEmpresaUsuario> repositorio, IMapper mapper)
        {
            _Repositorio = repositorio;
            _Mapper = mapper;
        }

        public async Task<RolProyectoEmpresaUsuarioDTO> Crear(RolProyectoEmpresaUsuarioDTO registro)
        {
            var registroCreado = await _Repositorio.Crear(_Mapper.Map<RolProyectoEmpresaUsuario>(registro));
            return _Mapper.Map<RolProyectoEmpresaUsuarioDTO>(registroCreado);
        }

        public async Task<RolProyectoEmpresaUsuarioDTO> ActivarDesactivar(RolProyectoEmpresaUsuarioDTO registro)
        {
            var registroObtenido = await _Repositorio.Obtener(z => z.Id == registro.Id);
            registroObtenido.Estatus = registro.Estatus;
            await _Repositorio.Editar(registroObtenido);
            var registroEditado = await _Repositorio.Obtener(z => z.Id == registro.Id);
            return _Mapper.Map<RolProyectoEmpresaUsuarioDTO>(registroEditado);
        }

        public async Task<List<RolProyectoEmpresaUsuarioDTO>> ObtenXIdEmpresaYIdUsuario(int IdEmpresa, int IdUsuario)
        {
            var registros = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa && z.IdUsuario == IdUsuario);
            if(registros.Count <= 0)
            {
                return new List<RolProyectoEmpresaUsuarioDTO>();
            }
            return _Mapper.Map<List<RolProyectoEmpresaUsuarioDTO>>(registros);
        }

        public async Task<List<RolProyectoEmpresaUsuarioDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var registros = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            if (registros.Count <= 0)
            {
                return new List<RolProyectoEmpresaUsuarioDTO>();
            }
            return _Mapper.Map<List<RolProyectoEmpresaUsuarioDTO>>(registros);
        }

        public async Task<List<RolProyectoEmpresaUsuarioDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var registros = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            if (registros.Count <= 0)
            {
                return new List<RolProyectoEmpresaUsuarioDTO>();
            }
            return _Mapper.Map<List<RolProyectoEmpresaUsuarioDTO>>(registros);
        }
    }
}
