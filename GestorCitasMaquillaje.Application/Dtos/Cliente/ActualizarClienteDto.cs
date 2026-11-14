using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Dtos.Cliente
{
    public class ActualizarClienteDto : CrearClienteDto
    {
        public int Id { get; set; }
    }
}
