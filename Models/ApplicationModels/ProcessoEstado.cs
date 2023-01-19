namespace WebApp.Models.ApplicationModels
{
    public class ProcessoEstado
    {

        public int Id { get; set; }

        public int ProcessoIndividualId { get; set; }

        public int ProcessoDocumentalId { get; set; }

        public ProcessoTipoEstado ProcessoTipoEstado { get; set; }

        public DateTime Data { get; set; }

    }
}