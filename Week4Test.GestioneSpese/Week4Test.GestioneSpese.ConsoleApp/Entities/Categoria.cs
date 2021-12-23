namespace Week4Test.GestioneSpese.ConsoleApp
{
    internal class Categoria
    {
        public int Id { get; set; }
        public string NomeCategoria { get; set; }

        public List<Spesa> Spese { get; set; } = new List<Spesa>();


        public override string ToString()
        {
            return $"{Id} - {NomeCategoria}";
        }
    }
}