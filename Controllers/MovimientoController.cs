using API.BLL;
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
    public class MovimientoController : ControllerBase
    {
        private SitioDB db;
        public MovimientoController(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<Respuesta> Crear(MovimientoDB m)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                if (ModelState.IsValid)
                {
                    //m.Fecha = ConvertDate(m.Fecha.ToString("yyyy-MM-dd"));
                    m.Fecha = Convert.ToDateTime(m.Fecha);
                    db.Movimientos.Add(m);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Movimiento creado exitosamente";
                    respuesta.Status = true;
                    respuesta.Item = m.ID_Movimiento;
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
        public async Task<MovimientoDB> Get(Int64 ID_Movimiento)
        {
            MovimientoDB m = await db.Movimientos.FindAsync(ID_Movimiento);
            if (m == null)
            {
                m = new MovimientoDB();
            }
            return m;
        }
        [HttpGet]
        [Route("ListSP")]
        public async Task<List<Movimiento>> ListarSP()
        {
            movimientoBLL BLL = new movimientoBLL(db);
            List<Movimiento> mtList = new List<Movimiento>();
            try
            {
                Respuesta respuesta = new Respuesta();
                respuesta =await BLL.Listar_Movimientos();
                if (respuesta.Status)
                {
                    mtList = (List<Movimiento>)respuesta.Item;
                }
            }
            catch (Exception ex)
            {
                
            }
            return mtList;
        }
        [HttpGet]
        [Route("List")]
        public async Task<List<Movimiento>> Listar()
        {
            List<MovimientoDB> m = await db.Movimientos.ToListAsync();
            List<Movimiento> mtList = new List<Movimiento>();                                                             
            m.ForEach((mov) =>
            {
                Movimiento mt = new Movimiento();
                mt.MovimientoDB = mov;
                mt.Fuente = new FuenteDB();
                mt.Fuente = db.Fuentes.Find(mov.ID_Fuente);
                mt.TipoMovimiento = new TipoMovimientoDB();
                mt.TipoMovimiento = db.TiposMovimientos.Find(mov.ID_TipoMovimiento);
                mtList.Add(mt);
            });
            return mtList;
        }
        [HttpPut]
        [Route("Edit")]
        public async Task<Respuesta> Editar(MovimientoDB m)
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                MovimientoDB Original = db.Movimientos.Find(m.ID_Movimiento);
                if (Original == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Movimiento no encontrado";
                    respuesta.Status = false;
                    return respuesta;
                }
                else
                {
                    Original.Fecha = m.Fecha;
                    Original.Motivo = m.Motivo;
                    Original.ID_TipoMovimiento = m.ID_TipoMovimiento;
                    Original.Monto = m.Monto;

                    if (await TryUpdateModelAsync(Original))
                    {
                        db.SaveChanges();
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "Movimiento editado exitosamente";
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
        public async Task<Respuesta> Eliminar(Int64 ID_Movimiento)
        {

            Respuesta respuesta = new Respuesta();
            try
            {
                MovimientoDB Eliminado = db.Movimientos.Find(ID_Movimiento);
                if (Eliminado == null)
                {
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Movimiento no encontrado";
                    respuesta.Status = true;
                }
                else
                {
                    db.Remove(Eliminado);
                    db.SaveChanges();
                    respuesta.Codigo = 200;
                    respuesta.Mensaje = "Movimiento eliminado exitosamente";
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
        private DateTime ConvertDate(string date)
        {
            var items = new List<int>();
            
            try
            {
                items.AddRange(date.Split('-').Select(d => Convert.ToInt32(d)));
                return new DateTime(items[2], items[0], items[1]);
            }
            catch (Exception)
            {
                items.AddRange(date.Split('-').Select(d => Convert.ToInt32(d)));
                return new DateTime(items[2], items[1], items[0]);
            }
        }
    }
}
