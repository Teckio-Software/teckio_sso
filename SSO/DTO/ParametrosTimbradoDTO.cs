using SSO.Modelos;

namespace SSO.DTO
{
    public class ParametrosTimbradoDTO : ParametrosTimbrado
    {
    }

    public class CertKeyDTO 
    {
        public IFormFile certificado { get; set; }
        public IFormFile key { get; set; }
        public string password { get; set; }    
    }
}
