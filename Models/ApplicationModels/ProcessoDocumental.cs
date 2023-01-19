namespace WebApp.Models.ApplicationModels
{
    public class ProcessoDocumental
    {

        public int Id { get; set; }

        public int ProprietarioReparticaoId { get; set; }

        public int RCAEId { get; set; }

        public string Assunto { get; set; }

        public DateTime DataExtremaInicial { get; set; }

        public DateTime DataExtremaFinal { get; set; }

        public string Observacoes { get; set; }

    }
}