using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Infrastructure.Exceptions
{
    public class MaquillistaException : Exception
    {
        public MaquillistaException(string message) : base(message)
        {
        }
    }
}
