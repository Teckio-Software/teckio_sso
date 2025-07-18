using AutoMapper;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.BLL.Contrato;
using Microsoft.EntityFrameworkCore;
using SistemaERP.Model;
using System.Reflection.Metadata.Ecma335;
using SistemaERP.DTO.SSO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class EmpresaService : IEmpresaService
    {
        private readonly ISSORepositorio<Empresa> _Repositorio;
        private readonly IMapper _Mapper;

        public EmpresaService(ISSORepositorio<Empresa> empresaRepositorio, IMapper mapper)
        {
            _Repositorio = empresaRepositorio;
            _Mapper = mapper;
        }

        public async Task<List<EmpresaDTO>> ObtenTodos()
        {
            try
            {
                var queryEmpresa = await _Repositorio.ObtenerTodos();


                return _Mapper.Map<List<EmpresaDTO>>(queryEmpresa);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<EmpresaDTO>> ObtenXIdCorporativo(int IdCorporativo)
        {
            try
            {
                var queryEmpresa = await _Repositorio.ObtenerTodos(z => z.IdCorporativo == IdCorporativo);
                return _Mapper.Map<List<EmpresaDTO>>(queryEmpresa);
            }
            catch (Exception ex)
            {
                return new List<EmpresaDTO>();
            }
        }

        public async Task<EmpresaDTO> CrearYObtener(EmpresaDTO modelo)
        {
            try
            {
                var EmpresaCreado = await _Repositorio.Crear(_Mapper.Map<Empresa>(modelo));

                if (EmpresaCreado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _Mapper.Map<EmpresaDTO>(EmpresaCreado);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Editar(EmpresaDTO modelo)
        {
            var empresaModelo = _Mapper.Map<Empresa>(modelo);

            var empresaEncontrado = await _Repositorio.Obtener(u => u.Id == empresaModelo.Id);
            if (empresaEncontrado == null)
            {
                return false;
            }
            empresaEncontrado.Rfc = empresaModelo.Rfc;
            empresaEncontrado.CodigoPostal = empresaModelo.CodigoPostal;
            empresaEncontrado.Estatus = empresaModelo.Estatus;
            empresaEncontrado.Sociedad = empresaModelo.Sociedad;

            bool respuesta = await _Repositorio.Editar(empresaEncontrado);

            if (!respuesta)
            {
                return false;
            }
            return respuesta;
        }

        public async Task<EmpresaDTO> ObtenXId(int Id)
        {
            try
            {
                var queryEmpresa = await _Repositorio.Obtener(z => z.Id == Id);
                return _Mapper.Map<EmpresaDTO>(queryEmpresa);
            }
            catch (Exception ex)
            {
                return new EmpresaDTO();
            }
        }

        public async Task<EmpresaDTO> ObtenXRFC(string rfc)
        {
            try
            {
                var queryEmpresa = await _Repositorio.Obtener(z => z.Rfc == rfc);
                return _Mapper.Map<EmpresaDTO>(queryEmpresa);
            }
            catch (Exception ex)
            {
                return new EmpresaDTO();
            }
        }

        public async Task<bool> Crear(EmpresaDTO parametro)
        {
            parametro.GuidEmpresa = Guid.NewGuid().ToString();
            var EmpresaCreado = await _Repositorio.Crear(_Mapper.Map<Empresa>(parametro));
            if (EmpresaCreado.Id == 0)
                return false;

            return true;
        }

        public async Task<EmpresaDTO> ObtenXSociedad(string sociedad)
        {
            var queryEmpresa = await _Repositorio.Obtener(z => z.Sociedad == sociedad);
            return _Mapper.Map<EmpresaDTO>(queryEmpresa);
        }
    }
}
