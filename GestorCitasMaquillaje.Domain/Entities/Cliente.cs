using GestorCitasMaquillaje.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Domain.Entities
{
    public class Cliente : Person
    {
        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}
