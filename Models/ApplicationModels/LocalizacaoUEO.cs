namespace WebApp.Models.ApplicationModels
{
    public class LocalizacaoUEO
    {

        public int Id { get; set; }

        public string UEO { get; set; }

        public bool Ativo { get; set; }


        public List<LocalizacaoEdificio>? LocalizacaoEdificios { get; set; }
    }
}