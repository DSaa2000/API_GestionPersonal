using System.ComponentModel.DataAnnotations;

namespace API.Models.Movimientos
{
    public class FuenteDB
    {
        [Key]
        public long ID_Fuente { get; set; }
        public string Nombre { get; set; }
    }
}
