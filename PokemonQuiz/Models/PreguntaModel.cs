using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonQuiz.Models
{
    public class PreguntaModel
    {
        public string Pregunta { get; set; } = "";
        public string Respuesta { get; set; } = "";
        public List<string> Distractores { get; set; } = new();

    }
}
