
using PokemonQuiz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PokemonQuiz.ViewModels
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private List<PokemonModel> Pokemones { get; set; } = new();
        private List<PokemonModel> Distractores { get; set; } = new();
        byte vidas;
        public string Vidas
        {
            get
            {
                if (vidas == 3)
                    return " Vidas: 💛💛💛";
                else if (vidas == 2)
                    return "Vidas: 💛💛";
                else
                    return "Vidas: 💛";
            }
        }
        public byte Aciertos => (byte)(150 - Pokemones.Count);
        public double Progreso => Aciertos/150f;
        private PokemonModel pokemonSeleccionado;
        private Random r = new();
        public PreguntaModel Pregunta { get; set; } = new();

        public ICommand IniciarJuegoCommand{ get; set; }
        public ICommand VerificarRespuestaCommand { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public QuizViewModel() 
        {
            IniciarJuegoCommand = new Command(IniciarJuego);
            VerificarRespuestaCommand = new Command<String>(VerificarRespuesta);
            Deserializar();
        }

        void VerificarRespuesta(string respuesta)
        {
            if(respuesta == pokemonSeleccionado.nombre)
            {
                Pokemones.Remove(pokemonSeleccionado);
                PropertyChanged?.Invoke(this, new(nameof(Progreso)));
                if(Pokemones.Count == 0)
                {
                    Shell.Current.GoToAsync("//Resultados");
                    PropertyChanged?.Invoke(this, new(nameof(Aciertos)));
                    return;
                }
                CrearPregunta();
            }
            else
            {
                vidas--;
                if(vidas == 0)
                {
                    Shell.Current.GoToAsync("//Resultados");
                    PropertyChanged?.Invoke(this, new(nameof(Aciertos)));
                    return;
                }
                PropertyChanged?.Invoke(this, new(nameof(Vidas)));
                CrearPregunta();
            }
        }

        private void IniciarJuego()
        {
            
            Pokemones.Clear();
            Pokemones.AddRange(Distractores);
            CrearPregunta();
            vidas = 3;
            PropertyChanged?.Invoke(this, new(nameof(Vidas)));
            PropertyChanged?.Invoke(this, new(nameof(Progreso)));
            Shell.Current.GoToAsync("//Juego");
        }
        private void CrearPregunta()
        {
            pokemonSeleccionado = Pokemones[r.Next(0, Pokemones.Count)];
            var pokemonesDistractores = Distractores.Where(x => x.nombre != pokemonSeleccionado.nombre).
                OrderBy(x => r.Next()).Take(3).ToList();
            pokemonesDistractores.Insert(r.Next(0, pokemonesDistractores.Count), pokemonSeleccionado);

            Pregunta = new()
            {
                Distractores = pokemonesDistractores.Select(x => x.nombre).ToList(),
                Respuesta = pokemonSeleccionado.nombre,
                Pregunta = pokemonSeleccionado.imagen
            };
            PropertyChanged?.Invoke(this, new(nameof(Pregunta)));
        }
        private void Deserializar()
        {
            Stream s = FileSystem.OpenAppPackageFileAsync("pokemon.json").Result;
            var datos = JsonSerializer.Deserialize<List<PokemonModel>>(s);
            if (datos != null)
            {
                Distractores.AddRange(datos);
            }
          

        }


    }
}
