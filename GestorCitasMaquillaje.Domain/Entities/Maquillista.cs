using GestorCitasMaquillaje.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Domain.Entities
{
    public class Maquillista : Person
    {
        public string? Especialidad { get; set; }
        public bool Disponible { get; set; } = true;
        public TimeSpan HoraInicio { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan HoraFin { get; set; } = new TimeSpan(18, 0, 0);
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }

}
