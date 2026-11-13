using GestorCitasMaquillaje.Domain.Entities;
using GestorCitasMaquillaje.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Infrastructure.Interfaces
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        Task<List<Cita>> GetByClienteAsync(int clienteId);
        Task<List<Cita>> GetByMaquillistaAsync(int maquillistaId, DateTime fecha);
        Task<bool> ExisteConflictoAsync(int maquillistaId, DateTime fechaHora);
        Task CancelarAsync(int citaId, string motivo);
    }
}
