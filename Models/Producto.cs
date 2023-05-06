using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Producto
    {
        [Key]
        public Int64 ID_Producto { get; set; }
        public string Nombre { get; set; }
    }
}
