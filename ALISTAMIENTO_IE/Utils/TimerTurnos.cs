using Common.cache;

namespace ALISTAMIENTO_IE.Utils
{
    /// <summary>
    /// Clase que maneja la lógica específica de los turnos, adhiriéndose al principio de Responsabilidad Única.
    /// </summary>
    internal class TimerTurnos : TimerBase
    {
        private readonly Form _parentForm;

        public TimerTurnos(Form parentForm)
        {
            _parentForm = parentForm;
            // Inicia el temporizador automáticamente al ser instanciado.
            this.Start();
        }

        protected override void OnTick(object? sender, System.EventArgs e)
        {
            CheckAndWarnIfShiftEnding();
        }

        private void CheckAndWarnIfShiftEnding()
        {
            TimeSpan horaActual = DateTime.Now.TimeOfDay;
            string loginUsuario = UserLoginCache.LoginName.ToUpper();

            // Lógica para el turno 1 (7:00 a 14:59) || VALIDA HASTA DENTRO DE 3 HORAS
            if (loginUsuario == "TURNO1" && horaActual >= new TimeSpan(14, 55, 0) && horaActual < new TimeSpan(15, 59, 0))
            {
                ShowSessionEndWarning();
            }
            // Lógica para el turno 2 (15:00 a 22:59) || VALIDA HASTA DENTRO DE 3 HORAS
            else if (loginUsuario == "TURNO2" && horaActual >= new TimeSpan(22, 55, 0) && horaActual < new TimeSpan(23, 59, 0))
            {
                ShowSessionEndWarning();
            }
            // Lógica para el turno 3 (23:00 a 6:59) || VALIDA HASTA DENTRO DE 3 HORAS
            else if (loginUsuario == "TURNO3" && horaActual >= new TimeSpan(6, 55, 0) && horaActual < new TimeSpan(9, 59, 0))
            {
                ShowSessionEndWarning();
            }
        }

        private void ShowSessionEndWarning()
        {
            // Revisa si ya hay un cuadro de diálogo visible para evitar múltiples mensajes
            if (Application.OpenForms["WarningMessage"] == null)
            {
                // NOTA: Usar un formulario personalizado es mejor que MessageBox para evitar problemas con la ventana principal.
                // Aquí usamos MessageBox como ejemplo simple.
                MessageBox.Show(
                    "Tu sesión está a punto de expirar debido al cambio de turno.\nPor favor, guarda tu trabajo y cierra la sesión.",
                    "Advertencia de Cierre de Sesión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }
    }
}
