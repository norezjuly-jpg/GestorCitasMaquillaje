namespace GestorCitasMaquillaje.API.Models
{
    public class Maquillista
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public bool Disponible { get; set; } = true;
    }
}

