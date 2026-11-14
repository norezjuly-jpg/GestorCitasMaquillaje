using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Contract
{
    public interface IClienteService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(CrearClienteDto dto);
        Task<ServiceResult> UpdateAsync(ActualizarClienteDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
