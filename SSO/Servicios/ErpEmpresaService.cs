using AutoMapper;
using SistemaERP.BLL.Contrato;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DTO;
using SistemaERP.Model;
using SistemaERP.Models;
using SSO.DTO;

namespace SSO.Servicios
{
    public class ErpEmpresaService : IErpService
    {
        private readonly ISSORepositorio<Erpcorporativo> _Repositorio;
        private readonly ISSORepositorio<Erp> _ErpRepositorio;
        private readonly IMapper _mapper;

        public ErpEmpresaService(ISSORepositorio<Erpcorporativo> ErpEmpresaRepositorio, IMapper mapper, ISSORepositorio<Erp> ErpRepositorio)
        {
            _Repositorio = ErpEmpresaRepositorio; 
            _ErpRepositorio = ErpRepositorio;
            _mapper = mapper;
        }

        public async Task<RespuestaDTO> Crear(ErpCorporativoDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var RpEmpresaCreado = await _Repositorio.Crear(_mapper.Map<Erpcorporativo>(parametro));

                if (RpEmpresaCreado.Id == 0)
                {
                    respuesta.Estatus = false;
                    respuesta.Descripcion = "No se pudo crear la relación Rp - Empresa";
                    return respuesta;
                }
                respuesta.Estatus = true;
                respuesta.Descripcion = "Relacion RP - Empresa creada.";
                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = "Algo salió mal en la creación de la relación Rp - Empresa";
                return respuesta;
            }
        }

        public async Task<ErpCorporativoDTO> CrearYObtener(ErpCorporativoDTO parametro)
        {
            try
            {
                var RpEmpresaCreado = await _Repositorio.Crear(_mapper.Map<Erpcorporativo>(parametro));

                if (RpEmpresaCreado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<ErpCorporativoDTO>(RpEmpresaCreado);

            }
            catch
            {
                throw;
            }
        }

        public async Task<RespuestaDTO> Editar(ErpCorporativoDTO parametro)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var ERPEmpresaModelo = _mapper.Map<Erpcorporativo>(parametro);

                var ERPEmpresaEncontrado = await _Repositorio.Obtener(u => u.Id == ERPEmpresaModelo.Id);

                if (ERPEmpresaModelo == null)
                    throw new TaskCanceledException("El rol no existe");

                ERPEmpresaEncontrado.IdCorporativo = ERPEmpresaModelo.IdCorporativo;
                ERPEmpresaEncontrado.IdErp = ERPEmpresaModelo.IdErp;


                respuesta.Estatus = await _Repositorio.Editar(ERPEmpresaEncontrado);

                if (!respuesta.Estatus)
                    throw new TaskCanceledException("No se pudo editar");

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<RespuestaDTO> Eliminar(int Id)
        {
            RespuestaDTO respuesta = new RespuestaDTO();
            try
            {
                var rpempresaencontrado = await _Repositorio.Obtener(u => u.Id == Id);

                if (rpempresaencontrado == null)
                    throw new TaskCanceledException("El rol no existe");

                respuesta.Estatus = await _Repositorio.Eliminar(rpempresaencontrado);

                if (!respuesta.Estatus)
                    throw new TaskCanceledException("No se pudo eliminar");

                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta.Estatus = false;
                respuesta.Descripcion = ex.Message;
                return respuesta;
            }
        }

        public async Task<List<ErpCorporativoDTO>> ObtenTodos()
        {
            try
            {
                var listarp = await _Repositorio.ObtenerTodos();
                return _mapper.Map<List<ErpCorporativoDTO>>(listarp.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<ErpDTO>> ObtenErps()
        {
            try
            {
                var listarp = await _ErpRepositorio.ObtenerTodos();
                return _mapper.Map<List<ErpDTO>>(listarp.ToList());
            }
            catch
            {
                throw;
            }
        }

        public async Task<ErpCorporativoDTO> ObtenXIdCorporativo(int IdCorporativo)
        {
            try
            {
                var listaRpempresa = await _Repositorio.Obtener(z => z.IdCorporativo == IdCorporativo);
                return _mapper.Map<ErpCorporativoDTO>(listaRpempresa);
            }
            catch (Exception ex)
            {
                return new ErpCorporativoDTO();
            }
        }
    }
}
