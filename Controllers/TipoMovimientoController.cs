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
    public class TipoMovimientoController : ControllerBase
    {
        private SitioDB db;
        private readonly ILogger<TipoMovimientoController> _logger;
        public TipoMovimientoController(ILogger<TipoMovimientoController> logger, SitioDB sitioDB)
        {
            _logger = logger;
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(TipoMovimientoDB tm)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    db.TiposMovimientos.Add(tm);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "TipoMovimiento creado exitosamente";
                    respuesta.Status = true;
                    respuesta.Item = tm.ID_TipoMovimiento;
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
        public async Task<TipoMovimientoDB> Get(Int64 ID_TipoMovimiento)
        {
            TipoMovimientoDB tm = await db.TiposMovimientos.FindAsync(ID_TipoMovimiento);
            if (tm == null)
            {
                tm = new TipoMovimientoDB();
                tm.ID_TipoMovimiento = 0;
                tm.Nombre = "";
            }
            return tm;

        }
        [HttpGet]
        [Route("List")]
        public async Task<List<TipoMovimientoDB>> Listar()
        {
            List<TipoMovimientoDB> fuentes = await db.TiposMovimientos.ToListAsync();
            return fuentes;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(TipoMovimientoDB tm)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                TipoMovimientoDB Original = db.TiposMovimientos.Find(tm.ID_TipoMovimiento);
                if (Original == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "TipoMovimiento no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    Original.Nombre = tm.Nombre;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "TipoMovimiento editado exitosamente";
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
        public async Task<Respuesta> Eliminar(Int64 ID_TipoMovimiento)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                TipoMovimientoDB Eliminado = db.TiposMovimientos.Find(ID_TipoMovimiento);
                if (Eliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "TipoMovimiento no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "TipoMovimiento eliminado exitosamente";
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
