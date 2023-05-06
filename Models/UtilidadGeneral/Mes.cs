using System.ComponentModel.DataAnnotations;

namespace API.Models.UtilidadGeneral
{
    public class MesDB
    {
        [Key]
        public long ID_Mes { get; set; }
        public string Nombre { get; set; }
    }
}
