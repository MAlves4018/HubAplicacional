namespace WebApp.Models.ApplicationModels
{
    public class ProcessoIndividual
    {

        public int Id { get; set; }

        public int ProprietarioReparticaoId { get; set; }

        public string Nome { get; set; }

        public DateTime Nascimento { get; set; }

        public string BI_CC { get; set; }

        public string NIF { get; set; }

        public string NIM { get; set; }

        public string Categoria { get; set; }

        public string Posto { get; set; }

        public string Pai { get; set; }

        public string Mae { get; set; }

        public string NaturalidadePais { get; set; }

        public string NaturalidadeConcelho { get; set; }

        public string NaturalidadeFreguesia { get; set; }


    }
}