using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Cliente;
using GestorCitasMaquillaje.Domain.Entities;
using GestorCitasMaquillaje.Infrastructure.Exceptions;
using GestorCitasMaquillaje.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GestorCitasMaquillaje.Application.Service
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var clientes = await _repository.GetAllAsync();
            return ServiceResult.Ok(clientes.Select(ToDto).ToList());
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);

            if (cliente is null)
                return ServiceResult.Fail("El cliente no existe.");

            return ServiceResult.Ok(ToDto(cliente));
        }

        public async Task<ServiceResult> CreateAsync(CrearClienteDto dto)
        {
            var error = Validar(dto.Nombre, dto.Apellido, dto.Telefono, dto.Email);
            if (error is not null)
                return ServiceResult.Fail(error);

            try
            {
                var cliente = new Cliente
                {
                    Nombre = dto.Nombre.Trim(),
                    Apellido = dto.Apellido.Trim(),
                    Telefono = SoloDigitos(dto.Telefono),
                    Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim()
                };

                await _repository.AddAsync(cliente);
                return ServiceResult.Ok(ToDto(cliente), "Cliente creado correctamente.");
            }
            catch (ClienteException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(ActualizarClienteDto dto)
        {
            var error = Validar(dto.Nombre, dto.Apellido, dto.Telefono, dto.Email);
            if (error is not null)
                return ServiceResult.Fail(error);

            var cliente = await _repository.GetByIdAsync(dto.Id);
            if (cliente is null)
                return ServiceResult.Fail("El cliente no existe.");

            // aqui el telefono no puede coincidir con el de otro cliente
            var telefono = SoloDigitos(dto.Telefono);
            var todos = await _repository.GetAllAsync();
            if (todos.Any(c => c.Id != dto.Id && c.Telefono == telefono))
                return ServiceResult.Fail("Ya existe otro cliente con ese teléfono.");

            cliente.Nombre = dto.Nombre.Trim();
            cliente.Apellido = dto.Apellido.Trim();
            cliente.Telefono = telefono;
            cliente.Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();

            await _repository.UpdateAsync(cliente);
            return ServiceResult.Ok(ToDto(cliente), "Cliente actualizado correctamente.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente is null)
                return ServiceResult.Fail("El cliente no existe.");

            await _repository.RemoveAsync(cliente);
            return ServiceResult.Ok(null, "Cliente eliminado correctamente.");
        }

        private static string? Validar(string nombre, string apellido, string telefono, string? email)
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

        private static ClienteDto ToDto(Cliente c) => new()
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            Telefono = c.Telefono,
            Email = c.Email
        };
    }
}
