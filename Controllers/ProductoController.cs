using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using Microsoft.EntityFrameworkCore;
using API.DataBase;
using API.DAL;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private SitioDB db;
        private readonly ILogger<ProductoController> _logger;
        public ProductoController(ILogger<ProductoController> logger, SitioDB sitioDB)
        {
            _logger = logger;
            db = sitioDB;

        }
        [HttpPost]
        [Route("Test")]
        public async Task<Respuesta> Test(Producto p)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                db.Database.ExecuteSqlRaw("SELECT count(*) FROM 'Movimientos'");
                
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
        [Route("Create")]
        public async Task<Respuesta> Crear(Producto p)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Productos.Add(p);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Producto creado exitosamente";
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
                respuesta.Mensaje = "Error: "+ex.Message;
                respuesta.Status = false;
            }
            return respuesta;
        }
        [HttpGet]
        [Route("Get")]
        public async Task<Producto> Get(Int64 ID_Producto)
        {
            Producto product = await db.Productos.FindAsync(ID_Producto);
            if (product == null)
            {
                product = new Producto();
                product.ID_Producto = 0;
                product.Nombre = "";
            }
            return product;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<Producto>> Listar()
        {
            List<Producto> products = await db.Productos.ToListAsync();

            return products;

        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(Producto producto)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                Producto productoOriginal = db.Productos.Find(producto.ID_Producto);
                if (productoOriginal == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Producto no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    productoOriginal.Nombre = producto.Nombre;
                   
                    if (await TryUpdateModelAsync(productoOriginal))
                    {
                        db.SaveChanges();
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Producto editado exitosamente";
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
        public async Task<Respuesta> Eliminar(Int64 ID_Producto)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                Producto productoEliminado = db.Productos.Find(ID_Producto);
                if (productoEliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Producto no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(productoEliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Producto eliminado exitosamente";
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
