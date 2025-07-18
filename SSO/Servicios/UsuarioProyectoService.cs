using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SSO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioProyectoService : IUsuarioProyectoService
    {
        private readonly ISSORepositorio<UsuarioProyecto> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioProyectoService(
            ISSORepositorio<UsuarioProyecto> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioProyectoDTO> Crear(UsuarioProyectoDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioProyecto>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioProyectoDTO();
            }
            return _Mapper.Map<UsuarioProyectoDTO>(objetoCreado);
        }

        public async Task<UsuarioProyectoDTO> CrearYObtener(UsuarioProyectoDTO registro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioProyecto>(registro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioProyectoDTO();
            }
            return _Mapper.Map<UsuarioProyectoDTO>(objetoCreado);
        }

        public async Task<bool> Editar(UsuarioProyectoDTO registro)
        {
            var objeto = await _Repositorio.Obtener(z => z.Id == registro.Id);
            objeto.Estatus = registro.Estatus;
            var objetoEditado = await _Repositorio.Editar(_Mapper.Map<UsuarioProyecto>(registro));
            return objetoEditado;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objeto = await _Repositorio.Obtener(z => z.Id == Id);
            var resultado = await _Repositorio.Eliminar(objeto);
            return resultado;
        }

        public async Task<List<UsuarioProyectoDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioProyectoDTO>>(lista);
        }

        public async Task<List<UsuarioProyectoDTO>> ObtenXIdProyecto(int IdProyecto)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdProyecto == IdProyecto);
            return _Mapper.Map<List<UsuarioProyectoDTO>>(lista);
        }

        public async Task<List<UsuarioProyectoDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioProyectoDTO>>(lista);
        }

        public async Task<List<UsuarioProyectoDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _Mapper.Map<List<UsuarioProyectoDTO>>(lista);
        }

        public async Task<List<UsuarioProyectoDTO>> ObtenXIdUsuarioXIdProyecto(int IdUsuario, int IdProyecto)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            lista = lista.Where(z => z.IdProyecto == IdProyecto).ToList();
            return _Mapper.Map<List<UsuarioProyectoDTO>>(lista);
        }

        public async Task<List<UsuarioProyectoDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario && z.IdEmpresa == IdEmpresa);
            //lista = lista.Where(z => z.IdProyecto == IdProyecto).ToList();
            return _Mapper.Map<List<UsuarioProyectoDTO>>(lista);
        }
    }
}
