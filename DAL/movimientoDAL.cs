using System.Data.SqlClient;
using System.Data;
using System;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using API.Controllers;
using API.DataBase;
using Microsoft.EntityFrameworkCore;

namespace API.DAL
{
    public class movimientoDAL
    {
        private WebApplicationBuilder builder = WebApplication.CreateBuilder();
        private string connectionString;
        private SitioDB db;
        public movimientoDAL(SitioDB sitioDB)
        {
            connectionString = builder.Configuration.GetConnectionString("SQLServerConnection");
            db = sitioDB;
        }
        public Task<DataSet> Listar_Movimientos()
        {
            return Task.Run(() =>
            {
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sql = "PR_S_MOVIMIENTOS";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = sql;
                        /*
                        
                        comm.Parameters.Add(new SqlParameter("ID_PRODUCTO", inventario.IdProducto));
                        comm.Parameters.Add(new SqlParameter("ID_FUNDO", inventario.IdFundo));
                        comm.Parameters.Add(new SqlParameter("ID_USUARIO", inventario.IdUsuario));
                        comm.Parameters.Add(new SqlParameter("CANT_PRODUCTO", inventario.CantidadProducto));
                        comm.Parameters.Add(new SqlParameter("VALOR_PRODUCTO", inventario.ValorProducto));
                         */
                        using (SqlDataReader rdr = comm.ExecuteReader())
                        {
                            table.Load(rdr);
                            dataSet.Tables.Add(table);

                        }
                    }
                    return dataSet;
                }

            });
        }

        public int Test()
        {
            
            using (EntityConnection conn = new EntityConnection(connectionString))
            {
                conn.Open();
                // Create an EntityCommand.
                using (EntityCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM 'Movimientos'";
                    cmd.CommandType = CommandType.Text;
                    //EntityParameter param = new EntityParameter();
                    //param.Value = 2;
                    //param.ParameterName = "StudentID";
                    //cmd.Parameters.Add(param);

                    // Execute the command.
                    using (EntityDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                    {
                        // Read the results returned by the stored procedure.
                        while (rdr.Read())
                        {
                            Console.WriteLine("Exito");
                        }
                    }
                }
                conn.Close();
            }
            return 0;
        }
    }
    
}
