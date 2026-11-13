using GestorCitasMaquillaje.Domain.Entities;
using GestorCitasMaquillaje.Infrastructure.Context;
using GestorCitasMaquillaje.Infrastructure.Core;
using GestorCitasMaquillaje.Infrastructure.Exceptions;
using GestorCitasMaquillaje.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Infrastructure.Repositories
{
    public class MaquillistaRepository : BaseRepository<Maquillista>, IMaquillistaRepository
    {
        public MaquillistaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Maquillista>> GetDisponiblesAsync(DateTime fechaHora)
        {
            var hora = fechaHora.TimeOfDay;

            return await _dbSet
                .Where(m => m.Activo
                            && m.HoraInicio <= hora
                            && m.HoraFin > hora
                            && !m.Citas.Any(c => c.FechaHora == fechaHora
                                                 && c.Estado != "Cancelada"
                                                 && c.Activo))
                .ToListAsync();
        }
    }
}

