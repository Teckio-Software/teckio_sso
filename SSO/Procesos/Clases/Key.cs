using SSO.DTO;
using SSO.Servicios;

namespace SSO.Procesos.Clases
{
    public class Key
    {
        private RsaCertificateService _Rsaservice;
        public byte[] Keybytes { get; private set; }
        public string Password { get; private set; }
        public Key(byte[] keybytes, string password)
        {

            Keybytes = keybytes;
            Password = password;
            _Rsaservice = new RsaCertificateService();
        }


        public RespuestaDTO ValidPassword 
        {
            get { return _Rsaservice.VerifyPassword(Password, Keybytes); }
        }

    }
}
