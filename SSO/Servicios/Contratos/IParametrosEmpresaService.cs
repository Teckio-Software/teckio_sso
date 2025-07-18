using SistemaERP.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.BLL.Contrato
{
    public interface IParametrosEmpresaService
    {
        Task<List<ParametrosEmpresaDTO>> Lista(int id);
        Task<ParametrosEmpresaDTO> Crear(ParametrosEmpresaDTO modelo);
        Task<bool> Editar(ParametrosEmpresaDTO modelo);
    }
}
