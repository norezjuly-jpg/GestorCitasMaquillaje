using GestorCitasMaquillaje.Domain.Entities;
using GestorCitasMaquillaje.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Infrastructure.Interfaces
{
    public interface IMaquillistaRepository : IBaseRepository<Maquillista>
    {
        Task<List<Maquillista>> GetDisponiblesAsync(DateTime fechaHora);
    }
}
