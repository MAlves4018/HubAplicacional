namespace WebApp.Models.ApplicationModels
{
    public class ProcessoLocalizacao
    {

        public int Id { get; set; }

        public int ProcessoIndividualId { get; set; }

        public int ProcessoDocumentalId { get; set; }

        public string LocalizacaoEdificioId { get; set; }

        public string Estante { get; set; }

        public string Prateleira { get; set; }

        public string NrCota { get; set; }

        public DateTime Data { get; set; }

    }
}