using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaERP.Model
{
    public class ParametrosEmpresa
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public short? DiaMaximoCargaCfdi { get; set; }

        public short DiasFinMes { get; set; }

        /// <summary>
        /// Tolerancia establecida para que al procesar los CFDIS encuentre coincidencias con las entradas de material
        /// </summary>
        public decimal? ToleranciaCfdi { get; set; }

        public bool? RequiereNotificacion { get; set; }

        public bool RevalidaMesAnterior { get; set; }

        /// <summary>
        /// Valida el presupuesto con el Ws de SAP
        /// </summary>
        public bool ValidaPptoWs { get; set; }

        public int? DecimalesTolerancia { get; set; }

        public bool? ToleranciaUniversal { get; set; }

        public decimal? MontoToleranciaMxn { get; set; }

        public bool? FacturaDirecto { get; set; }

        public bool? AsignacionDirectaObligada { get; set; }
        public bool MultiplesValidaciones { get; set; }

        public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
    }
}
