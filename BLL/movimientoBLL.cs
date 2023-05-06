using API.Controllers;
using API.DAL;
using API.DataBase;
using API.Models;
using API.Models.Movimientos;
using System.Data;

namespace API.BLL
{
    public class movimientoBLL
    {
        private SitioDB db;
        public movimientoBLL(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        public async Task<Respuesta> Listar_Movimientos()
        {
            Respuesta respuesta = new Respuesta();
            movimientoDAL DAO = new movimientoDAL(db);
            List<Movimiento> lista = new List<Movimiento>();

            DataSet dataSet;
            try
            {
                dataSet = await DAO.Listar_Movimientos();
                if (dataSet.Tables.Count > 0)
                {
                    DataTable tabla = dataSet.Tables[0];
                    if (tabla.Rows.Count > 0)
                    {
                        Movimiento m;;
                        foreach (DataRow fila in tabla.Rows)
                        {
                            m = new Movimiento();
                            m.MovimientoDB = new MovimientoDB();
                            m.MovimientoDB.ID_Movimiento = Convert.ToInt64(fila["ID_Movimiento"]);
                            m.MovimientoDB.ID_Fuente = Convert.ToInt64(fila["ID_Fuente"]);
                            m.MovimientoDB.ID_TipoMovimiento = Convert.ToInt64(fila["ID_TipoMovimiento"]);
                            m.MovimientoDB.Motivo = fila["Motivo"].ToString();
                            m.MovimientoDB.Fecha = Convert.ToDateTime(fila["Fecha"]);
                            m.MovimientoDB.Monto = Convert.ToDouble(fila["Monto"]);
                            m.Fuente = new FuenteDB();
                            m.Fuente.ID_Fuente = Convert.ToInt64(fila["ID_Fuente"]);
                            m.Fuente.Nombre = fila["NombreFuente"].ToString();
                            m.TipoMovimiento = new TipoMovimientoDB();
                            m.TipoMovimiento.ID_TipoMovimiento = Convert.ToInt64(fila["ID_TipoMovimiento"]);
                            m.TipoMovimiento.Nombre = fila["NombreTipoMovimiento"].ToString();
                            lista.Add(m);                            
                        }
                        respuesta.Codigo = 200;
                        respuesta.Mensaje = "ok";
                        respuesta.Status = true;
                        respuesta.Item = lista;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta.Codigo = 1;
                respuesta.Status = false;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
