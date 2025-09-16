using System.Data.SqlClient;
using static System.Configuration.ConfigurationManager;
namespace Common.cache
{

    public static class UserLoginCache
    {
        public static string conexlocal = ConnectionStrings["stringConexionLocal"].ConnectionString;
        public static string conexUnoe = ConnectionStrings["stringConexionUnoe"].ConnectionString;

        public static int IdUser { get; set; }
        public static string LoginName { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string Postion { get; set; }
        public static string Email { get; set; }
        public static int idArea { get; set; }

        public static int lote { get; set; }
        public static int item { get; set; }
        public static int item_laminacion { get; set; }
        public static int item_telares { get; set; }

        public static int item_saco { get; set; }

        public static string abierto_por { get; set; }

        public static int lote_trazabilidad { get; set; }

        public static string operario_impresion { get; set; }

        public static string operario_laminacion { get; set; }
        public static string operario_telares { get; set; }

        public static int item_impresion { get; set; }



        public static string RetornaNombrePorId(int idUsuario)
        {
            using (var con = new SqlConnection(conexlocal))
            {
                con.Open();

                using (var comando = new SqlCommand("select PrimerNombre + ' ' + Apellido from Usuarios where UsuarioID = " + idUsuario + "", con))
                {
                    using (var reader = comando.ExecuteReader())
                    {
                        reader.Read();
                        if (reader.HasRows)
                        {
                            return reader.GetString(0);
                        }
                        else
                        {
                            return "No nombre";
                        }
                    }
                }
            }
        }





    }


}
