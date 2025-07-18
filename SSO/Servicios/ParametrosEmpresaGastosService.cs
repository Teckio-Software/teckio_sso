using AutoMapper;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO.SSO;
using SistemaERP.Model.SSO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class ParametrosEmpresaGastosService : IParametrosEmpresaGastosService
    {
        private readonly ISSORepositorio<ParametrosEmpresaGastos> _parametrosempresaRepositorio;
        private readonly IMapper _mapper;

        public ParametrosEmpresaGastosService(ISSORepositorio<ParametrosEmpresaGastos> parametrosempresaRepositorio, IMapper mapper)
        {
            _parametrosempresaRepositorio = parametrosempresaRepositorio;
            _mapper = mapper;
        }

        public async Task<ParametrosEmpresaGastosDTO> Crear(ParametrosEmpresaGastosDTO modelo)
        {
            try
            {
                var ParametrosEmpresaCreado = await _parametrosempresaRepositorio.Crear(_mapper.Map<ParametrosEmpresaGastos>(modelo));

                if (ParametrosEmpresaCreado.id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<ParametrosEmpresaGastosDTO>(ParametrosEmpresaCreado);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ParametrosEmpresaGastosDTO> Editar(ParametrosEmpresaGastosDTO modelo)
        {
            var parametrosempresaModelo = _mapper.Map<ParametrosEmpresaGastosDTO>(modelo);

            var parametroempresaEncontrado = await _parametrosempresaRepositorio.Obtener(u => u.id == parametrosempresaModelo.id);

            if (parametroempresaEncontrado == null)
                throw new TaskCanceledException("El parametro de la empresa no existe");

            parametroempresaEncontrado.idEmpresa = parametrosempresaModelo.idEmpresa;

            parametroempresaEncontrado.Permiso = parametrosempresaModelo.Permiso;
            parametroempresaEncontrado.Valor = parametrosempresaModelo.Valor;
            parametroempresaEncontrado.Estatus = parametrosempresaModelo.Estatus;
            parametroempresaEncontrado.EditablexUsuario = parametrosempresaModelo.EditablexUsuario;

            var respuesta = await _parametrosempresaRepositorio.Editar(parametroempresaEncontrado);

            if (!respuesta)
                throw new TaskCanceledException("No se pudo editar");

            return _mapper.Map<ParametrosEmpresaGastosDTO>(parametroempresaEncontrado);

        }

        //public Task<bool> Eliminar(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<List<ParametrosEmpresaGastosDTO>> ObtenerXPermiso(string Permiso)
        //{
        //    try
        //    {
        //        var queryEmpresa = await _parametrosempresaRepositorio.Consultar(empresa => empresa.Permiso == Permiso);

        //        return _mapper.Map<List<ParametrosEmpresaGastosDTO>>(queryEmpresa);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public async Task<List<ParametrosEmpresaGastosDTO>> ObtenTodosxEmpresa(int idEmpresa)
        {
            try
            {
                var queryEmpresa = await _parametrosempresaRepositorio.ObtenerTodos(empresa => empresa.idEmpresa == idEmpresa);

                return _mapper.Map<List<ParametrosEmpresaGastosDTO>>(queryEmpresa);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ParametrosEmpresaGastosDTO> ObtenXId(int id)
        {
            try
            {
                var queryEmpresa = await _parametrosempresaRepositorio.Obtener(empresa => empresa.id == id);

                return _mapper.Map<ParametrosEmpresaGastosDTO>(queryEmpresa);
            }
            catch
            {
                throw;
            }
        }

        public async Task<ParametrosEmpresaGastosDTO> ObtenXPermiso(string Permiso)
        {
            try
            {
                var queryEmpresa = await _parametrosempresaRepositorio.Obtener(empresa => empresa.Permiso == Permiso);

                return _mapper.Map<ParametrosEmpresaGastosDTO>(queryEmpresa);
            }
            catch
            {
                throw;
            }
        }

    }
}
