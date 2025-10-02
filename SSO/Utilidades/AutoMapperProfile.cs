using System.Drawing;
using AutoMapper;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SistemaERP.DTO.SSO;
using SistemaERP.Model;
using SistemaERP.Model.Gastos;
using SistemaERP.Model.SSO;
using SistemaERP.Models;
using SSO.DTO;
using SSO.Modelos;

namespace Utilidades
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region SSO
            #region Corporativo
            CreateMap<CorporativoDTO, Corporativo>();
            CreateMap<Corporativo, CorporativoDTO>();
            #endregion
            #region Empresa
            CreateMap<Empresa, EmpresaDTO>()
                .ForMember(destino => destino.Estatus,
                opt => opt.MapFrom(origen => origen.Estatus == true ? 1 : 0));
            CreateMap<EmpresaDTO, Empresa>()
           .ForMember(destino => destino.IdCorporativoNavigation,
                opt => opt.Ignore())
           .ForMember(destino => destino.Logs, opt => opt.Ignore());

            #endregion
            #region Division
            CreateMap<Division, DivisionDTO>();
            CreateMap<DivisionDTO, Division>()
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore());
            #endregion
            #region ClasificacionesCfdi
            CreateMap<ClasificacionesCfdi, ClasificacionCfdiDTO>();
            CreateMap<ClasificacionCfdiDTO, ClasificacionesCfdi>()
                .ForMember(destino => destino.IdCorporacionNavigation,
                opt => opt.Ignore());
            #endregion
            #region MenuEmpresa
            CreateMap<MenuEmpresa, MenuEmpresaDTO>();
            CreateMap<MenuEmpresaDTO, MenuEmpresa>()
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdMenuNavigation,
                opt => opt.Ignore());
            #endregion

            #region CatalogoMenu
            CreateMap<CatalogoMenu, CatalogoMenuDTO>();
            CreateMap<CatalogoMenuDTO, CatalogoMenu>();
            #endregion
            #region CatalogoSeccion
            CreateMap<CatalogoSeccion, CatalogoSeccionDTO>();
            CreateMap<CatalogoSeccionDTO, CatalogoSeccion>()
                .ForMember(destino => destino.IdMenuNavigation,
                opt => opt.Ignore());
            #endregion
            #region CatalogoActividad
            CreateMap<CatalogoActividad, CatalogoActividadDTO>();
            CreateMap<CatalogoActividadDTO, CatalogoActividad>()
                .ForMember(destino => destino.IdSeccionNavigation,
                opt => opt.Ignore());
            #endregion

            #region Rol
            CreateMap<Rol, RolDTO>();
            CreateMap<RolDTO, Rol>();
            #endregion
            #region RolEmpresa
            CreateMap<RolEmpresa, RolEmpresaDTO>();
            CreateMap<RolEmpresaDTO, RolEmpresa>();
            #endregion
            #region RolSeccion
            CreateMap<RolSeccion, RolSeccionDTO>();
            CreateMap<RolSeccionDTO, RolSeccion>()
                .ForMember(destino => destino.IdRolNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdSeccionNavigation,
                opt => opt.Ignore());
            #endregion
            #region RolActividad
            CreateMap<RolActividad, RolActividadDTO>();
            CreateMap<RolActividadDTO, RolActividad>()
                .ForMember(destino => destino.IdActividadNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdRolNavigation,
                opt => opt.Ignore());
            #endregion

            #region Usuario
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<UsuarioDTO, Usuario>()
           .ForMember(destino => destino.Logs, opt => opt.Ignore());

            #endregion
            #region UsuarioSeccion
            CreateMap<UsuarioSeccion, UsuarioSeccionDTO>();
            CreateMap<UsuarioSeccionDTO, UsuarioSeccion>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdSeccionNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioActividad
            CreateMap<UsuarioActividad, UsuarioActividadDTO>();
            CreateMap<UsuarioActividadDTO, UsuarioActividad>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdActividadNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioCorporativo
            CreateMap<UsuarioCorporativo, UsuarioCorporativoDTO>();
            CreateMap<UsuarioCorporativoDTO, UsuarioCorporativo>()
                .ForMember(destino => destino.IdCorporativoNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioEmpresa
            CreateMap<UsuarioEmpresa, UsuarioEmpresaDTO>();
            CreateMap<UsuarioEmpresaDTO, UsuarioEmpresa>()
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioDivision
            CreateMap<UsuarioDivision, UsuarioDivisionDTO>();
            CreateMap<UsuarioDivisionDTO, UsuarioDivision>()
                .ForMember(destino => destino.IdDivisionNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioProveedor
            CreateMap<UsuarioProveedor, UsuarioProveedorDTO>();
            CreateMap<UsuarioProveedorDTO, UsuarioProveedor>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region Usuario Gastos
            CreateMap<UsuarioGastos, UsuarioGastosDTO>();
            CreateMap<UsuarioGastosDTO, UsuarioGastos>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioProyecto
            CreateMap<UsuarioProyecto, UsuarioProyectoDTO>();
            CreateMap<UsuarioProyectoDTO, UsuarioProyecto>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioClasificacionCfdi
            CreateMap<UsuarioClasificacionCfdi, UsuarioClasificacionCfdiDTO>();
            CreateMap<UsuarioClasificacionCfdiDTO, UsuarioClasificacionCfdi>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdClasificacionCfdiNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioEmpresaPorDefecto
            CreateMap<UsuarioEmpresaPorDefecto, UsuarioEmpresaPorDefectoDTO>();
            CreateMap<UsuarioEmpresaPorDefectoDTO, UsuarioEmpresaPorDefecto>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuarioPertenecienteACliente
            CreateMap<UsuarioPertenecienteACliente, UsuarioPertenecienteAClienteDTO>();
            CreateMap<UsuarioPertenecienteAClienteDTO, UsuarioPertenecienteACliente>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdCorporativoNavigation,
                opt => opt.Ignore());
            #endregion
            #region UsuariosUltimaSeccion
            CreateMap<UsuariosUltimaSeccion, UsuariosUltimaSeccionDTO>();
            CreateMap<UsuariosUltimaSeccionDTO, UsuariosUltimaSeccion>()
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdUsuarioNavigation
                , opt => opt.Ignore());
            #endregion
            #region Actividad-ActividadParaRol
            CreateMap<CatalogoActividadDTO, CatalogoActividadParaRolDTO>()
                .ForMember(destino => destino.EsSeleccionado,
                opt => opt.MapFrom(origen => origen.EsActividadUnica ? false : false));
            CreateMap<CatalogoActividadParaRolDTO, CatalogoActividadDTO>();
            #endregion

            #region ParametrosEmpresaGastos
            CreateMap<ParametrosEmpresaGastos, ParametrosEmpresaGastosDTO>();
            CreateMap<ParametrosEmpresaGastosDTO, ParametrosEmpresaGastos>();
            #endregion

            #endregion
            #region ParametrosEmpresa
            CreateMap<ParametrosEmpresa, ParametrosEmpresaDTO>();
            CreateMap<ParametrosEmpresaDTO, ParametrosEmpresa>();
            #endregion

            #region RpEmpresa
            CreateMap<Erpcorporativo, ErpCorporativoDTO>();
            CreateMap<ErpCorporativoDTO, Erpcorporativo>();

            CreateMap<Erp, ErpDTO>();
            CreateMap<ErpDTO, Erp>();

            CreateMap<ParametrosTimbrado, ParametrosTimbradoDTO>();
            CreateMap<ParametrosTimbradoDTO, ParametrosTimbrado>();
                
            #endregion


            #region UsuarioEmpresaFormaPago
            CreateMap<UsuarioProveedorFormasPagoXempresa, UsuarioFormasPagoXEmpresaDTO>();
            CreateMap<UsuarioFormasPagoXEmpresaDTO, UsuarioProveedorFormasPagoXempresa>();
            #endregion


            #region Usuario Gastos
            CreateMap<UsuarioGastos, UsuarioGastosDTO>();
            CreateMap<UsuarioGastosDTO, UsuarioGastos>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion

            #region Usuario AutoFac
            CreateMap<UsuarioAutoFac, UsuarioAutoFacDTO>();
            CreateMap<UsuarioAutoFacDTO, UsuarioAutoFac>()
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore());
            #endregion
            #region LoginTeckio
            CreateMap<RolProyectoEmpresaUsuario, RolProyectoEmpresaUsuarioDTO>();
            CreateMap<RolProyectoEmpresaUsuarioDTO, RolProyectoEmpresaUsuario>()
                .ForMember(destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdUsuarioNavigation,
                opt => opt.Ignore())
                .ForMember(destino => destino.IdRolNavigation,
                opt => opt.Ignore());
            #endregion

            #region Log

            CreateMap<LogRegistro, LogDTO>();
            CreateMap<LogDTO, LogRegistro>()
                .ForMember(destino => destino.IdEmpresaNavigation, opt => opt.Ignore())
                .ForMember(destino => destino.IdUsuarioNavigation, opt => opt.Ignore());

            #endregion

        }
    }
}