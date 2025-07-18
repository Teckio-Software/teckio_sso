using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class RolEmpresaService : IRolEmpresaService
    {
        private readonly ISSORepositorio<RolEmpresa> _Repositorio;
        private readonly IMapper _mapper;

        public RolEmpresaService(ISSORepositorio<RolEmpresa> rolRepositorio, IMapper mapper)
        {
            _Repositorio = rolRepositorio;
            _mapper = mapper;
        }

        public async Task<RespuestaDTO> Crear(RolEmpresaDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var RolCreado = await _Repositorio.Crear(_mapper.Map<RolEmpresa>(parametro));

            if (RolCreado.Id == 0)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "No se pudo crear";
                return respuesta;
            }
            respuesta.Estatus = true;
            respuesta.Descripcion = "Relación creada";
            return respuesta;
        }

        public async Task<RolEmpresaDTO> CrearYObtener(RolEmpresaDTO parametro)
        {
            var RolCreado = await _Repositorio.Crear(_mapper.Map<RolEmpresa>(parametro));

            if (RolCreado.Id == 0)
            {
                return _mapper.Map<RolEmpresaDTO>(RolCreado);
            }
            return _mapper.Map<RolEmpresaDTO>(RolCreado);
        }

        public async Task<RespuestaDTO> Eliminar(int Id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            var rolEncontrado = await _Repositorio.Obtener(u => u.Id == Id);

            if (rolEncontrado == null)
            {
                respuesta.Descripcion = "El rol no existe";
                respuesta.Estatus = false;
                return respuesta;
            }
            respuesta.Estatus = await _Repositorio.Eliminar(rolEncontrado);
            if (!respuesta.Estatus)
            {
                respuesta.Descripcion = "No se pudo eliminar";
                respuesta.Estatus = false;
                return respuesta;
            }
            respuesta.Descripcion = "Relación eliminada";
            respuesta.Estatus = true;
            return respuesta;
        }

        public async Task<List<RolEmpresaDTO>> ObtenTodos()
        {
            var listaRoles = await _Repositorio.ObtenerTodos();
            return _mapper.Map<List<RolEmpresaDTO>>(listaRoles.ToList());
        }

        public async Task<RolEmpresaDTO> ObtenXId(int Id)
        {
            var listaRoles = await _Repositorio.Obtener(z => z.Id == Id);
            return _mapper.Map<RolEmpresaDTO>(listaRoles);
        }

        public async Task<List<RolEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var listaRoles = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _mapper.Map<List<RolEmpresaDTO>>(listaRoles.ToList());
        }

        public async Task<List<RolEmpresaDTO>> ObtenXIdRol(int IdRol)
        {
            var listaRoles = await _Repositorio.ObtenerTodos(z => z.IdRol == IdRol);
            return _mapper.Map<List<RolEmpresaDTO>>(listaRoles.ToList());
        }
    }
}
