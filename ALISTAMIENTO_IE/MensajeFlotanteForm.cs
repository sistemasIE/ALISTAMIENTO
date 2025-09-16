using System;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ALISTAMIENTO_IE
{
    public class MensajeFlotanteForm : Form
    {
        private Timer _timer;
        private Label lblMensaje;
        public MensajeFlotanteForm(string mensaje)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(40, 167, 69); // Verde Bootstrap
            Size = new Size(350, 120);
            TopMost = true;
            ShowInTaskbar = false;

            lblMensaje = new Label
            {
                Text = mensaje,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            Controls.Add(lblMensaje);

            _timer = new Timer { Interval = 2000 };
            _timer.Tick += (s, e) => { _timer.Stop(); Close(); };
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _timer.Start();
        }
    }
}
