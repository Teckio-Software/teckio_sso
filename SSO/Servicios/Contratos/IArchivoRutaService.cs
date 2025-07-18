using GuardarArchivos.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SistemaERP.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSO.DTO;

namespace SistemaERP.BLL.Contrato.Utilidades
{
    public interface IArchivoRutaService
    {
        Task<Archivo> obtenerRutaArchivo(int id);
        Task<RespuestaDTO> EditarRutaArchivo(Archivo archivo);
        Task<Archivo> GuardarArchivo(Archivo archivo);
        Task<Archivo> obtenerArchivoxUUID(string UUID);
       


    }
}
