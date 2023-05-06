using API.DataBase;
using API.Models.Usuario;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Models.UtilidadGeneral;
using API.BLL;
using API.DAL;
using Microsoft.Extensions.WebEncoders.Testing;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolUsuarioController : ControllerBase
    {
        private SitioDB db;
        public RolUsuarioController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(Rol r)
        {
            Respuesta respuesta = new Respuesta();
            usuarioDAL DAL = new usuarioDAL(db);
            try
            {
                if (ModelState.IsValid)
                {
                    List<PermisoAccesoDB> permisos = new List<PermisoAccesoDB>();
                                       
                    db.RolesUsuarios.Add(r.RolUsuario);
                    db.SaveChanges();
                    foreach (PermisoAcceso permiso in r.Permisos)
                    {
                        if (permiso.Permiso)
                        {
                            PermisoAccesoDB p = new PermisoAccesoDB();
                            p.ID_ItemMenu = permiso.ID_ItemMenu;
                            p.ID_Rol = r.RolUsuario.ID_Rol;
                            await DAL.Insertar_PermisosAcceso(p);
                        }
                    }
                    db.PermisosAcceso.AddRange(permisos);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Rol creado exitosamente";
                    respuesta.Status = true;
                    respuesta.Item = r.RolUsuario.ID_Rol;
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
        public async Task<RolDB> Get(Int64 ID_Rol)
        {
            RolDB r = await db.RolesUsuarios.FindAsync(ID_Rol);
            if (r == null)
            {
                r = new RolDB();
                r.ID_Rol = 0;
                r.Nombre = "";
            }
            return r;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<RolDB>> Listar()
        {
            List<RolDB> list = await db.RolesUsuarios.ToListAsync();
            return list;
        }
        [HttpGet]
        [Route("Listar_PermisosByRol")]
        public async Task<Rol> Listar_PermisosByRol(Int64 ID_Rol)
        {
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
                permisos.Add(pa);
            }
            r.RolUsuario = rol;
            r.Permisos = permisos;
            return r;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(Rol r)
        {
            usuarioDAL DAL = new usuarioDAL(db);
            Respuesta respuesta = new Respuesta();
            try
            {
                RolDB Original = db.RolesUsuarios.Find(r.RolUsuario.ID_Rol);
                if (Original == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Rol no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    Original.Nombre = r.RolUsuario.Nombre;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        await DAL.Eliminar_PermisosAcceso(r.RolUsuario.ID_Rol);
                        foreach (PermisoAcceso p in r.Permisos)
                        {
                            if (p.Permiso)
                            {
                                PermisoAccesoDB pdb = new PermisoAccesoDB();
                                pdb.ID_Rol = r.RolUsuario.ID_Rol;
                                pdb.ID_ItemMenu = p.ID_ItemMenu;
                                await DAL.Insertar_PermisosAcceso(pdb);
                            }
                        }
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Rol editado exitosamente";
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
        public async Task<Respuesta> Eliminar(Int64 ID_Rol)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                RolDB Eliminado = db.RolesUsuarios.Find(ID_Rol);
                if (Eliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Rol no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Rol eliminado exitosamente";
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
    }
}
