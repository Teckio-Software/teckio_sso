using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SSO.Utilidades;
using SSO.DTO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1;

namespace SSO.Servicios
{
    public class RsaCertificateService : IRsaCertificateService
    {
        private byte[] pfx = null;

        public string CertificateNumber(X509Certificate2 cert)
        {
            string hexadecimalString = cert.SerialNumber;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= hexadecimalString.Length - 2; i += 2)
            {
                sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hexadecimalString.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
            }
            return sb.ToString();
        }

        public bool isFIEL(List<X509KeyUsageExtension> extension)
        {
            X509KeyUsageFlags flag = X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation | X509KeyUsageFlags.KeyAgreement | X509KeyUsageFlags.DataEncipherment;
            return extension[0].KeyUsages.HasFlag(flag);
        }

        public bool isCSD(List<X509KeyUsageExtension> extension)
        {
            X509KeyUsageFlags flag = X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation;
            return extension[0].KeyUsages.HasFlag(flag);
        }

        public CerProps ReadCertificate(byte[] certificado)
        {
            X509Certificate2 x509Certificate = new X509Certificate2(certificado);
            try
            {
                return new CerProps 
                {
                    Nombre = x509Certificate.GetNameInfo(X509NameType.SimpleName, false),
                    FechaExpedicion = x509Certificate.NotBefore,
                    FechaVigencia = x509Certificate.NotAfter,
                    Editor = x509Certificate.GetNameInfo(X509NameType.SimpleName, true)
                };
                
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public string DetectEncryptionAlgorithm(byte[] keyBytes)
        {
            try
            {
                using (var stream = new MemoryStream(keyBytes))
                using (var asn1Stream = new Asn1InputStream(stream))
                {
                    var sequence = (Asn1Sequence)asn1Stream.ReadObject();
                    var encryptionAlgorithm = (Asn1Sequence)sequence[0];
                    var oid = (DerObjectIdentifier)encryptionAlgorithm[0];

                    return $"Algoritmo de cifrado OID: {oid.Id}\nNombre común: {GetAlgorithmNameFromOid(oid.Id)}";
                }
            }
            catch (Exception ex)
            {
                return $"Error al analizar la clave: {ex.Message}";
            }
        }
        public string GetAlgorithmNameFromOid(string oid)
        {
            try
            {
                var oidObj = new System.Security.Cryptography.Oid(oid);
                return !string.IsNullOrEmpty(oidObj.FriendlyName) ? oidObj.FriendlyName : $"OID desconocido: {oid}";
            }
            catch
            {
                return $"OID inválido o no reconocido: {oid}";
            }
        }

        public async Task<RespuestaDTO> ValidarCertificadoKey(byte[] bytesCER, byte[] bytesKEY, string password)
        {
            var respuestapfx = new RespuestaDTO();

            try
            {
                X509Certificate2 certificate;
                try
                {
                    certificate = new X509Certificate2(bytesCER);
                }
                catch
                {
                    respuestapfx.Estatus = false;
                    respuestapfx.Descripcion = "El archivo CER no es válido o está dañado.";
                    return respuestapfx;
                }

                AsymmetricKeyParameter privateKey;
                try
                {
                    char[] arrayOfChars = password.ToCharArray();
                    privateKey = PrivateKeyFactory.DecryptKey(arrayOfChars, bytesKEY);
                }
                catch
                {
                    respuestapfx.Estatus = false;
                    respuestapfx.Descripcion = "La contraseña es incorrecta o el archivo KEY no es válido.";
                    return respuestapfx;
                }

                RSA rsa;
                try
                {
                    RsaUtils rsau = new RsaUtils();
                    rsa = rsau.ToRSA((RsaPrivateCrtKeyParameters)privateKey);
                }
                catch
                {
                    respuestapfx.Estatus = false;
                    respuestapfx.Descripcion = "Error al convertir la clave privada a formato RSA.";
                    return respuestapfx;
                }

                X509Certificate2 certWithKey;
                try
                {
                    certWithKey = certificate.CopyWithPrivateKey(rsa);
                }
                catch
                {
                    respuestapfx.Estatus = false;
                    respuestapfx.Descripcion = "La clave privada no coincide con el certificado público.";
                    return respuestapfx;
                }

                try
                {
                    pfx = certWithKey.Export(X509ContentType.Pfx, password);
                    respuestapfx.Estatus = true;
                    respuestapfx.Descripcion = "PFX creado exitosamente.";
                }
                catch
                {
                    respuestapfx.Estatus = false;
                    respuestapfx.Descripcion = "Error al exportar el archivo PFX.";
                }
            }
            catch (Exception e)
            {
                respuestapfx.Estatus = false;
                respuestapfx.Descripcion = $"Error inesperado: {e.Message}";
            }

            return respuestapfx;
        }


        public byte[] GetPFX() 
        {
            return pfx;
        }

        public RespuestaDTO VerifyPassword(string password, byte[] keybytes)
        {
            RespuestaDTO response = new RespuestaDTO { Estatus = true, Descripcion = "Contraseña correcta." };
            AsymmetricKeyParameter privateKey;
            try
            {
                char[] arrayOfChars = password.ToCharArray();
                privateKey = PrivateKeyFactory.DecryptKey(arrayOfChars, keybytes);
                return response;
            }
            catch
            {
                response.Estatus = false;
                response.Descripcion = "La contraseña propocionada es incorrecta o el archivo KEY no es válido.";
                return response;
            }

        }

    }

    public class CerProps
    {
        public string Nombre { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public DateTime FechaVigencia { get; set; }
        public string Editor { get; set; }
    }

}
