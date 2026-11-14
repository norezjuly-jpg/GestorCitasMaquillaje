using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Dtos.Cita
{
    public class CancelarCitaDto
    {
        public int Id { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}
