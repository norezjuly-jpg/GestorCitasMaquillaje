using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Dtos.Cita
{
    public class ActualizarCitaDto : CrearCitaDto
    {
        public int Id { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
