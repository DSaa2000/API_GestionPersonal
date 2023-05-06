using Microsoft.EntityFrameworkCore;
using API.Models;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using API.Models.Movimientos;
using API.Models.UtilidadGeneral;
using API.Models.Usuario;

namespace API.DataBase
{
    public class SitioDB : DbContext
    {
        public SitioDB(DbContextOptions<SitioDB> options) : base(options)
        {

        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<MesDB> Meses { get; set; }
        public DbSet<MovimientoDB> Movimientos { get; set; }
        public DbSet<FuenteDB> Fuentes { get; set; }
        public DbSet<TipoMovimientoDB> TiposMovimientos { get; set; }
        public DbSet<RolDB> RolesUsuarios { get; set; }
        public DbSet<UsuarioDB> Usuarios { get; set; }
        public DbSet<ItemMenuDB> ItemsMenu { get; set; }
        public DbSet<PermisoAccesoDB> PermisosAcceso { get; set; }
    }
}
