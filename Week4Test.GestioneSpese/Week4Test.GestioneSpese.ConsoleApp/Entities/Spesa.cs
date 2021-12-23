using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week4Test.GestioneSpese.ConsoleApp
{
    internal class Spesa
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Descrizione { get; set; }
        public string Utente { get; set; }
        public double Importo { get; set; }
        public bool Approvato { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public override string ToString()
        {
            string state;
            if (Approvato)
                 state = "Approvato";
            else
                state = "Non approvato";

            return $"{Id} - {Data.ToShortDateString()} - {Categoria.NomeCategoria} - {Descrizione} - {Importo} euro - {state}";
        }
    }
}
