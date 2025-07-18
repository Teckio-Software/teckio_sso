using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.DTO
{
    public class ParametrosEmpresaDTO
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int? DiaMaximoCargaCfdi { get; set; }

        public int DiasFinMes { get; set; }

        public decimal? ToleranciaCfdi { get; set; }

        public bool? RequiereNotificacion { get; set; }

        public bool RevalidaMesAnterior { get; set; }

        public bool ValidaPptoWs { get; set; }

        public int? DecimalesTolerancia { get; set; }

        public bool? ToleranciaUniversal { get; set; }

        public decimal? MontoToleranciaMxn { get; set; }

        public bool? FacturaDirecto { get; set; }

        public bool? AsignacionDirectaObligada { get; set; }

    }
}
