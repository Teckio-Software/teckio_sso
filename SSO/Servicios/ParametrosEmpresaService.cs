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
    public class ParametrosEmpresaService : IParametrosEmpresaService
    {
        private readonly ISSORepositorio<ParametrosEmpresa> _parametrosempresaRepositorio;
        private readonly IMapper _mapper;

        public ParametrosEmpresaService(ISSORepositorio<ParametrosEmpresa> parametrosempresaRepositorio, IMapper mapper)
        {
            _parametrosempresaRepositorio = parametrosempresaRepositorio;
            _mapper = mapper;
        }

        public async Task<ParametrosEmpresaDTO> Crear(ParametrosEmpresaDTO modelo)
        {
            try
            {
                var ParametrosEmpresaCreado = await _parametrosempresaRepositorio.Crear(_mapper.Map<ParametrosEmpresa>(modelo));

                if (ParametrosEmpresaCreado.Id == 0)
                    throw new TaskCanceledException("No se pudo crear");

                return _mapper.Map<ParametrosEmpresaDTO>(ParametrosEmpresaCreado);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> Editar(ParametrosEmpresaDTO modelo)
        {
            var parametrosempresaModelo = _mapper.Map<ParametrosEmpresa>(modelo);

            var parametroempresaEncontrado = await _parametrosempresaRepositorio.Obtener(u => u.Id == parametrosempresaModelo.Id);

            if (parametroempresaEncontrado == null)
                throw new TaskCanceledException("El parametro de la empresa no existe");

            parametroempresaEncontrado.IdEmpresa = parametrosempresaModelo.IdEmpresa;
            parametroempresaEncontrado.DiaMaximoCargaCfdi = parametrosempresaModelo.DiaMaximoCargaCfdi;
            parametroempresaEncontrado.DiasFinMes = parametrosempresaModelo.DiasFinMes;

            parametroempresaEncontrado.ToleranciaCfdi = parametrosempresaModelo.ToleranciaCfdi;
            parametroempresaEncontrado.RequiereNotificacion = parametrosempresaModelo.RequiereNotificacion;
            parametroempresaEncontrado.RevalidaMesAnterior = parametrosempresaModelo.RevalidaMesAnterior;
            parametroempresaEncontrado.ValidaPptoWs = parametrosempresaModelo.ValidaPptoWs;

            parametroempresaEncontrado.DecimalesTolerancia = parametrosempresaModelo.DecimalesTolerancia;
            parametroempresaEncontrado.ToleranciaUniversal = parametrosempresaModelo.ToleranciaUniversal;
            parametroempresaEncontrado.MontoToleranciaMxn = parametrosempresaModelo.MontoToleranciaMxn;
            parametroempresaEncontrado.FacturaDirecto = parametrosempresaModelo.FacturaDirecto;
            parametroempresaEncontrado.AsignacionDirectaObligada = parametrosempresaModelo.AsignacionDirectaObligada;

            bool respuesta = await _parametrosempresaRepositorio.Editar(parametroempresaEncontrado);

            if (!respuesta)
                throw new TaskCanceledException("No se pudo editar");

            return respuesta;
        }

        public async Task<List<ParametrosEmpresaDTO>> Lista(int id)
        {
            try
            {
                var queryEmpresa = await _parametrosempresaRepositorio.Consultar(empresa => empresa.IdEmpresa == id);

                return _mapper.Map<List<ParametrosEmpresaDTO>>(queryEmpresa);
            }
            catch
            {
                throw;
            }
        }

    }
}
