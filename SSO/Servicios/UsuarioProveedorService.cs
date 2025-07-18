using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO.SSO;
using SistemaERP.Model;

namespace SistemaERP.BLL.ServicioSSO
{
    public class UsuarioProveedorService : IUsuarioProveedorService
    {
        private readonly ISSORepositorio<UsuarioProveedor> _Repositorio;
        private readonly IMapper _Mapper;

        public UsuarioProveedorService(
            ISSORepositorio<UsuarioProveedor> Repositorio
            , IMapper mapper)
        {
            _Repositorio = Repositorio;
            _Mapper = mapper;
        }

        public async Task<UsuarioProveedorDTO> Crear(UsuarioProveedorDTO parametro)
        {
            var objetoCreado = await _Repositorio.Crear(_Mapper.Map<UsuarioProveedor>(parametro));
            if (objetoCreado.Id <= 0)
            {
                return new UsuarioProveedorDTO();
            }
            return _Mapper.Map<UsuarioProveedorDTO>(objetoCreado);
        }

        public async Task<bool> Editar(UsuarioProveedorDTO parametro)
        {
            var objeto = _Mapper.Map<UsuarioProveedor>(parametro);
            var ObjetoEncontrado = await _Repositorio.Obtener(u => u.Id == objeto.Id);
            if (ObjetoEncontrado == null)
                return false;
            ObjetoEncontrado.NumeroProveedor = objeto.NumeroProveedor;
            ObjetoEncontrado.Rfc = objeto.Rfc;
            ObjetoEncontrado.IdentificadorFiscal = objeto.IdentificadorFiscal;
            var respuesta = await _Repositorio.Editar(ObjetoEncontrado);
            return respuesta;
        }

        public async Task<bool> Eliminar(int Id)
        {
            var objeto = await _Repositorio.Obtener(z => z.Id == Id);
            var resultado = await _Repositorio.Eliminar(objeto);
            return resultado;
        }

        public async Task<List<UsuarioProveedorDTO>> ObtenTodos()
        {
            var lista = await _Repositorio.ObtenerTodos();
            return _Mapper.Map<List<UsuarioProveedorDTO>>(lista);
        }

        public async Task<UsuarioProveedorDTO> ObtenXId(int Id)
        {
            var lista = await _Repositorio.Obtener(z => z.Id == Id);
            return _Mapper.Map<UsuarioProveedorDTO>(lista);
        }

        public async Task<List<UsuarioProveedorDTO>> ObtenXIdentificadorFiscal(string IdentificadorFiscal)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdentificadorFiscal == IdentificadorFiscal);
            return _Mapper.Map<List<UsuarioProveedorDTO>>(lista);
        }

        public async Task<List<UsuarioProveedorDTO>> ObtenXIdUsuario(int IdUsuario)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.IdUsuario == IdUsuario);
            return _Mapper.Map<List<UsuarioProveedorDTO>>(lista);
        }

        public async Task<List<UsuarioProveedorDTO>> ObtenXNumeroProveedor(string NumeroProveedor)
        {
            var lista = await _Repositorio.ObtenerTodos(z => z.NumeroProveedor == NumeroProveedor);
            return _Mapper.Map<List<UsuarioProveedorDTO>>(lista);
        }

        public async Task<UsuarioProveedorDTO> ObtenXRfc(string Rfc)
        {
            var lista = await _Repositorio.Obtener(z => z.Rfc.ToLower() == Rfc.ToLower());
            return _Mapper.Map<UsuarioProveedorDTO>(lista);
        }
    }
}
