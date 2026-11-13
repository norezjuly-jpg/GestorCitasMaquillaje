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
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        public CitaRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<Cita>> GetAllAsync()
        {
            return await _dbSet
                .Include(c => c.Cliente)
                .Include(c => c.Maquillista)
                .Include(c => c.Servicio)
                .Where(c => c.Activo)
                .ToListAsync();
        }

        public override async Task<Cita?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Cliente)
                .Include(c => c.Maquillista)
                .Include(c => c.Servicio)
                .FirstOrDefaultAsync(c => c.Id == id && c.Activo);
        }

        public async Task<List<Cita>> GetByClienteAsync(int clienteId)
        {
            return await _dbSet
                .Include(c => c.Servicio)
                .Include(c => c.Maquillista)
                .Where(c => c.ClienteId == clienteId && c.Activo)
                .ToListAsync();
        }

        public async Task<List<Cita>> GetByMaquillistaAsync(int maquillistaId, DateTime fecha)
        {
            var inicio = fecha.Date;
            var fin = inicio.AddDays(1);

            return await _dbSet
                .Include(c => c.Cliente)
                .Include(c => c.Servicio)
                .Where(c => c.MaquillistaId == maquillistaId
                            && c.FechaHora >= inicio
                            && c.FechaHora < fin
                            && c.Activo)
                .ToListAsync();
        }

        public async Task<bool> ExisteConflictoAsync(int maquillistaId, DateTime fechaHora)
        {
            return await _dbSet.AnyAsync(c => c.MaquillistaId == maquillistaId
                                              && c.FechaHora == fechaHora
                                              && c.Estado != "Cancelada"
                                              && c.Activo);
        }

        public async Task CancelarAsync(int citaId, string motivo)
        {
            var cita = await GetByIdAsync(citaId)
                       ?? throw new CitaException("La cita no existe.");

            if (cita.Estado == "Cancelada")
                throw new CitaException("La cita ya está cancelada.");

            cita.Estado = "Cancelada";
            cita.FechaCancelacion = DateTime.UtcNow;
            cita.MotivoCancelacion = motivo;

            await UpdateAsync(cita);
        }

        public override async Task<Cita> AddAsync(Cita entity)
        {
            if (await ExisteConflictoAsync(entity.MaquillistaId, entity.FechaHora))
                throw new CitaException("La maquillista ya tiene una cita en ese horario.");

            return await base.AddAsync(entity);
        }
    }
}
