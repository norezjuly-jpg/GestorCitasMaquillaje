using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Infrastructure.Exceptions
{
    public class ClienteException : Exception
    {
        public ClienteException(string message) : base(message)
        {
        }
    }
}