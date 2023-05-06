using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Movimientos
{
    public class MovimientoDB
    {
        [Key]
        public long ID_Movimiento { get; set; }
        public long ID_Fuente { get; set; }
        public long ID_TipoMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public double Monto { get; set; }
    }
    public class Movimiento
    {
        public MovimientoDB MovimientoDB { get; set; }
        public FuenteDB Fuente { get; set; }
        public TipoMovimientoDB TipoMovimiento { get; set; }
    }
}
