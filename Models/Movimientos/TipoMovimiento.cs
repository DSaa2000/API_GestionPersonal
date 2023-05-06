using System.ComponentModel.DataAnnotations;

namespace API.Models.Movimientos
{
    public class TipoMovimientoDB
    {
        [Key]
        public long ID_TipoMovimiento { get; set; }
        public string Nombre { get; set; }
    }
}
