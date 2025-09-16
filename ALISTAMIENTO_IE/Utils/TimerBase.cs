namespace ALISTAMIENTO_IE.Utils
{
    /// <summary>
    /// Clase base abstracta para encapsular la funcionalidad de un temporizador de Windows Forms.
    /// Esto permite reutilizar y extender la lógica del temporizador.
    /// </summary>
    internal abstract class TimerBase
    {
        protected readonly System.Windows.Forms.Timer _timer;

        public TimerBase()
        {
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += OnTick;
            // Configurar el intervalo por defecto en 5 minutos (300,000 milisegundos).
            _timer.Interval = 30000;
        }

        // Método abstracto que debe ser implementado por las clases derivadas.
        protected abstract void OnTick(object? sender, System.EventArgs e);

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
