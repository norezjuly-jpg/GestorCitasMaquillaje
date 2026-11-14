using GestorCitasMaquillaje.Application.Contract;
using GestorCitasMaquillaje.Application.Core;
using GestorCitasMaquillaje.Application.Dtos.Cita;
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
    public class CitaService : ICitaService
    {
        private static readonly string[] MetodosPago = { "Efectivo", "Tarjeta", "Transferencia" };
        private static readonly string[] EstadosEditables = { "Pendiente", "Confirmada", "Completada" };

        private readonly ICitaRepository _citaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMaquillistaRepository _maquillistaRepository;
        private readonly IServicioRepository _servicioRepository;

        public CitaService(
            ICitaRepository citaRepository,
            IClienteRepository clienteRepository,
            IMaquillistaRepository maquillistaRepository,
            IServicioRepository servicioRepository)
        {
            _citaRepository = citaRepository;
            _clienteRepository = clienteRepository;
            _maquillistaRepository = maquillistaRepository;
            _servicioRepository = servicioRepository;
        }

        public async Task<ServiceResult> GetAllAsync()
        {
            var citas = await _citaRepository.GetAllAsync();
            return ServiceResult.Ok(citas.Select(ToDto).ToList());
        }

        public async Task<ServiceResult> GetByIdAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);

            if (cita is null)
                return ServiceResult.Fail("La cita no existe.");

            return ServiceResult.Ok(ToDto(cita));
        }

        public async Task<ServiceResult> GetByClienteAsync(int clienteId)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente is null)
                return ServiceResult.Fail("El cliente no existe.");

            var citas = await _citaRepository.GetByClienteAsync(clienteId);
            var dtos = citas.Select(ToDto).ToList();

            foreach (var dto in dtos)
                dto.NombreCliente = $"{cliente.Nombre} {cliente.Apellido}";

            return ServiceResult.Ok(dtos);
        }

        public async Task<ServiceResult> CreateAsync(CrearCitaDto dto)
        {
            var error = Validar(dto.ClienteId, dto.MaquillistaId, dto.ServicioId,
                                dto.FechaHora, dto.Notas, dto.MontoPagado, dto.MetodoPago);
            if (error is not null)
                return ServiceResult.Fail(error);

            if (dto.FechaHora <= DateTime.Now)
                return ServiceResult.Fail("La fecha y hora de la cita debe ser futura.");

            var rel = await ObtenerRelacionesAsync(dto.ClienteId, dto.MaquillistaId,
                                                   dto.ServicioId, dto.FechaHora, null);
            if (rel.Error is not null)
                return ServiceResult.Fail(rel.Error);

            var servicio = rel.Servicio!;
            if (dto.MontoPagado > servicio.Precio)
                return ServiceResult.Fail("El monto pagado no puede ser mayor que el precio del servicio.");

            try
            {
                var cita = new Cita
                {
                    FechaHora = dto.FechaHora,
                    Estado = "Pendiente",
                    Notas = string.IsNullOrWhiteSpace(dto.Notas) ? null : dto.Notas.Trim(),
                    MontoTotal = servicio.Precio,
                    MontoPagado = dto.MontoPagado,
                    MetodoPago = NormalizarMetodoPago(dto.MetodoPago),
                    ClienteId = dto.ClienteId,
                    Cliente = rel.Cliente,
                    MaquillistaId = dto.MaquillistaId,
                    Maquillista = rel.Maquillista,
                    ServicioId = dto.ServicioId,
                    Servicio = servicio
                };

                await _citaRepository.AddAsync(cita);
                return ServiceResult.Ok(ToDto(cita), "Cita creada correctamente.");
            }
            catch (CitaException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(ActualizarCitaDto dto)
        {
            var error = Validar(dto.ClienteId, dto.MaquillistaId, dto.ServicioId,
                                dto.FechaHora, dto.Notas, dto.MontoPagado, dto.MetodoPago);
            if (error is not null)
                return ServiceResult.Fail(error);

            if (!EstadosEditables.Contains(dto.Estado))
                return ServiceResult.Fail("El estado debe ser Pendiente, Confirmada o Completada. Para cancelar use la opción de cancelar.");

            var cita = await _citaRepository.GetByIdAsync(dto.Id);
            if (cita is null)
                return ServiceResult.Fail("La cita no existe.");

            if (cita.Estado == "Cancelada")
                return ServiceResult.Fail("No se puede modificar una cita cancelada.");

            if (dto.FechaHora != cita.FechaHora && dto.FechaHora <= DateTime.Now)
                return ServiceResult.Fail("La nueva fecha y hora de la cita debe ser futura.");

            var rel = await ObtenerRelacionesAsync(dto.ClienteId, dto.MaquillistaId,
                                                   dto.ServicioId, dto.FechaHora, dto.Id);
            if (rel.Error is not null)
                return ServiceResult.Fail(rel.Error);

            var servicio = rel.Servicio!;
            if (dto.MontoPagado > servicio.Precio)
                return ServiceResult.Fail("El monto pagado no puede ser mayor que el precio del servicio.");

            cita.FechaHora = dto.FechaHora;
            cita.Estado = dto.Estado;
            cita.Notas = string.IsNullOrWhiteSpace(dto.Notas) ? null : dto.Notas.Trim();
            cita.MontoTotal = servicio.Precio;
            cita.MontoPagado = dto.MontoPagado;
            cita.MetodoPago = NormalizarMetodoPago(dto.MetodoPago);
            cita.ClienteId = dto.ClienteId;
            cita.Cliente = rel.Cliente;
            cita.MaquillistaId = dto.MaquillistaId;
            cita.Maquillista = rel.Maquillista;
            cita.ServicioId = dto.ServicioId;
            cita.Servicio = servicio;

            await _citaRepository.UpdateAsync(cita);
            return ServiceResult.Ok(ToDto(cita), "Cita actualizada correctamente.");
        }

        public async Task<ServiceResult> CancelarAsync(CancelarCitaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Motivo))
                return ServiceResult.Fail("El motivo de cancelación es obligatorio.");

            if (dto.Motivo.Trim().Length > 250)
                return ServiceResult.Fail("El motivo no puede tener más de 250 caracteres.");

            var cita = await _citaRepository.GetByIdAsync(dto.Id);
            if (cita is null)
                return ServiceResult.Fail("La cita no existe.");

            if (cita.Estado == "Cancelada")
                return ServiceResult.Fail("La cita ya está cancelada.");

            if (cita.Estado == "Completada")
                return ServiceResult.Fail("No se puede cancelar una cita completada.");

            try
            {
                await _citaRepository.CancelarAsync(dto.Id, dto.Motivo.Trim());
            }
            catch (CitaException ex)
            {
                return ServiceResult.Fail(ex.Message);
            }

            var actualizada = await _citaRepository.GetByIdAsync(dto.Id);
            return ServiceResult.Ok(actualizada is null ? null : ToDto(actualizada),
                                    "Cita cancelada correctamente.");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita is null)
                return ServiceResult.Fail("La cita no existe.");

            await _citaRepository.RemoveAsync(cita);
            return ServiceResult.Ok(null, "Cita eliminada correctamente.");
        }

        private static string? Validar(int clienteId, int maquillistaId, int servicioId,
                                       DateTime fechaHora, string? notas,
                                       decimal montoPagado, string? metodoPago)
        {
            if (clienteId <= 0)
                return "Debe seleccionar un cliente.";

            if (maquillistaId <= 0)
                return "Debe seleccionar una maquillista.";

            if (servicioId <= 0)
                return "Debe seleccionar un servicio.";

            if (fechaHora == default)
                return "La fecha y hora de la cita son obligatorias.";

            if (notas is not null && notas.Trim().Length > 250)
                return "Las notas no pueden tener más de 250 caracteres.";

            if (montoPagado < 0)
                return "El monto pagado no puede ser negativo.";

            if (!string.IsNullOrWhiteSpace(metodoPago) && NormalizarMetodoPago(metodoPago) is null)
                return "El metodo de pago debe ser Efectivo, Tarjeta o Transferencia.";

            if (montoPagado > 0 && string.IsNullOrWhiteSpace(metodoPago))
                return "Debe indicar el metodo de pago cuando hay un monto pagado.";

            return null;
        }

        private async Task<(string? Error, Cliente? Cliente, Maquillista? Maquillista, Servicio? Servicio)>
            ObtenerRelacionesAsync(int clienteId, int maquillistaId, int servicioId,
                                   DateTime fechaHora, int? excluirCitaId)
        {
            var cliente = await _clienteRepository.GetByIdAsync(clienteId);
            if (cliente is null)
                return ("El cliente no existe.", null, null, null);

            var maquillista = await _maquillistaRepository.GetByIdAsync(maquillistaId);
            if (maquillista is null)
                return ("La maquillista no existe.", null, null, null);

            if (!maquillista.Disponible)
                return ("La maquillista no está disponible.", null, null, null);

            var servicio = await _servicioRepository.GetByIdAsync(servicioId);
            if (servicio is null)
                return ("El servicio no existe.", null, null, null);

            // Horario de trabajo de la maquillista
            var inicio = fechaHora.TimeOfDay;
            var fin = inicio + TimeSpan.FromMinutes(servicio.DuracionMinutos);

            if (inicio < maquillista.HoraInicio || fin > maquillista.HoraFin)
                return ($"La cita debe estar dentro del horario de la maquillista ({maquillista.HoraInicio:hh\\:mm} a {maquillista.HoraFin:hh\\:mm}).",
                        null, null, null);

            // Choque con otras citas de la misma maquillista ese día
            var citasDelDia = await _citaRepository.GetByMaquillistaAsync(maquillistaId, fechaHora);
            var finCita = fechaHora.AddMinutes(servicio.DuracionMinutos);

            var hayChoque = citasDelDia.Any(c =>
                c.Id != excluirCitaId &&
                c.Estado != "Cancelada" &&
                fechaHora < c.FechaHora.AddMinutes(c.Servicio?.DuracionMinutos ?? 0) &&
                c.FechaHora < finCita);

            if (hayChoque)
                return ("La maquillista ya tiene una cita en ese horario.", null, null, null);

            return (null, cliente, maquillista, servicio);
        }

        private static string? NormalizarMetodoPago(string? metodo)
        {
            if (string.IsNullOrWhiteSpace(metodo))
                return null;

            return MetodosPago.FirstOrDefault(m =>
                m.Equals(metodo.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private static CitaDto ToDto(Cita c) => new()
        {
            Id = c.Id,
            FechaHora = c.FechaHora,
            Estado = c.Estado,
            Notas = c.Notas,
            MontoTotal = c.MontoTotal,
            MontoPagado = c.MontoPagado,
            MetodoPago = c.MetodoPago,
            FechaCancelacion = c.FechaCancelacion,
            MotivoCancelacion = c.MotivoCancelacion,
            ClienteId = c.ClienteId,
            MaquillistaId = c.MaquillistaId,
            ServicioId = c.ServicioId,
            NombreCliente = c.Cliente is null ? string.Empty : $"{c.Cliente.Nombre} {c.Cliente.Apellido}",
            NombreMaquillista = c.Maquillista is null ? string.Empty : $"{c.Maquillista.Nombre} {c.Maquillista.Apellido}",
            NombreServicio = c.Servicio?.Nombre ?? string.Empty
        };
    }
}
