using AutoMapper;
using GuardarArchivos;
using GuardarArchivos.DTO;
using Microsoft.AspNetCore.Hosting;
using SistemaERP.BLL.Contrato;
using SistemaERP.Model;
using SSO.DTO;
using SSO.Modelos;
using SSO.Procesos.Clases;
using SSO.Servicios.Contratos;

namespace SSO.Procesos
{
    public class ProcesoArchivosTimbradoEmpresa
    {

        private readonly IMapper _Mapper;
        private readonly IArchivosEmpresaService _ArchivosEmpresaService;
        private readonly IEmpresaService _EmpresaService;
        private readonly IRsaCertificateService _RsaCertificateService;
        private readonly GuardarArchivosC _GuardarProcess;
        public ProcesoArchivosTimbradoEmpresa
        (
            IMapper mapper,
            IArchivosEmpresaService archivosEmpresaService,
            IEmpresaService empresaService,
            GuardarArchivosC guardarArchivosC,
            IWebHostEnvironment env,
            IRsaCertificateService rsaCertificateService
        )
        {
            _Mapper = mapper;
            _ArchivosEmpresaService = archivosEmpresaService;
            _EmpresaService = empresaService;
            _GuardarProcess = guardarArchivosC;
            _RsaCertificateService = rsaCertificateService;
        }


        public async Task<List<ParametrosTimbradoDTO>> ObtenerArchivosEmpresa(int idEmpresa)
        {
            if (idEmpresa > 0)
            {
                return await _ArchivosEmpresaService.ObtenenXEmpresa(idEmpresa);
            }
            else
            {
                return new List<ParametrosTimbradoDTO>();
            }
        }

        public async Task<ParametrosTimbradoDTO> ObtenerParametro(int id, bool isdefault = false) 
        {
            var archivo = await _ArchivosEmpresaService.Filtar(x=> 
                x.Id == id);

            if (isdefault) 
            {
                archivo = await _ArchivosEmpresaService.Filtar(x =>
                    x.IdEmpresa == id && x.Predeterminado == 1
                );
            }
            return archivo.First();
        
        }

        public async Task<RespuestaDTO> EliminarArchivoTimbrado(int id)
        {
            RespuestaDTO respuestaDTO = new RespuestaDTO();

            var encontrado = await _ArchivosEmpresaService.ObtenenXId(id);
            if (encontrado != null && encontrado.Id > 0)
            {
                encontrado.Estatus = 0;
                if (encontrado.Predeterminado == 1) 
                {
                    encontrado.Predeterminado = 0;
                    var archivosactivos = await _ArchivosEmpresaService.Filtar(x => x.IdEmpresa == encontrado.IdEmpresa && x.Estatus == 1 && x.Id != encontrado.Id);
                    if(archivosactivos != null) 
                    {
                        var nuevopredeterminado = archivosactivos.First();
                        nuevopredeterminado.Predeterminado = 1;
                        await _ArchivosEmpresaService.Editar(nuevopredeterminado);
                    }
                }

                var parametrotimbradoeliminar = await _ArchivosEmpresaService.Editar(encontrado);
                if (!parametrotimbradoeliminar.Estatus)
                {
                    return parametrotimbradoeliminar;
                }

                var archivocereliminar = await EliminarArchivo(encontrado.IdArchivoCer);
                if (!archivocereliminar.Estatus)
                {
                    return archivocereliminar;
                }
                var archivokeyeliminar = await EliminarArchivo(encontrado.IdArchivoKey);
                if (!archivokeyeliminar.Estatus)
                {
                    return archivokeyeliminar;
                }
                respuestaDTO.Estatus = true;
                respuestaDTO.Descripcion = "Archivo eliminado correctamente";
                return respuestaDTO;
            }
            else
            {
                respuestaDTO.Estatus = false;
                respuestaDTO.Descripcion = "No encontrado";
                return respuestaDTO;
            }
        }


        private async Task<RespuestaDTO> EliminarArchivo(int IdArchivo)
        {
            var fileresult = await _GuardarProcess.ObtenerArchivo(IdArchivo);
            var file = fileresult.Value;
            if (file != null && file.Id > 0)
            {
                file.Borrado = true;
                var eliminado = await _GuardarProcess.EditarArchivo(file);
                return eliminado.Value ?? new RespuestaDTO();
            }
            else
            {
                return new RespuestaDTO { Estatus = true }; //ya borrado;
            }

        }


