using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Contract
{
    public interface IServicioService
    {
        Task<ServiceResult> GetAllAsync();
        Task<ServiceResult> GetByIdAsync(int id);
        Task<ServiceResult> CreateAsync(CrearServicioDto dto);
        Task<ServiceResult> UpdateAsync(ActualizarServicioDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}
