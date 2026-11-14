using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Maquillista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Contract
{
    public interface IMaquillistaService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(CrearMaquillistaDto dto);
        Task<ServiceResult> UpdateAsync(ActualizarMaquillistaDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
