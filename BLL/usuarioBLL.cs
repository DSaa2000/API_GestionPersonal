using API.DAL;
using API.DataBase;
using API.Models.Movimientos;
using API.Models;
using System.Data;
using API.Models.Usuario;

namespace API.BLL
{
    public class usuarioBLL
    {
        private SitioDB db;
        public usuarioBLL(SitioDB sitioDB)
        {
            db = sitioDB;
        }
        public async Task<Respuesta> Listar_Usuarios()
        {
            Respuesta respuesta = new Respuesta();
            usuarioDAL DAO = new usuarioDAL(db);
            List<Usuario> lista = new List<Usuario>();

            DataSet dataSet;
            try
            {
                dataSet = await DAO.Listar_Usuarios();
                if (dataSet.Tables.Count > 0)
                {
                    DataTable tabla = dataSet.Tables[0];
                    if (tabla.Rows.Count > 0)
                    {
                        Usuario u; ;
                        foreach (DataRow fila in tabla.Rows)
                        {
                            u = new Usuario();
                            u.ID_Usuario = Convert.ToInt64(fila["ID_Usuario"]);
                            u.ID_Rol = Convert.ToInt64(fila["ID_Rol"]);
                            u.Nombre = fila["Nombre"].ToString();
                            u.Correo = fila["Correo"].ToString();
                            u.Password = "";// fila["Password"].ToString();
                            u.Rol = new RolDB();
                            u.Rol.ID_Rol = Convert.ToInt64(fila["ID_Rol"]);
                            u.Rol.Nombre = fila["NombreRol"].ToString();
                            lista.Add(u);
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
        public async Task<Respuesta> Insertar_PermisosAcceso(List<PermisoAccesoDB> permisos)
        {
            Respuesta respuesta = new Respuesta();
            usuarioDAL DAO = new usuarioDAL(db);
            List<Usuario> lista = new List<Usuario>();

            DataSet dataSet;
            try
            {
                foreach (PermisoAccesoDB p in permisos)
                {
                    await DAO.Insertar_PermisosAcceso(p);
                }
                respuesta.Codigo = 200;
                respuesta.Status = true;
                respuesta.Mensaje = "Permisos de Acceso registrados Exitosamente";
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
