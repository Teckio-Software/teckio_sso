using SSO.DTO;
using SSO.Servicios;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public interface IRsaCertificateService
{
    string CertificateNumber(X509Certificate2 cert);
    bool isFIEL(List<X509KeyUsageExtension> extension);
    bool isCSD(List<X509KeyUsageExtension> extension);
    CerProps ReadCertificate(byte[] certificado);
    string DetectEncryptionAlgorithm(byte[] keyBytes);
    string GetAlgorithmNameFromOid(string oid);
    Task<RespuestaDTO> ValidarCertificadoKey(byte[] bytesCER, byte[] bytesKEY, string password);
    byte[] GetPFX();

}
