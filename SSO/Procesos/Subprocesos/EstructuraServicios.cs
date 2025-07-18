using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaERP.BLL.ContratoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.SubProcesoSSO
{
    /// <summary>
    /// Para mantener una estructura de los servicios de RFacil
    /// </summary>
    public class EstructuraServicios
    {
        private readonly ICatalogoMenuService _CatalogoMenuService;
        private readonly ICatalogoSeccionService _CatalogoSeccionService;
        private readonly ICatalogoActividadService _CatalogoActividadService;
        private readonly IMenuEmpresaService _MenuEmpresaService;
        /// <summary>
        /// Convierte los registros nativos del dbContext a los DTO para enviar información
        /// </summary>
        private readonly IMapper _Mapper;
        public EstructuraServicios(
            IMapper zMapper
            , ICatalogoMenuService CatalogoMenuService
            , ICatalogoSeccionService CatalogoSeccionService
            , ICatalogoActividadService CatalogoActividadService
            , IMenuEmpresaService MenuEmpresaService
            , ICatalogoMenuService menuService
            , ICatalogoSeccionService SeccionService
            , ICatalogoActividadService ActividadService
            )
        {
            _Mapper = zMapper;
            _MenuEmpresaService = MenuEmpresaService;
            _CatalogoMenuService = CatalogoMenuService;
            _CatalogoSeccionService = CatalogoSeccionService;
            _CatalogoActividadService = CatalogoActividadService;
        }
        /// <summary>
        /// Obtiene la estructura de los servicios dados de alta
        /// </summary>
        /// <returns></returns>
        public async Task<List<CatalogoSeccionConMenuDTO>> ObtenEstructuraSerivicios()
        {
            var menus = await _CatalogoMenuService.ObtenTodos();
            var secciones = await _CatalogoSeccionService.ObtenTodos();
            var actividades = await _CatalogoActividadService.ObtenTodos();
            List<CatalogoSeccionConMenuDTO> seccionesEstructura = new List<CatalogoSeccionConMenuDTO>();
            for (int i = 0; i < secciones.Count; i++)
            {
                var actividadesFiltrado = actividades.Where(z => z.IdSeccion == secciones[i].Id).ToList();
                var actividadesFiltradoParaRol = _Mapper.Map<List<CatalogoActividadParaRolDTO>>(actividadesFiltrado);
                var menuFiltrado = menus.Where(z => z.Id == secciones[i].IdMenu).ToList();
                seccionesEstructura.Add(new CatalogoSeccionConMenuDTO()
                {
                    IdMenu = secciones[i].Id,
                    Id = secciones[i].Id,
                    Descripcion = secciones[i].Descripcion!,
                    Actividades = actividadesFiltradoParaRol,
                    CodigoSeccion = secciones[i].CodigoSeccion,
                    DescripcionInterna = secciones[i].DescripcionInterna,
                    DescripcionMenu = menuFiltrado[0].Descripcion,
                    EsSeccionUnica = secciones[i].EsSeccionUnica,
                    EsSeleccionado = false
                });
            }
            seccionesEstructura = seccionesEstructura.OrderBy(z => z.DescripcionMenu).ToList();
            return seccionesEstructura;
        }
    }
}
