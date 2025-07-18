using SistemaERP.Model;
using SistemaERP.Models;
using System;
using System.Collections.Generic;

namespace SistemaERP.Model;

public partial class Erpcorporativo
{
    public int Id { get; set; }

    public int IdCorporativo { get; set; }

    public int IdErp { get; set; }

    public virtual Corporativo IdCorporativoNavigation { get; set; } = null!;

    public virtual Erp IdErpNavigation { get; set; } = null!;
}

