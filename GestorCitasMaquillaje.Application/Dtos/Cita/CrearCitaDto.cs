using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Dtos.Cita
{
    public class CrearCitaDto
    {
        public DateTime FechaHora { get; set; }
        public string? Notas { get; set; }
        public decimal MontoPagado { get; set; }
        public string? MetodoPago { get; set; }

        public int ClienteId { get; set; }
        public int MaquillistaId { get; set; }
        public int ServicioId { get; set; }
    }
}
