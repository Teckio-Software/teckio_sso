using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using SistemaERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioProveedorFormasPagoXEmpresaService : IUsuarioProveedorFormasPagoXEmpresaService
    {
        private readonly ISSORepositorio<UsuarioProveedorFormasPagoXempresa> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioProveedorFormasPagoXEmpresaService(
            ISSORepositorio<UsuarioProveedorFormasPagoXempresa> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioFormasPagoXEmpresaDTO> CrearYObtener(UsuarioFormasPagoXEmpresaDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioProveedorFormasPagoXempresa>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioFormasPagoXEmpresaDTO();
            }
            return _Mapper.Map<UsuarioFormasPagoXEmpresaDTO>(objetoCreado);
        }

        public async Task<bool> Editar(UsuarioFormasPagoXEmpresaDTO parametro)
        {
            var objeto = _Mapper.Map<UsuarioProveedorFormasPagoXempresa>(parametro);
            var ObjetoEncontrado = await _Repositorio.Obtener(u => u.Id == objeto.Id);
            if (ObjetoEncontrado == null)
                return false;
            ObjetoEncontrado.EsPue = objeto.EsPue;
            ObjetoEncontrado.EsPpd = objeto.EsPpd;
            var respuesta = await _Repositorio.Editar(ObjetoEncontrado);
            return respuesta;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objeto = await _Repositorio.Obtener(z => z.Id == Id);
            var resultado = await _Repositorio.Eliminar(objeto);
            return resultado;
        }

        public async Task<List<UsuarioFormasPagoXEmpresaDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioFormasPagoXEmpresaDTO>>(lista);
        }

        public async Task<UsuarioFormasPagoXEmpresaDTO> ObtenXId(int Id)
        {
            var lista = await _Repositorio.Obtener(z => z.Id == Id);
            return _Mapper.Map<UsuarioFormasPagoXEmpresaDTO>(lista);
        }

        public async Task<List<UsuarioFormasPagoXEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdEmpresa == IdEmpresa);
            return _Mapper.Map<List<UsuarioFormasPagoXEmpresaDTO>>(lista);
        }

        public async Task<List<UsuarioFormasPagoXEmpresaDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioFormasPagoXEmpresaDTO>>(lista);
        }

        public async Task<UsuarioFormasPagoXEmpresaDTO> ObtenXIdUsuarioEIdEmpresa(int IdUsuario, int IdEmpresa)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            lista = lista.Where(z => z.IdEmpresa == IdEmpresa).ToList();
            if (lista.Count() > 0)
            {
                return _Mapper.Map<UsuarioFormasPagoXEmpresaDTO>(lista.FirstOrDefault());
            }
            else
            {
                return new UsuarioFormasPagoXEmpresaDTO();
            }
        }
    }
}
