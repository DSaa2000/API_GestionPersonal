using System.ComponentModel.DataAnnotations;

namespace API.Models.Usuario
{
    public class UsuarioDB
    {
        [Key]
        public Int64 ID_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public long ID_Rol { get; set; }
    }
    public class Usuario
    {
        public long ID_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }
        public string Correo { get; set; }
        public long ID_Rol { get; set; }
        public RolDB Rol { get; set; }
    } 
    public class NuevoPerfil
    {
        public string User { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long ID_Rol { get; set; }
    } 

}
