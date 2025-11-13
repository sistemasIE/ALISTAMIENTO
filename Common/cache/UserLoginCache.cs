using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Configuration.ConfigurationManager;
namespace Common.cache
{

    public static class UserLoginCache
    {
        public static string conexlocal = ConnectionStrings["stringConexionLocal"].ConnectionString;
        public static string conexUnoe = ConnectionStrings["stringConexionUnoe"].ConnectionString;

        public static int IdUser { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }
        public static string LoginName { get; set; }
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

        // Cache de permisos del usuario autenticado
        public static HashSet<string> Permisos { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public static void CargarPermisosEnCache(int usuarioId)
        {
            Permisos.Clear();
            using (var con = new SqlConnection(conexlocal))
            {
                con.Open();

                string sql = @"
            SELECT p.Tipo_permiso
            FROM PERMISOS_USUARIO pu
            JOIN USUARIOS u ON u.UsuarioId = pu.Id_Usuario
            JOIN PERMISOS p ON p.Id_Permiso = pu.Id_Permiso
            WHERE u.UsuarioId = @UsuarioId";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                string permiso = reader.GetString(0).Trim();
                                if (!string.IsNullOrWhiteSpace(permiso))
                                    Permisos.Add(permiso);
                            }
                        }
                    }
                }
            }
        }

        public static bool TienePermisoLike(string fragmento)
        {
            if (string.IsNullOrWhiteSpace(fragmento)) return false;

            // Busca coincidencias parciales en cualquier permiso
            return Permisos.Any(p =>
                p?.IndexOf(fragmento, StringComparison.OrdinalIgnoreCase) >= 0);
        }

    }


}
