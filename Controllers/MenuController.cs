using API.DataBase;
using API.Models.Movimientos;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models.UtilidadGeneral;
using Microsoft.EntityFrameworkCore;
using API.Models.Usuario;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private SitioDB db;
        public MenuController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(ItemMenuDB im)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.ItemsMenu.Add(im);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "ItemMenu creado exitosamente";
                    respuesta.Item = im.ID_ItemMenu;
                    respuesta.Status = true;
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
        public async Task<ItemMenuDB> Get(Int64 ID_ItemMenu)
        {
            ItemMenuDB im = await db.ItemsMenu.FindAsync(ID_ItemMenu);
            if (im == null)
            {
                im = new ItemMenuDB();
                im.ID_ItemMenu = 0;
                im.ID_ItemPadre = 0;
                im.Titulo = "";
                im.Url = "";
                im.Icon = "";                
            }
            return im;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<ItemMenu>> Listar(Int64 ID_Rol) // Solo 3 niveles de profundidad
        {
            List<Int64> permisos = new List<Int64>();
            List<PermisoAccesoDB> listaPermisosDB = db.PermisosAcceso.ToList().Where(p => p.ID_Rol == ID_Rol).ToList();
            List<ItemMenuDB> list = db.ItemsMenu.ToList();
           
            foreach (PermisoAccesoDB p in listaPermisosDB)
            {
                permisos.Add(p.ID_ItemMenu);
            }
            List<ItemMenu> listOrdenada = new List<ItemMenu>();
            list = list.Where(i => permisos.Contains(i.ID_ItemMenu)).ToList();
            List<Int64> ids = new List<Int64>();
            foreach (ItemMenuDB item in list)
            {
                if (!ids.Contains(item.ID_ItemMenu) && item.ID_ItemPadre == 0)
                {
                    ids.Add(item.ID_ItemMenu);
                    ItemMenu im = new ItemMenu();
                    im.ID_ItemMenu = item.ID_ItemMenu;
                    im.ID_ItemPadre = item.ID_ItemPadre;
                    im.label = item.Titulo;
                    im.url = item.Url;
                    im.icon = item.Icon;
                    im.Permiso = true;
                    im.Prioridad = item.Prioridad;
                    im.children = new List<ItemMenu>();
                    List<ItemMenuDB> l = list.Where(i => i.ID_ItemPadre == im.ID_ItemMenu).ToList();
                    foreach (ItemMenuDB item2 in l)
                    {
                        if (!ids.Contains(item2.ID_ItemMenu))
                        {
                            ids.Add(item2.ID_ItemMenu);
                            ItemMenu im2 = new ItemMenu();
                            im2.ID_ItemMenu = item2.ID_ItemMenu;
                            im2.ID_ItemPadre = item2.ID_ItemPadre;
                            im2.label = item2.Titulo;
                            im2.url = item2.Url;
                            im2.Permiso = true;
                            im2.Prioridad = item2.Prioridad;
                            im2.icon = item2.Icon;
                            im2.children = new List<ItemMenu>();
                            List<ItemMenuDB> l2 = list.Where(i => i.ID_ItemPadre == im2.ID_ItemMenu).ToList();
                            foreach (ItemMenuDB item3 in l2)
                            {
                                if (!ids.Contains(item3.ID_ItemMenu))
                                {
                                    ids.Add(item3.ID_ItemMenu);
                                    ItemMenu im3 = new ItemMenu();
                                    im3.ID_ItemMenu = item3.ID_ItemMenu;
                                    im3.ID_ItemPadre = item3.ID_ItemPadre;
                                    im3.label = item3.Titulo;
                                    im3.url = item3.Url;
                                    im3.Permiso = true;
                                    im3.Prioridad = item3.Prioridad;
                                    im3.icon = item3.Icon;
                                    im3.children = new List<ItemMenu>();
                                    List<ItemMenuDB> l3 = list.Where(i => i.ID_ItemPadre == im3.ID_ItemMenu).ToList();
                                    /* CONTINUAR EN 4TO NIVEL */
                                    foreach (ItemMenuDB item4 in l3)
                                    {
                                        if (!ids.Contains(item4.ID_ItemMenu) )
                                        {
                                            ids.Add(item4.ID_ItemMenu);
                                            ItemMenu im4 = new ItemMenu();
                                            im4.ID_ItemMenu = item4.ID_ItemMenu;
                                            im4.ID_ItemPadre = item4.ID_ItemPadre;
                                            im4.label = item4.Titulo;
                                            im4.Permiso = true;
                                            im4.url = item4.Url;
                                            im4.Prioridad = item4.Prioridad;
                                            im4.icon = item4.Icon;
                                            im4.children = new List<ItemMenu>();
                                            /* CONTINUAR EN 4TO NIVEL */
                                            //im3.children = im3.children.OrderBy(i => i.Prioridad).ToList();
                                            im3.children.Add(im4);
                                        }
                                    }
                                    im2.children.Add(im3);
                                }
                            }
                            im2.children = im2.children.OrderBy(i => i.Prioridad).ToList();
                            im.children.Add(im2);

                        }                    
                    }
                    im.children = im.children.OrderBy(i => i.Prioridad).ToList();
                    listOrdenada.Add(im);
                }               
            }
            return listOrdenada.OrderBy(i => i.Prioridad).ToList();
        }
        [HttpGet]
        [Route("Listar_Items")]
        public async Task<List<ItemMenu>> Listar_Items() // Solo 3 niveles de profundidad
        {
            List<ItemMenuDB> list = await db.ItemsMenu.ToListAsync();
            List<ItemMenu> lista = new List<ItemMenu>();
            foreach (ItemMenuDB item in list)
            {
                ItemMenu im = new ItemMenu();
                im.ID_ItemMenu = item.ID_ItemMenu;
                im.ID_ItemPadre = item.ID_ItemPadre;
                im.label = item.Titulo;
                im.url = item.Url;
                im.icon = item.Icon;
                im.Prioridad = item.Prioridad;
                im.children = new List<ItemMenu>();
                lista.Add(im);
                
            }
            return lista.OrderBy(i => i.ID_ItemMenu).ToList();
        }
        [HttpGet]
        [Route("Listar_Modulos")]
        public async Task<List<ItemMenu>> ListarModulos() // Solo 3 niveles de profundidad
        {
            List<ItemMenuDB> list = await db.ItemsMenu.ToListAsync();
            List<ItemMenu> listOrdenada = new List<ItemMenu>();
            List<Int64> ids = new List<Int64>();
            foreach (ItemMenuDB item in list)
            {
                if (!ids.Contains(item.ID_ItemMenu) && item.ID_ItemPadre == 0)
                {
                    ids.Add(item.ID_ItemMenu);
                    ItemMenu im = new ItemMenu();
                    im.ID_ItemMenu = item.ID_ItemMenu;
                    im.ID_ItemPadre = item.ID_ItemPadre;
                    im.label = item.Titulo;
                    im.url = item.Url;
                    im.icon = item.Icon;
                    im.Prioridad = item.Prioridad;
                    im.children = new List<ItemMenu>();
                    listOrdenada.Add(im);
                }
            }
            return listOrdenada.OrderBy(i => i.Prioridad).ToList();
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(ItemMenuDB im)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                ItemMenuDB Original = db.ItemsMenu.Find(im.ID_ItemMenu);
                if (Original == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "ItemMenu no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    Original.Titulo = im.Titulo;
                    Original.ID_ItemPadre = im.ID_ItemPadre;
                    Original.Url = im.Url;
                    Original.Icon = im.Icon;
                    Original.Prioridad = im.Prioridad;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "ItemMenu editado exitosamente";
                        respuesta.Status = true;
                    }
                    else
                    {
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Error al editar el producto";
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
        public async Task<Respuesta> Eliminar(Int64 ID_ItemMenu)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                ItemMenuDB Eliminado = db.ItemsMenu.Find(ID_ItemMenu);
                if (Eliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "ItemMenu no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "ItemMenu eliminado exitosamente";
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