using AutoMapper;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.BLL.Contrato;
using Microsoft.EntityFrameworkCore;
using SistemaERP.Model;
using SistemaERP.DTO.SSO;

namespace SistemaERP.BLL.ServicioSSO
{
    public class CorporativoService : ICorporativoService
    {
        private readonly ISSORepositorio<Corporativo> _corporativoRepositorio;
        private readonly IMapper _mapper;

        public CorporativoService(ISSORepositorio<Corporativo> corporativoRepositorio, IMapper mapper)
        {
            _corporativoRepositorio = corporativoRepositorio;
            _mapper = mapper;
        }

        public async Task<List<CorporativoDTO>> ObtenTodos()
        {
            try
            {
                var queryCorporativo = await _corporativoRepositorio.ObtenerTodos();
                return _mapper.Map<List<CorporativoDTO>>(queryCorporativo);
            }
            catch
            {
                throw;
            }
        }

        public async Task<CorporativoDTO> Crear(CorporativoDTO modelo)
        {
            try
            {
                var corporativoCreado = await _corporativoRepositorio.Crear(_mapper.Map<Corporativo>(modelo));

                if (corporativoCreado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<CorporativoDTO>(corporativoCreado);

            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> Editar(CorporativoDTO modelo)
        {
            var corporativoModelo = _mapper.Map<Corporativo>(modelo);

            var corporativoEncontrado = await _corporativoRepositorio.Obtener(u => u.Id == corporativoModelo.Id);

            if (corporativoEncontrado == null)
                throw new TaskCanceledException("El corporativo no existe");

            corporativoEncontrado.Nombre = corporativoModelo.Nombre;
            corporativoEncontrado.Estatus = corporativoModelo.Estatus;

            bool respuesta = await _corporativoRepositorio.Editar(corporativoEncontrado);

            if (!respuesta)
                throw new TaskCanceledException("No se pudo editar");

            return respuesta;

        }

        public async Task<bool> Eliminar(int Id)
        {
            try
            {
                var corporativoEncontrado = await _corporativoRepositorio.Obtener(u => u.Id == Id);

                if (corporativoEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                bool respuesta = await _corporativoRepositorio.Eliminar(corporativoEncontrado);

                if (!respuesta)
                    throw new TaskCanceledException("No se pudo eliminar");

                return respuesta;

            }
            catch
            {
                throw;
            }

        }

        public async Task<CorporativoDTO> ObtenXId(int IdCorporativo)
        {
            try
            {
                var queryCorporativo = await _corporativoRepositorio.Obtener(z => z.Id == IdCorporativo);
                //var listaCorporativo = queryCorporativo.Include(Id => Id.Id).ToList();

                return _mapper.Map<CorporativoDTO>(queryCorporativo);
            }
            catch (Exception ex)
            {
                return new CorporativoDTO();
            }
        }
    }
}
