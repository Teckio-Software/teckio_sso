using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using SSO.DTO;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using SSO.Servicios;

namespace SSO.Procesos.Clases
{
    public class Certificado
    {
        private byte[] certificado;
        private X509Certificate2 x509Certificate;
        public RsaCertificateService _RsaCerService;

        public Certificado(byte[] certbytes) 
        {
            certificado = certbytes;
            x509Certificate = new X509Certificate2(certificado);
            _RsaCerService = new RsaCertificateService();
            ReadCer();
        }

        public bool IsCSD 
        {
            get 
            {
                List<X509KeyUsageExtension> extension = x509Certificate.Extensions.OfType<X509KeyUsageExtension>().ToList();
                return _RsaCerService.isCSD(extension);
            }
        }

        public bool IsFIEL
        {
            get
            {
                List<X509KeyUsageExtension> extension = x509Certificate.Extensions.OfType<X509KeyUsageExtension>().ToList();
                return _RsaCerService.isFIEL(extension);
            }
        }

        public RespuestaDTO IsValid
        {
            get
            {
                var vigente = FechaVigencia.Date >= DateTime.Today.Date;
                if (vigente) 
                {
                    if (IsFIEL)
                    {
                        return new RespuestaDTO { Estatus = false, Descripcion = "El archivo .cer corresponde a una FIEL, solo se admiten certificados de seguridad." };

                    }
                    else if (IsCSD)
                    {
                        return new RespuestaDTO { Estatus = true };

                    }
                    else
                    {
                        return new RespuestaDTO { Estatus = false, Descripcion = "No fue posible determinar el tipo de certificado, el certificado es inválido o está corrupto." };
                    }

                }
                else 
                {
                    return new RespuestaDTO { Estatus = false, Descripcion = $"El certificado de seguridad expiró el día {FechaVigencia.Date}" };
                }
                

            }
        }

        public string NoCertificado 
        {
            get 
            {
                return _RsaCerService.CertificateNumber(x509Certificate);
            }
        }

        public DateTime FechaExpedicion 
        {
            get;
            private set;
        }

        public DateTime FechaVigencia
        {
            get;
            private set;
        }

        public string Editor { get; private set; }

        public string Nombre { get; private set; }

        public void ReadCer() 
        {
            var cerinfo = _RsaCerService.ReadCertificate(certificado);
            Nombre = cerinfo.Nombre;
            Editor = cerinfo.Editor;
            FechaExpedicion = cerinfo.FechaExpedicion;
            FechaVigencia = cerinfo.FechaVigencia;
        }
    }
}
