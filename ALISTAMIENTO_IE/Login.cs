using Common.cache;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace LECTURA_DE_BANDA
{
    public partial class Login : Form
    {
        public Usuario? UsuarioAutenticado { get; private set; }

        public Login()
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;

            TXT_USUARIO.Enter += TXT_USUARIO_Enter;
            TXT_USUARIO.Leave += TXT_USUARIO_Leave;
            TXT_CONTRASEÑA.Enter += TXT_CONTRASEÑA_Enter;
            TXT_CONTRASEÑA.Leave += TXT_CONTRASEÑA_Leave;
            TXT_CONTRASEÑA.KeyDown += TXT_CONTRASEÑA_KeyDown;


        }

        private void TXT_USUARIO_Enter(object sender, EventArgs e)
        {
            if (TXT_USUARIO.Text == "USUARIO")
            {
                TXT_USUARIO.Text = "";
                TXT_USUARIO.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void TXT_USUARIO_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXT_USUARIO.Text))
            {
                TXT_USUARIO.Text = "USUARIO";
                TXT_USUARIO.ForeColor = System.Drawing.Color.DimGray;
            }
        }

        private void TXT_CONTRASEÑA_Enter(object sender, EventArgs e)
        {
            if (TXT_CONTRASEÑA.Text == "CONTRASEÑA")
            {
                TXT_CONTRASEÑA.Text = "";
                TXT_CONTRASEÑA.ForeColor = System.Drawing.Color.Black;
                TXT_CONTRASEÑA.UseSystemPasswordChar = true;
            }
        }

        private void TXT_CONTRASEÑA_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TXT_CONTRASEÑA.Text))
            {
                TXT_CONTRASEÑA.Text = "CONTRASEÑA";
                TXT_CONTRASEÑA.ForeColor = System.Drawing.Color.DimGray;
                TXT_CONTRASEÑA.UseSystemPasswordChar = false;
            }
        }

        private void BTN_CERRAR_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BTN_MINIMIZAR_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BTN_LOGIN_Click(object sender, EventArgs e)
        {
            lbl_error.Visible = false;
            if (TXT_USUARIO.Text == "USUARIO" || string.IsNullOrWhiteSpace(TXT_USUARIO.Text))
            {
                msgError("Por favor ingrese un usuario válido.");
                return;
            }
            if (TXT_CONTRASEÑA.Text == "CONTRASEÑA" || string.IsNullOrWhiteSpace(TXT_CONTRASEÑA.Text))
            {
                msgError("Por favor ingrese una contraseña válida.");
                return;
            }

            var usuario = AutenticarUsuario(TXT_USUARIO.Text.Trim(), TXT_CONTRASEÑA.Text.Trim());
            if (usuario == null)
            {
                msgError("Usuario o Contraseña incorrecta.\nPor favor intente nuevamente.");
                return;
            }
            if (!ValidarTurnoPorHora(usuario.LoginNombre))
            {
                msgError("El usuario no tiene permitido acceder en este horario.");
                return;
            }
            UsuarioAutenticado = usuario;
            UserLoginCache.Email = usuario.Email;
            UserLoginCache.LoginName = usuario.LoginNombre;
            UserLoginCache.Postion = usuario.Posicion;
            UserLoginCache.FirstName = usuario.PrimerNombre;
            UserLoginCache.LastName = usuario.Apellido;
            UserLoginCache.idArea = usuario.IdArea;

            // Cargar permisos del usuario para toda la sesión
            Common.cache.UserLoginCache.CargarPermisosEnCache(UserLoginCache.IdUser);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private Usuario? AutenticarUsuario(string login, string password)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT TOP 1 * FROM Usuarios WHERE LoginNombre = @login AND Contraseña = @password AND ID_Area IN(SELECT id_area 
                    from area where AREA like '%LOG%' or AREA LIKE '%alistamiento%')";
                var res = connection.QueryFirstOrDefault<Usuario>(sql, new { login, password });
                if (res == null) return null;

                UserLoginCache.IdUser = res.UsuarioId;
                UserLoginCache.FirstName = res.PrimerNombre;
                UserLoginCache.Postion = res.Posicion; // corregido
                return res;

            }
        }

        private bool ValidarTurnoPorHora(string usuario)
        {
            TimeSpan horaActual = DateTime.Now.TimeOfDay;
            if (usuario.Equals("TURNO1", StringComparison.OrdinalIgnoreCase))
                return horaActual >= new TimeSpan(7, 0, 0) && horaActual < new TimeSpan(15, 0, 0);
            if (usuario.Equals("TURNO2", StringComparison.OrdinalIgnoreCase))
                return horaActual >= new TimeSpan(15, 0, 0) && horaActual < new TimeSpan(23, 0, 0);
            if (usuario.Equals("TURNO3", StringComparison.OrdinalIgnoreCase))
                return horaActual >= new TimeSpan(23, 0, 0) || horaActual < new TimeSpan(7, 0, 0);
            return true;
        }

        private void msgError(string msg)
        {
            lbl_error.Text = "   " + msg;
            lbl_error.Visible = true;
        }

        private void TXT_CONTRASEÑA_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BTN_LOGIN_Click(sender, e);
            }
        }

        private void TXT_USUARIO_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
