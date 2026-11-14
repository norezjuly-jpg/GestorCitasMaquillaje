using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Servicio;
using GestorCitasMaquillaje.Domain.Entities;
using GestorCitasMaquillaje.Infrastructure.Exceptions;
using GestorCitasMaquillaje.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Service
{
    public class ServicioService : IServicioService
    {
        private readonly IServicioRepository _repository;

        public ServicioService(IServicioRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var servicios = await _repository.GetAllAsync();
            return ServiceResult.Ok(servicios.Select(ToDto).ToList());
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            var servicio = await _repository.GetByIdAsync(id);

            if (servicio is null)
                return ServiceResult.Fail("El servicio no existe.");

            return ServiceResult.Ok(ToDto(servicio));
        }

        public async Task<ServiceResult> CreateAsync(CrearServicioDto dto)
        {
            var error = Validar(dto.Nombre, dto.Descripcion, dto.Precio, dto.DuracionMinutos);
            if (error is not null)
                return ServiceResult.Fail(error);

            try
            {
                var servicio = new Servicio
                {
                    Nombre = dto.Nombre.Trim(),
                    Descripcion = dto.Descripcion?.Trim(),
                    Precio = dto.Precio,
                    DuracionMinutos = dto.DuracionMinutos
                };

                await _repository.AddAsync(servicio);
                return ServiceResult.Ok(ToDto(servicio), "Servicio creado correctamente.");
            }
            catch (ServicioException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(ActualizarServicioDto dto)
        {
            var error = Validar(dto.Nombre, dto.Descripcion, dto.Precio, dto.DuracionMinutos);
            if (error is not null)
                return ServiceResult.Fail(error);

            var servicio = await _repository.GetByIdAsync(dto.Id);
            if (servicio is null)
                return ServiceResult.Fail("El servicio no existe.");

            var todos = await _repository.GetAllAsync();
            var nombre = dto.Nombre.Trim();
            if (todos.Any(s => s.Id != dto.Id &&
                               s.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
                return ServiceResult.Fail("Ya existe otro servicio con ese nombre.");

            servicio.Nombre = nombre;
            servicio.Descripcion = dto.Descripcion?.Trim();
            servicio.Precio = dto.Precio;
            servicio.DuracionMinutos = dto.DuracionMinutos;

            await _repository.UpdateAsync(servicio);
            return ServiceResult.Ok(ToDto(servicio), "Servicio actualizado correctamente.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var servicio = await _repository.GetByIdAsync(id);
            if (servicio is null)
                return ServiceResult.Fail("El servicio no existe.");

            await _repository.RemoveAsync(servicio);
            return ServiceResult.Ok(null, "Servicio eliminado correctamente.");
        }

        private static string? Validar(string nombre, string? descripcion, decimal precio, int duracion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "El nombre es obligatorio.";

            if (nombre.Trim().Length > 100)
                return "El nombre no puede tener más de 100 caracteres.";

            if (descripcion is not null && descripcion.Trim().Length > 250)
                return "La descripción no puede tener más de 250 caracteres.";

            if (precio <= 0)
                return "El precio debe ser mayor que cero.";

            if (duracion <= 0)
                return "La duración debe ser mayor que cero.";

            if (duracion > 480)
                return "La duración no puede superar los 480 minutos.";

            return null;
        }

        private static ServicioDto ToDto(Servicio s) => new()
        {
            Id = s.Id,
            Nombre = s.Nombre,
            Descripcion = s.Descripcion,
            Precio = s.Precio,
            DuracionMinutos = s.DuracionMinutos
        };
    }
}
