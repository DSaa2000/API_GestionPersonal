using API.Models.UtilidadGeneral;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Models.Usuario
{
    public class RolDB
    {
        [Key]
        public Int64 ID_Rol { get; set; }
        public string Nombre { get; set; }
    }
    public class Rol
    {
        public RolDB RolUsuario { get; set; }
        public List<PermisoAcceso> Permisos { get; set; }
    }
    [Keyless]
    public class PermisoAccesoDB
    {
        public Int64 ID_ItemMenu { get; set; }
        public Int64 ID_Rol { get; set; }
    }
    public class PermisoAcceso
    {
        public Int64 ID_ItemMenu { get; set; }
        public Int64 ID_ItemPadre { get; set; }
        public string Nombre { get; set; }
        public string Path { get; set; }
        public bool Permiso { get; set; }
    }
    
}
