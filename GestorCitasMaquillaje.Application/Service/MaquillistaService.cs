using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Maquillista;
using GestorCitasMaquillaje.Domain.Entities;
using GestorCitasMaquillaje.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Service
{
    public class MaquillistaService : IMaquillistaService
    {
        private readonly IMaquillistaRepository _repository;

        public MaquillistaService(IMaquillistaRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var maquillistas = await _repository.GetAllAsync();
            return ServiceResult.Ok(maquillistas.Select(ToDto).ToList());
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            var maquillista = await _repository.GetByIdAsync(id);

            if (maquillista is null)
                return ServiceResult.Fail("La maquillista no existe.");

            return ServiceResult.Ok(ToDto(maquillista));
        }

        public async Task<ServiceResult> CreateAsync(CrearMaquillistaDto dto)
        {
            var error = Validar(dto.Nombre, dto.Apellido, dto.Telefono, dto.Email,
                                dto.Especialidad, dto.HoraInicio, dto.HoraFin);
            if (error is not null)
                return ServiceResult.Fail(error);

            // El telefono no puede estar repetido
            var telefono = SoloDigitos(dto.Telefono);
            var todas = await _repository.GetAllAsync();
            if (todas.Any(m => m.Telefono == telefono))
                return ServiceResult.Fail("Ya existe una maquillista con ese teléfono.");

            var maquillista = new Maquillista
            {
                Nombre = dto.Nombre.Trim(),
                Apellido = dto.Apellido.Trim(),
                Telefono = telefono,
                Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim(),
                Especialidad = string.IsNullOrWhiteSpace(dto.Especialidad) ? null : dto.Especialidad.Trim(),
                Disponible = dto.Disponible,
                HoraInicio = dto.HoraInicio,
                HoraFin = dto.HoraFin
            };

            await _repository.AddAsync(maquillista);
            return ServiceResult.Ok(ToDto(maquillista), "Maquillista creada correctamente.");
        }

        public async Task<ServiceResult> UpdateAsync(ActualizarMaquillistaDto dto)
        {
            var error = Validar(dto.Nombre, dto.Apellido, dto.Telefono, dto.Email,
                                dto.Especialidad, dto.HoraInicio, dto.HoraFin);
            if (error is not null)
                return ServiceResult.Fail(error);

            var maquillista = await _repository.GetByIdAsync(dto.Id);
            if (maquillista is null)
                return ServiceResult.Fail("La maquillista no existe.");

            var telefono = SoloDigitos(dto.Telefono);
            var todas = await _repository.GetAllAsync();
            if (todas.Any(m => m.Id != dto.Id && m.Telefono == telefono))
                return ServiceResult.Fail("Ya existe otra maquillista con ese teléfono.");

            maquillista.Nombre = dto.Nombre.Trim();
            maquillista.Apellido = dto.Apellido.Trim();
            maquillista.Telefono = telefono;
            maquillista.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
            maquillista.Especialidad = string.IsNullOrWhiteSpace(dto.Especialidad) ? null : dto.Especialidad.Trim();
            maquillista.Disponible = dto.Disponible;
            maquillista.HoraInicio = dto.HoraInicio;
            maquillista.HoraFin = dto.HoraFin;

            await _repository.UpdateAsync(maquillista);
            return ServiceResult.Ok(ToDto(maquillista), "Maquillista actualizada correctamente.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var maquillista = await _repository.GetByIdAsync(id);
            if (maquillista is null)
                return ServiceResult.Fail("La maquillista no existe.");

            await _repository.RemoveAsync(maquillista);
            return ServiceResult.Ok(null, "Maquillista eliminada correctamente.");
        }

        private static string? Validar(string nombre, string apellido, string telefono, string? email,
                                       string? especialidad, TimeSpan horaInicio, TimeSpan horaFin)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "El nombre es obligatorio.";

            if (nombre.Trim().Length > 50)
                return "El nombre no puede tener más de 50 caracteres.";

            if (string.IsNullOrWhiteSpace(apellido))
                return "El apellido es obligatorio.";

            if (apellido.Trim().Length > 50)
                return "El apellido no puede tener más de 50 caracteres.";

            if (string.IsNullOrWhiteSpace(telefono))
                return "El teléfono es obligatorio.";

            var digitos = SoloDigitos(telefono);
            var soloPermitidos = telefono.All(c => char.IsDigit(c) || c is ' ' or '-' or '(' or ')');

            if (!soloPermitidos || digitos.Length != 10)
                return "El teléfono debe tener exactamente 10 dígitos (ejemplo: 809-555-1234).";

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (email.Trim().Length > 100)
                    return "El email no puede tener más de 100 caracteres.";

                if (!EmailValido(email.Trim()))
                    return "El email no tiene un formato válido.";
            }

            if (especialidad is not null && especialidad.Trim().Length > 100)
                return "La especialidad no puede tener más de 100 caracteres.";

            if (horaInicio < TimeSpan.Zero || horaInicio >= TimeSpan.FromHours(24))
                return "La hora de inicio debe estar entre 00:00 y 23:59.";

            if (horaFin <= TimeSpan.Zero || horaFin >= TimeSpan.FromHours(24))
                return "La hora de fin debe estar entre 00:01 y 23:59.";

            if (horaFin <= horaInicio)
                return "La hora de fin debe ser posterior a la hora de inicio.";

            return null;
        }

        private static string SoloDigitos(string texto) =>
            new string(texto.Where(char.IsDigit).ToArray());

        private static bool EmailValido(string email)
        {
            try
            {
                var direccion = new MailAddress(email);
                return direccion.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private static MaquillistaDto ToDto(Maquillista m) => new()
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Apellido = m.Apellido,
            Telefono = m.Telefono,
            Email = m.Email,
            Especialidad = m.Especialidad,
            Disponible = m.Disponible,
            HoraInicio = m.HoraInicio,
            HoraFin = m.HoraFin
        };
    }
}
