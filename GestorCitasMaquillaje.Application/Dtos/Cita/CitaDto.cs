using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Dtos.Cita
{
    public class CitaDto : DtoBase
    {
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Notas { get; set; }

        public decimal MontoTotal { get; set; }
        public decimal MontoPagado { get; set; }
        public string? MetodoPago { get; set; }

        public DateTime? FechaCancelacion { get; set; }
        public string? MotivoCancelacion { get; set; }

        public int ClienteId { get; set; }
        public int MaquillistaId { get; set; }
        public int ServicioId { get; set; }

        public string NombreCliente { get; set; } = string.Empty;
        public string NombreMaquillista { get; set; } = string.Empty;
        public string NombreServicio { get; set; } = string.Empty;
    }
}
