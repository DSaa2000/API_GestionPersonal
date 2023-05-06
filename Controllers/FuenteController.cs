using API.DataBase;
using API.Models;
using API.Models.Movimientos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuenteController : ControllerBase    
    {
        private SitioDB db;
        public FuenteController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(FuenteDB f)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.Fuentes.Add(f);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Fuente creado exitosamente";
                    respuesta.Status = true;
                    respuesta.Item = f.ID_Fuente;
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
        public async Task<FuenteDB> Get(Int64 ID_Fuente)
        {
            FuenteDB f = await db.Fuentes.FindAsync(ID_Fuente);
            if (f == null)
            {
                f = new FuenteDB();
                f.ID_Fuente = 0;
                f.Nombre = "";
            }
            return f;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<FuenteDB>> Listar()
        {
            List<FuenteDB> list = await db.Fuentes.ToListAsync();
            return list;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(FuenteDB f)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                FuenteDB Original = db.Fuentes.Find(f.ID_Fuente);
                if (Original == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Fuente no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    Original.Nombre = f.Nombre;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Fuente editado exitosamente";
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
        public async Task<Respuesta> Eliminar(Int64 ID_Fuente)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                FuenteDB Eliminado = db.Fuentes.Find(ID_Fuente);
                if (Eliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Fuente no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Fuente eliminado exitosamente";
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