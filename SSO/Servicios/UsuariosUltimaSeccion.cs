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
    public class UsuariosUltimaSeccionServicio : IUsuariosUltimaSeccionService
    {
        private readonly ISSORepositorio<UsuariosUltimaSeccion> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuariosUltimaSeccionServicio( 
            ISSORepositorio<UsuariosUltimaSeccion> usuarioRepositorio
        , IMapper mapper)
        {
            _Repositorio = usuarioRepositorio;
            _Mapper = mapper;
        }

        public async Task<List<UsuariosUltimaSeccionDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuariosUltimaSeccionDTO>>(lista);
        }

        public async Task<UsuariosUltimaSeccionDTO> ObtenXIdUltimoS(int IdUsuarioUltimaS)
        {
            var lista = await _Repositorio.Obtener(z => z.Id == IdUsuarioUltimaS);
            return _Mapper.Map<UsuariosUltimaSeccionDTO>(lista);
        }

        public async Task<UsuariosUltimaSeccionDTO> CrearYObtener(UsuariosUltimaSeccionDTO modelo)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuariosUltimaSeccion>(modelo));
            if (objetoCreado.Id <= 0)
            {
                return new UsuariosUltimaSeccionDTO();
            }
            return _Mapper.Map<UsuariosUltimaSeccionDTO>(objetoCreado);
        }

        public async Task<bool> Editar(UsuariosUltimaSeccionDTO modelo)
        {
            // Mapear DTO a entidad
            var objeto = _Mapper.Map<UsuariosUltimaSeccion>(modelo); 

            // Obtener la entidad existente del repositorio
            var ObjetoEncontrado = await _Repositorio.Obtener(u => u.Id == objeto.Id);
            if (ObjetoEncontrado == null)
            {
                throw new Exception("El usuario no existe"); // Cambiado a Exception por simplicidad
            }

            // Actualizar las propiedades relevantes de la entidad encontrada
            ObjetoEncontrado.IdProyecto = objeto.IdProyecto;
            ObjetoEncontrado.IdEmpresa = objeto.IdEmpresa;

            // Llamar al método Editar del repositorio
            bool respuesta = await _Repositorio.Editar(ObjetoEncontrado);

            if (!respuesta)
            {
                throw new Exception("No se pudo editar");
            }

            return respuesta; // Devolver la respuesta del repositorio

        }
        public Task<UsuariosUltimaSeccionDTO> Eliminar(int IdUsuarioUltimaS)
        {
            throw new NotImplementedException();
        }

        public async Task<UsuariosUltimaSeccionDTO> ObtenerXIdUsuario(int IdTablaUsuario)
        {
            var resultado = await _Repositorio.Obtener(z => z.IdUsuario == IdTablaUsuario);

            return _Mapper.Map<UsuariosUltimaSeccionDTO>(resultado);
        }

   

 
    }
}
