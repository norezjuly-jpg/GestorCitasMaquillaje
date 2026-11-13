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
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Email == email && c.Activo);
        }

        public async Task<bool> ExisteTelefonoAsync(string telefono)
        {
            return await _dbSet.AnyAsync(c => c.Telefono == telefono && c.Activo);
        }

        public override async Task<Cliente> AddAsync(Cliente entity)
        {
            if (await ExisteTelefonoAsync(entity.Telefono))
                throw new ClienteException("Ya existe un cliente con ese teléfono.");

            return await base.AddAsync(entity);
        }
    }
}
