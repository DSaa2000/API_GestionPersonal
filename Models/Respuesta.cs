namespace API.Models
{
    public class Respuesta
    {
        public int Codigo { get; set; }
        public string Mensaje { get; set; }
        public bool Status { get; set; }
        public object Item { get; set; }
    }
}
