
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

        public ICommand IniciarJuegoCommand{ get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public QuizViewModel() 
        {
            IniciarJuegoCommand = new Command(IniciarJuego);
        }

        private void IniciarJuego()
        {
            Shell.Current.GoToAsync("//Juego");

        }
        private void Deserializar()
        {
            Stream s = FileSystem.OpenAppPackageFileAsync("pokemon.json").Result;
            var datos  =   JsonSerializer.Deserialize<List<PokemonModel>>(s);

        }
    }
}