        public async Task<RespuestaDTO> EditarArchivoTimbrado(ParametrosTimbradoDTO archivoEmpresaDTO)
        {
            var encontrado = await _ArchivosEmpresaService.ObtenenXId(archivoEmpresaDTO.Id);
            if (encontrado == null || encontrado.Id <= 0)
            {
                return new RespuestaDTO
                {
                    Estatus = false,
                    Descripcion = "No se encontró el archivo de timbrado especificado."
                };
            }

            // se quiere cambiar a default?
            var predeterminado = encontrado.Predeterminado == 0 && archivoEmpresaDTO.Predeterminado == 1;

            if (predeterminado)
            {
                var PredeterminadoAnterior = await _ArchivosEmpresaService.Filtar(x =>
                    x.IdEmpresa == archivoEmpresaDTO.IdEmpresa &&
                    x.Predeterminado == 1);

                var anteriorDefault = PredeterminadoAnterior.FirstOrDefault();
                if (anteriorDefault != null && anteriorDefault.Id > 0)
                {
                    anteriorDefault.Predeterminado = 0;
                    var quitarDefault = await _ArchivosEmpresaService.Editar(anteriorDefault);
                    if (!quitarDefault.Estatus) 
                    {
                        return quitarDefault;
                    }
                    
                }
            }
            var resultadoEdicion = await _ArchivosEmpresaService.Editar(archivoEmpresaDTO);
            if (!resultadoEdicion.Estatus)
            {
                return resultadoEdicion;
            }
            return resultadoEdicion;
        }

        public async Task<RespuestaDTO> AlmacenarCertificadoYKey(CertKeyDTO certKeyDTO, int idempresa)
        {
            var response = new RespuestaDTO { Estatus = false };
            var empresa = await _EmpresaService.ObtenXId(idempresa);

            var certificadofile = certKeyDTO.certificado;
            var keyfile = certKeyDTO.key;
            var password = certKeyDTO.password;

            if (certificadofile.Length > 0 && certificadofile.Length > 0)
            {
                if (empresa != null)
                {
                    var certbytes = await ConvertToBytesAsync(certificadofile);
                    var keybytes = await ConvertToBytesAsync(keyfile);

                    Certificado certificado = new Certificado(certbytes);
                    if (!certificado.IsValid.Estatus)
                    {
                        return certificado.IsValid;
                    }

                    Key key = new Key(keybytes, password);
                    if (!key.ValidPassword.Estatus)
                    {
                        return key.ValidPassword;
                    }

                    var procesovalidacion = await _RsaCertificateService.ValidarCertificadoKey(certbytes, keybytes, password);
                    if (procesovalidacion.Estatus)
                    {
                        var rutacompuesta = Path.Combine($"C:", "Timbrado", empresa.Rfc, certificado.NoCertificado);
                        var idcert = await _GuardarProcess.Post(certificadofile, rutacompuesta, empresa.Rfc, true);
                        var idkey = await _GuardarProcess.Post(keyfile, rutacompuesta, empresa.Rfc, true);

                        var existeparamdefault = await _ArchivosEmpresaService.Filtar(x=> x.IdEmpresa == empresa.Id && x.Predeterminado == 1);

                        var cerdefault = existeparamdefault.FirstOrDefault();

                        if (cerdefault != null && cerdefault.Id > 0)
                        {
                            cerdefault.Predeterminado = 0;
                            await _ArchivosEmpresaService.Editar(cerdefault);
                        }

                        if (idcert > 0 && idkey > 0)
                        {
                            var cerempresa = new ParametrosTimbradoDTO
                            {
                                IdEmpresa = empresa.Id,
                                IdArchivoCer = idcert,
                                IdArchivoKey = idkey,
                                KeyPassword = password,
                                FechaExpedicion = certificado.FechaExpedicion,
                                FechaVigencia = certificado.FechaVigencia,
                                Predeterminado = 1,
                                Estatus = 1
                            };

                            var almacenaparamempresa = await _ArchivosEmpresaService.Crear(cerempresa);
                            if (almacenaparamempresa.Estatus)
                            {
                                response.Descripcion = $"Certificado y key almacenados correctamente.";
                                response.Estatus = true;
                            }
                            else
                            {
                                response.Descripcion = $"Error al relacionar los archivos";
                                response.Estatus = false;
                            }
                        }
                        else
                        {
                            response.Descripcion = $"No se han podido almacenar los archivos.";
                            response.Estatus = false;
                        }
                    }
                    else
                    {
                        response = procesovalidacion;
                    }
                }
                else
                {
                    response.Descripcion = $"No se ha encontrada ninguna empresa con el identificador {idempresa}.";
                    response.Estatus = false;
                }
            }
            else
            {
                if (certificadofile.Length <= 0 && certificadofile.Length <= 0)
                {
                    response.Descripcion = "Certificado y Key inválidos";
                }
                else if (certificadofile.Length <= 0 && certificadofile.Length > 0)
                {
                    response.Descripcion = "Certificado inválido";
                }
                else if (certificadofile.Length <= 0 && certificadofile.Length > 0)
                {
                    response.Descripcion = "Key inválido";
                }
            }
            return response;
        }

        private async Task<byte[]> ConvertToBytesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
