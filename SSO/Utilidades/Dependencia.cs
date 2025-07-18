using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.BLL;
using SistemaERP.BLL.ServicioSSO;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.SubProcesoSSO;
using SistemaERP.BLL.ContratoSSO;
using Microsoft.EntityFrameworkCore;
using SistemaERP.DAL.Repositorios.Contrato;
using SistemaERP.DAL.Repositorios;
using SistemaERP.BLL.Contrato.Utilidades;
using SistemaERP.BLL.Servicios.Utilidades;
using GuardarArchivos;
using SSO.Servicios;
using SSO.Servicios.Contratos;
using SSO.Procesos;


namespace Utilidades
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped(typeof(ISSORepositorio<>), typeof(SSORepositorio<>));
            services.AddAutoMapper(typeof(AutoMapperProfile));
            //Sistema SSO
            //Estructura del cliente
            services.AddScoped<ICorporativoService, CorporativoService>();
            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IParametrosEmpresaService, ParametrosEmpresaService>();
            services.AddScoped<IDivisionService, DivisionService>();
            services.AddScoped<IUsuariosUltimaSeccionService, UsuariosUltimaSeccionServicio>();
            services.AddScoped(typeof(IParametrosEmpresaGastosService), typeof(ParametrosEmpresaGastosService));
            services.AddScoped<IErpService, ErpEmpresaService>();
            //Menus
            services.AddScoped<ICatalogoMenuService, CatalogoMenuService>();
            services.AddScoped<ICatalogoSeccionService, CatalogoSeccionService>();
            services.AddScoped<ICatalogoActividadService, CatalogoActividadService>();
            //Roles
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IRolSeccionService, RolSeccionService>();
            services.AddScoped<IRolEmpresaService, RolEmpresaService>();
            services.AddScoped<IRolActividadService, RolActividadService>();

            //Usuario
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUsuarioSeccionService, UsuarioSeccionService>();
            services.AddScoped<IUsuarioActividadService, UsuarioActividadService>();
            services.AddScoped<IUsuarioCorporativoService, UsuarioCorporativoService>();
            services.AddScoped<IUsuarioEmpresaService, UsuarioEmpresaService>();
            services.AddScoped<IUsuarioDivisionService, UsuarioDivisionService>();
            services.AddScoped<IUsuarioProveedorService, UsuarioProveedorService>();
            services.AddScoped<IUsuarioProveedorFormasPagoXEmpresaService, UsuarioProveedorFormasPagoXEmpresaService>();
            services.AddScoped<IUsuarioProyectoService, UsuarioProyectoService>();
            services.AddScoped<IUsuarioClasificacionCfdiService, UsuarioClasificacionCfdiService>();
            services.AddScoped<IUsuarioPertenecienteAClienteService, UsuarioPertenecienteAClienteService>();
            services.AddScoped<IUsuarioGastosService, UsuarioGastosService>();
            services.AddScoped<IUsuarioAutoFacService, UsuarioAutoFacService>();
            services.AddScoped<IUsuarioEmpresaPorDefectoService, UsuarioEmpresaPorDefectoService>();
            services.AddScoped<IMenuEmpresaService, MenuEmpresaService>();
            services.AddScoped<IRolProyectoEmpresaUsuarioService, RolProyectoEmpresaUsuarioService>();
            //Procesos SSO
            services.AddScoped(typeof(ProcesoRelacionUsuarioEmpresa));
            services.AddScoped<ProcesoRelacionUsuarioEmpresa>();
            services.AddScoped<ProcesoUsuarioCreacion>();
            services.AddScoped<ProcesoUsuariosBusqueda>();
            services.AddScoped<ProcesoUsuarioCorporativo>();
            services.AddScoped<ProcesoRol>();
            services.AddScoped<ProcesoUsuariosUltimaSeccion>();
            services.AddScoped<ProcesoUsuarioFormaPagoPermitidaPorEmpresa>();
            services.AddScoped<ObtenRolesXCorporativo>();
            services.AddScoped<ObtenEmpresasEnRoles>();
            services.AddScoped<EstructuraServicios>();
            services.AddScoped<ProcesoUsuarioProveedorCrearConPermisos>();
            services.AddScoped<ProcesoRelacionUsuarioPermisosPorEmpresa>();
            services.AddScoped<ProcesoArchivosTimbradoEmpresa>();
            services.AddScoped<GuardarArchivosC>();
            services.AddScoped<EmpresaService>();

            // archivo
            services.AddScoped(typeof(IArchivoRutaService), typeof(ArchivoRutaService));
            services.AddScoped(typeof(IArchivosEmpresaService), typeof(ArchivosEmpresaService));
            //rsa
            services.AddScoped(typeof(IRsaCertificateService), typeof(RsaCertificateService));



        }
    }
}