namespace ProjetoDBZ.Models
{
    public class Personagem
    {
        // o EF coloca já como primary key 
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
    }
}