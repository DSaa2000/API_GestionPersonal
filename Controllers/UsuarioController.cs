using API.DataBase;
using API.Models.Movimientos;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models.Usuario;
using Microsoft.EntityFrameworkCore;
using API.BLL;
using API.Models.UtilidadGeneral;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private SitioDB db;
        public UsuarioController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(UsuarioDB u)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuarios.Add(u);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Usuario creado exitosamente";
                    respuesta.Status = true;
                    respuesta.Item = u.ID_Usuario;
                }
                else
                {
                    respuesta.Codigo = 400;
                    respuesta.Mensaje = "Modelo Inválido";
                    respuesta.Status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 500;
                respuesta.Mensaje = "Error: " + ex.Message;
                respuesta.Status = false;
            }
            return respuesta;
        }
        [HttpPost]
        [Route("Create_Perfil")]
        public async Task<Respuesta> Crear_Perfil(NuevoPerfil p)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioDB nu = new UsuarioDB();
                    nu.Nombre = p.User;
                    nu.Correo = p.Email;
                    nu.Password = p.Password;
                    nu.ID_Rol = p.ID_Rol;
                    db.Usuarios.Add(nu);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Usuario creado exitosamente";
                    respuesta.Status = true;
                    respuesta.Item = nu.ID_Usuario;
                }
                else
                {
                    respuesta.Codigo = 400;
                    respuesta.Mensaje = "Modelo Inválido";
                    respuesta.Status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 500;
                respuesta.Mensaje = "Error: " + ex.Message;
                respuesta.Status = false;
            }
            return respuesta;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<UsuarioDB> Get(Int64 ID_Usuario)
        {
            UsuarioDB u = await db.Usuarios.FindAsync(ID_Usuario);
            if (u == null)
            {
                u = new UsuarioDB();
                u.ID_Usuario = 0;
                u.Nombre = "";
                u.Correo = "";
                u.Password = "";
                u.ID_Rol = 0;
            }
            return u;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<UsuarioDB>> Listar()
        {
            List<UsuarioDB> list = await db.Usuarios.ToListAsync();
            return list;
        }
        [HttpGet]
        [Route("ListSP")]
        public async Task<List<Usuario>> ListarSP()
        {
            usuarioBLL BLL = new usuarioBLL(db);
            List<Usuario> List = new List<Usuario>();
            try
            {
                Respuesta respuesta = new Respuesta();
                respuesta = await BLL.Listar_Usuarios();
                if (respuesta.Status)
                {
                    List = (List<Usuario>)respuesta.Item;
                }
            }
            catch (Exception ex)
            {

            }
            return List;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(Usuario u)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                UsuarioDB Original = db.Usuarios.Find(u.ID_Usuario);
                if (Original == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Usuario no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    Original.Nombre = u.Nombre;
                    Original.Correo = u.Correo;
                    Original.Password = u.Password;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Usuario editado exitosamente";
                        respuesta.Status = true;
                    }
                    else
                    {
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Error al editar el usuario";
                        respuesta.Status = false;
                        return respuesta;
                    }

                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 500;
                respuesta.Mensaje = "Error: " + ex.Message;
                respuesta.Status = false;
            }
            return respuesta;

        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<Respuesta> Eliminar(Int64 ID_Usuario)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                UsuarioDB Eliminado = db.Usuarios.Find(ID_Usuario);
                if (Eliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Usuario no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Usuario eliminado exitosamente";
                    respuesta.Status = false;
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 500;
                respuesta.Mensaje = "Error: " + ex.Message;
                respuesta.Status = false;
            }
            return respuesta;
        }
        [HttpPost]
        [Route("Login")]
        public async Task<Respuesta> Login(UsuarioDB user)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                List<UsuarioDB> lista = db.Usuarios.Where(u => (u.Nombre == user.Nombre || u.Correo == user.Nombre) && u.Password == user.Password).ToList();
                if (lista.Count == 0)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Acceso inválido. Por favor, inténtelo otra vez.";
                    respuesta.Status = false;
                }
                else
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Iniciando Sesión";
                    respuesta.Status = true;
                    Perfil perfil = new Perfil();
                    UsuarioDB userDB = lista.First();
                    perfil.Usuario = userDB;
                    Int64 ID_Rol = userDB.ID_Rol;
                    
                    RolDB rol = db.RolesUsuarios.Find(ID_Rol);
                    List<PermisoAcceso> permisos = new List<PermisoAcceso>();
                    List<PermisoAccesoDB> list = db.PermisosAcceso.ToList();
                    List<ItemMenuDB> items = db.ItemsMenu.ToList();
                    Rol r = new Rol();
                    foreach (ItemMenuDB item in items)
                    {
                        PermisoAcceso pa = new PermisoAcceso();
                        pa.ID_ItemPadre = item.ID_ItemPadre;
                        pa.ID_ItemMenu = item.ID_ItemMenu;
                        pa.Nombre = item.Titulo;
                        pa.Permiso = list.Where(i => i.ID_ItemMenu == item.ID_ItemMenu && i.ID_Rol == ID_Rol).ToList().Count == 1;
                        pa.Path = item.Url;
                        permisos.Add(pa);
                    }
                    r.RolUsuario = rol;
                    r.Permisos = permisos;
                    perfil.Rol = r;
                    respuesta.Item = perfil;
                }
            }
            catch (Exception ex) {
                respuesta.Codigo = 500;
                respuesta.Mensaje = "Error: " + ex.Message;
                respuesta.Status = false;
            }
            
            return respuesta;

        }

    }
}