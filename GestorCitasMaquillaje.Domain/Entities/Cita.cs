using GestorCitasMaquillaje.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Domain.Entities
{
    public class Cita : BaseEntity
    {
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string? Notas { get; set; }

        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public string? MetodoPago { get; set; }

        public DateTime? FechaCancelacion { get; set; }
        public string? MotivoCancelacion { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int MaquillistaId { get; set; }
        public Maquillista? Maquillista { get; set; }

        public int ServicioId { get; set; }
        public Servicio? Servicio { get; set; }
    }
}
