namespace GestorCitasMaquillaje.API.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int MaquillistaId { get; set; }
        public int ServicioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = "Pendiente";
    }
}
