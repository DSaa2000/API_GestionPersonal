using API.DataBase;
using API.Models.Usuario;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API.DAL
{
    public class usuarioDAL
    {
        private WebApplicationBuilder builder = WebApplication.CreateBuilder();
        private string connectionString;
        private SitioDB db;
        public usuarioDAL(SitioDB sitioDB)
        {
            connectionString = builder.Configuration.GetConnectionString("SQLServerConnection");
            db = sitioDB;
        }
        public Task<DataSet> Listar_Usuarios()
        {
            return Task.Run(() =>
            {
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sql = "PR_S_USUARIOS";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = sql;
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
        public Task<DataSet> Insertar_PermisosAcceso(PermisoAccesoDB permiso)
        {
            return Task.Run(() =>
            {
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sql = "PR_I_PERMISO_ACCESO_MENU";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = sql;
                        comm.Parameters.Add(new SqlParameter("ID_ItemMenu", permiso.ID_ItemMenu));
                        comm.Parameters.Add(new SqlParameter("ID_Rol", permiso.ID_Rol));
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
        public Task<DataSet> Eliminar_PermisosAcceso(Int64 ID_Rol)
        {
            return Task.Run(() =>
            {
                DataSet dataSet = new DataSet();
                DataTable table = new DataTable();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    String sql = "PR_D_PERMISOS_ACCESO_MENU";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.CommandText = sql;
                        comm.Parameters.Add(new SqlParameter("ID_Rol", ID_Rol));
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
    }
}
