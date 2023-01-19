namespace WebApp.Models.ApplicationModels
{
    public class ProprietarioReparticao
    {

        public int Id { get; set; }

        public int ProprietarioFundoId { get; set; }

        public string Reparticao { get; set; }

        public bool Ativo { get; set; }

    }
}