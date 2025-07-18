using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using SistemaERP.BLL.Contrato;
using SistemaERP.BLL.ProcesoSSO;
using SistemaERP.BLL.Servicios.Contrato;
using SistemaERP.DTO;
using SistemaERP.DTO.Menu;
using SistemaERP.Model;
using SSO.DTO;
using SSO.Servicios;
using SSO.Servicios.Contratos;
using System.Data;
using System.Security.Claims;

namespace SistemaERP.API.Controllers.SSO
{
    [Route("api/UsuarioProyecto")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UsuarioProyectoController: ControllerBase
    {
        private readonly IUsuarioProyectoService _UsuarioProyectoService;
        private readonly IUsuarioService _UsuarioService;
        private readonly ProcesoRol _procesoRol;

        public UsuarioProyectoController(
            IUsuarioProyectoService usuarioProyectoService
            , IUsuarioService usuarioService
            , ProcesoRol procesoRol)
        {
            _UsuarioProyectoService = usuarioProyectoService;
            _UsuarioService = usuarioService;
            _procesoRol = procesoRol;
        }

        [HttpPost("obtenerProyectosUsuario")]
        public async Task<List<UsuarioProyectoDTO>> obtenerProyectosXIdUsuario(UsuarioProyectoParametrosBusquedaDTO parametros)
        {
            var usuario = await _UsuarioService.ObtenXIdUsuario(parametros.IdUsuario);
            var registros = await _UsuarioProyectoService.ObtenXIdUsuarioXIdEmpresa(parametros.IdUsuario, parametros.IdEmpresa);
            return registros;
        }

        [HttpPost("asignarProyectoUsuario")]
        public async Task<RespuestaDTO> asignacionProyectoAUsuario(UsuarioProyectoDTO registro)
        {
            var respuesta = new RespuestaDTO();
            if(registro.Id != 0)
            {
                var asignarProyectoAUsuario = await _UsuarioProyectoService.CrearYObtener(registro);
                if(asignarProyectoAUsuario.Id != 0)
                {
                    respuesta.Estatus = true;
                    if(registro.Estatus == true)
                    {
                        respuesta.Descripcion = "Usuario asignado al proyecto correctamente";
                    }
                    else
                    {
                        respuesta.Descripcion = "Usuario removido del proyecto correctamente";
                    }
                }
                else
                {
                    respuesta.Estatus = false;
                    if (registro.Estatus == true)
                    {
                        respuesta.Descripcion = "Usuario asignado al proyecto correctamente";
                    }
                    else
                    {
                        respuesta.Descripcion = "Usuario removido del proyecto correctamente";
                    }
                }
            }
            else
            {
                var editarRegistro = await _UsuarioProyectoService.Editar(registro);
                if(editarRegistro != false)
                {
                    respuesta.Estatus = true;
                    if (registro.Estatus == true)
                    {
                        respuesta.Descripcion = "Usuario asignado al proyecto correctamente";
                    }
                    else
                    {
                        respuesta.Descripcion = "Usuario removido del proyecto correctamente";
                    }
                }
                else
                {
                    respuesta.Estatus = false;
                    if (registro.Estatus == true)
                    {
                        respuesta.Descripcion = "Usuario asignado al proyecto correctamente";
                    }
                    else
                    {
                        respuesta.Descripcion = "Usuario removido del proyecto correctamente";
                    }
                }
            }
            return respuesta;
        }

        [HttpPost("asignarPermisosProyecto")]
        public async Task<ActionResult<RespuestaDTO>> asignarPermisosProyectoAnterior(UsuarioProyectoDTO parametros) {
            var respuesta = await _procesoRol.asignarPermisosProyectoAnterior(parametros);
            return respuesta;
        }

        [HttpPost("asignarRolDefault")]
        public async Task<ActionResult<RespuestaDTO>> asignarRolDefault(UsuarioProyectoDTO parametros)
        {
            var authen = HttpContext.User;
            var respuesta = await _procesoRol.asignarRolDefault(parametros, authen.Claims.ToList());
            return respuesta;
        }
    }
}
