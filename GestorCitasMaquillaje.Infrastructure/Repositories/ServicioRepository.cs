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
    public class ServicioRepository : BaseRepository<Servicio>, IServicioRepository
    {
        public ServicioRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            return await _dbSet.AnyAsync(s => s.Nombre == nombre && s.Activo);
        }

        public override async Task<Servicio> AddAsync(Servicio entity)
        {
            if (await ExisteNombreAsync(entity.Nombre))
                throw new ServicioException("Ya existe un servicio con ese nombre.");

            return await base.AddAsync(entity);
        }
    }
}
