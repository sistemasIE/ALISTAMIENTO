namespace ALISTAMIENTO_IE.DTOs
{
    public class PlanificacionCamionDTO
    {
        public int Item { get; set; }
        public string Descripcion { get; set; }

        public float PacasEsperadas { get; set; }

        public float KilosEsperados { get; set; }
    }


    public class InformacionCamionDTO : PlanificacionCamionDTO
    {


        public float PacasAlistadas { get; set; }
        public float KilosAlistados { get; set; }

        public float PacasRestantes { get; set; }

        public float KilosRestantes { get; set; }

    }
}
