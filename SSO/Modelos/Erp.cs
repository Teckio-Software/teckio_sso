using SistemaERP.Model;
using System;
using System.Collections.Generic;

namespace SistemaERP.Models;

public partial class Erp
{
    public int Id { get; set; }

    public string NombreErp { get; set; } = null!;

    public virtual ICollection<Erpcorporativo> Erpcorporativos { get; set; } = new List<Erpcorporativo>();
}

