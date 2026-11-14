using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Cita;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Contract
{
    public interface ICitaService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(int id);
        Task<ServiceResult> GetByClienteAsync(int clienteId);
        Task<ServiceResult> CreateAsync(CrearCitaDto dto);
        Task<ServiceResult> UpdateAsync(ActualizarCitaDto dto);
        Task<ServiceResult> CancelarAsync(CancelarCitaDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
