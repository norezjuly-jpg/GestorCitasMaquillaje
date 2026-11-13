using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Infrastructure.Exceptions
{
    public class CitaException : Exception
    {
        public CitaException(string message) : base(message)
        {
        }
    }
}
