using SistemaERP.DTO;
using SistemaERP.DTO.SSO;
using SSO.DTO;

namespace SistemaERP.BLL.Contrato
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDTO>> ObtenTodos();
        Task<List<UsuarioDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<UsuarioDTO> ObtenXIdUsuario(int IdUsuario);
        Task<UsuarioDTO> ObtenXGuidUsuario(string GuidUsuario);
        Task<List<UsuarioDTO>> ObtenXEmailUsuario(string Email);
        Task<UsuarioDTO> ObtenXUsername(string Username);
        Task<UsuarioDTO> CrearYObtener(UsuarioDTO parametro);
        Task<RespuestaDTO> Editar(UsuarioDTO parametro);
        Task<RespuestaDTO> Desactivar(int IdUsuario);
        Task<RespuestaDTO> Activar(int IdUsuario);
    }
    public interface IUsuarioSeccionService
    {
        Task<List<UsuarioSeccionDTO>> ObtenTodos();
        Task<UsuarioSeccionDTO> ObtenUsuariosXId(int Id);
        Task<List<UsuarioSeccionDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioSeccionDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa);
        Task<List<UsuarioSeccionDTO>> ObtenXIdSeccion(int IdSeccion);
        Task<RespuestaDTO> DesactivarPermisoUsuario(int Id);
        Task<RespuestaDTO> ActivarPermisoUsuario(UsuarioSeccionDTO parametro);
    }

    public interface IUsuarioActividadService
    {
        Task<List<UsuarioActividadDTO>> ObtenTodos();
        Task<UsuarioActividadDTO> ObtenXId(int Id);
        Task<List<UsuarioActividadDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioActividadDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa);
        Task<List<UsuarioActividadDTO>> ObtenXIdActividad(int IdActividad);
        Task<RespuestaDTO> DesactivarPermisoUsuario(int Id);
        Task<RespuestaDTO> ActivarPermisoUsuario(UsuarioActividadDTO parametro);
    }
    //Los permisos de un usuario con un corporativo es que el usuario-corporativo puede dar de alta sus empresas, sucursales y demás cosas, (no es operativo, pero podría).
    /// <summary>
    /// Relación de los usuarios con los corporativos (si esta activo es que es usuario-corporativo)
    /// </summary>
    public interface IUsuarioCorporativoService
    {
        Task<List<UsuarioCorporativoDTO>> ObtenTodos();
        Task<UsuarioCorporativoDTO> ObtenXIdUsuario(int idUsuario);
        Task<List<UsuarioCorporativoDTO>> ObtenXIdCorporativo(int idCorporativo);
        Task<List<UsuarioCorporativoDTO>> ObtenXIdUsuarioXIdCorporativo(int idCorporativo, int idUsuario);
        Task<UsuarioCorporativoDTO> CrearYObtener(UsuarioCorporativoDTO modelo);
        Task<bool> Crear(UsuarioCorporativoDTO modelo);
        Task<bool> Eliminar(int Id);
    }
    /// <summary>
    /// Relación entre el usuario con cada una de las empresa
    /// </summary>
    public interface IUsuarioEmpresaService
    {
        Task<List<UsuarioEmpresaDTO>> ObtenTodos();
        Task<List<UsuarioEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<List<UsuarioEmpresaDTO>> ObtenXIdRol(int IdRol);
        Task<List<UsuarioEmpresaDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioEmpresaDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa);
        Task<bool> CambiarRol(UsuarioEmpresaDTO parametro);
        Task<RespuestaDTO> Desactivar(UsuarioEmpresaDTO parametro);
        Task<RespuestaDTO> Activar(UsuarioEmpresaDTO parametro);
    }
    public interface IUsuarioDivisionService
    {
        Task<List<UsuarioDivisionDTO>> ObtenTodos();
        Task<List<UsuarioDivisionDTO>> ObtenXIdDivision(int IdDivision);
        Task<List<UsuarioDivisionDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioDivisionDTO>> ObtenXIdUsuarioXIdDivision(int IdUsuario, int IdDivision);
        Task<UsuarioDivisionDTO> Crear(UsuarioDivisionDTO parametro);
        Task<bool> Eliminar(int Id);
    }
    public interface IUsuarioProveedorService
    {
        Task<List<UsuarioProveedorDTO>> ObtenTodos();
        Task<List<UsuarioProveedorDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<UsuarioProveedorDTO> ObtenXId(int Id);
        Task<UsuarioProveedorDTO> ObtenXRfc(string Rfc);
        Task<List<UsuarioProveedorDTO>> ObtenXNumeroProveedor(string NumeroProveedor);
        Task<List<UsuarioProveedorDTO>> ObtenXIdentificadorFiscal(string IdentificadorFiscal);
        Task<UsuarioProveedorDTO> Crear(UsuarioProveedorDTO parametro);
        Task<bool> Eliminar(int Id);
        Task<bool> Editar(UsuarioProveedorDTO parametro);
    }
    public interface IUsuarioGastosService
    {
        Task<List<UsuarioGastosDTO>> ObtenTodos();
        Task<UsuarioGastosDTO> ObtenXIdUsuario(int IdUsuario);

        Task<UsuarioGastosDTO> ObtenXId(int Id);
        Task<UsuarioGastosDTO> Crear(UsuarioGastosDTO parametro);
        Task<bool> Editar(UsuarioGastosDTO parametro);

    }
    public interface IUsuarioAutoFacService
    {
        Task<List<UsuarioAutoFacDTO>> ObtenTodos();
        Task<UsuarioAutoFacDTO> ObtenXIdUsuario(int IdUsuario);
        Task<UsuarioAutoFacDTO> ObtenXId(int Id);
        Task<UsuarioAutoFacDTO> Crear(UsuarioAutoFacDTO parametro);
        Task<bool> Editar(UsuarioAutoFacDTO parametro);

    }
    public interface IUsuarioProyectoService
    {
        Task<List<UsuarioProyectoDTO>> ObtenTodos();
        Task<List<UsuarioProyectoDTO>> ObtenXIdProyecto(int IdProyecto);
        Task<List<UsuarioProyectoDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioProyectoDTO>> ObtenXIdUsuarioXIdProyecto(int IdUsuario, int IdProyecto);
        Task<UsuarioProyectoDTO> Crear(UsuarioProyectoDTO parametro);
        Task<bool> Eliminar(int Id);
        Task<List<UsuarioProyectoDTO>> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa);
        Task<UsuarioProyectoDTO> CrearYObtener(UsuarioProyectoDTO registro);
        Task<bool> Editar(UsuarioProyectoDTO registro);
    }
    public interface IUsuarioClasificacionCfdiService
    {
        Task<List<UsuarioClasificacionCfdiDTO>> ObtenTodos();
        Task<List<UsuarioClasificacionCfdiDTO>> ObtenXIdClasificacionCfdi(int IdClasificacionCfdi);
        Task<List<UsuarioClasificacionCfdiDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioClasificacionCfdiDTO>> ObtenXIdUsuarioXIdClasificacionCfdi(int IdUsuario, int IdClasificacionCfdi);
        Task<UsuarioClasificacionCfdiDTO> Crear(UsuarioClasificacionCfdiDTO parametro);
        Task<bool> Eliminar(int Id);
    }
    public interface IUsuarioEmpresaPorDefectoService
    {
        Task<List<UsuarioEmpresaPorDefectoDTO>> ObtenTodos();
        Task<List<UsuarioEmpresaPorDefectoDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<UsuarioEmpresaPorDefectoDTO> ObtenXIdUsuario(int IdUsuario);
        Task<UsuarioEmpresaPorDefectoDTO> ObtenXIdUsuarioXIdEmpresa(int IdUsuario, int IdEmpresa);
        Task<UsuarioEmpresaPorDefectoDTO> CrearYObtener(UsuarioEmpresaPorDefectoDTO parametro);
        Task<bool> CambiaEmpresaPorDefecto(UsuarioEmpresaPorDefectoDTO parametro);
        Task<bool> Eliminar(int Id);
    }

    public interface IUsuarioPertenecienteAClienteService
    {
        Task<List<UsuarioPertenecienteAClienteDTO>> ObtenTodos();
        Task<List<UsuarioPertenecienteAClienteDTO>> ObtenXIdCliente(int IdCorporativo);
        Task<UsuarioPertenecienteAClienteDTO> ObtenXIdUsuario(int IdUsuario);
        Task<UsuarioPertenecienteAClienteDTO> ObtenXIdUsuarioXIdCliente(int IdUsuario, int IdCorporativo);
        Task<UsuarioPertenecienteAClienteDTO> CrearYObtener(UsuarioPertenecienteAClienteDTO parametro);
        Task<bool> Eliminar(int Id);
    }

    public interface IUsuariosUltimaSeccionService
    {
        Task<List<UsuariosUltimaSeccionDTO>> ObtenTodos();

        Task<UsuariosUltimaSeccionDTO> ObtenXIdUltimoS(int IdUsuarioUltimaS);

        Task<UsuariosUltimaSeccionDTO> CrearYObtener(UsuariosUltimaSeccionDTO parametro);
        
        Task<bool> Editar(UsuariosUltimaSeccionDTO modelo);


        Task<UsuariosUltimaSeccionDTO> Eliminar(int IdUsuarioUltimaS);

        Task<UsuariosUltimaSeccionDTO> ObtenerXIdUsuario(int IdTablaUsuario);

    }

    public interface IUsuarioProveedorFormasPagoXEmpresaService
    {
        Task<List<UsuarioFormasPagoXEmpresaDTO>> ObtenTodos();
        Task<UsuarioFormasPagoXEmpresaDTO> ObtenXId(int Id);
        Task<List<UsuarioFormasPagoXEmpresaDTO>> ObtenXIdUsuario(int IdUsuario);
        Task<List<UsuarioFormasPagoXEmpresaDTO>> ObtenXIdEmpresa(int IdEmpresa);
        Task<UsuarioFormasPagoXEmpresaDTO> ObtenXIdUsuarioEIdEmpresa(int IdUsuario, int IdEmpresa);
        Task<UsuarioFormasPagoXEmpresaDTO> CrearYObtener(UsuarioFormasPagoXEmpresaDTO parametro);
        Task<bool> Editar(UsuarioFormasPagoXEmpresaDTO modelo);
        Task<bool> Eliminar(int Id);
    }
}
