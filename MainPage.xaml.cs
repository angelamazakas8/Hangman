
using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Hangman
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        #region UIProperties

        public string Spotlight
        {
            get => spotlight;
            set
            {
                spotlight = value;
                OnPropertyChanged();
            }
        }

        public List<char> Letters
        {
            get => letters;
            set
            {
                letters = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public string GameStatus 
        {
            get => gameStatus;
            set
            {
                gameStatus = value;
                OnPropertyChanged();
            }

        }

        public string CurrentImage
        {
            get => currentImage;
            set
            {
                currentImage = value;
                OnPropertyChanged();
            }

        }
        #endregion

        #region Fields
        List<string> words = new List<string>
                ()
                {
                "algorithm",
                "database",
                "encryption",
                "firewall",
                "internet",
                "javascript",
                "keyboard",
                "laptop",
                "monitor",
                "network",
                "password",
                "software",
                "smartphone",
                "swift",
                "technology",
                "username",
                "virtual",
                "browser",
                "cybersecurity",
                "ethernet"
                };


        string answer = "";
        private string spotlight;
        List<char> guessed = new List<char>();
        private List<char> letters = new List<char> ();
        private string message;
        int mistakes = 0;
        private string gameStatus;
        int maxWrong = 6;
        private string currentImage = "hangman1.png";

        #endregion
        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
            BindingContext = this;
            PickWord();
            CalculateWord(answer, guessed);
        }
        #region GameEngine
        private void PickWord()
        {
            answer = words[new Random().Next(0, words.Count)];
            Debug.WriteLine(answer);
        }
        private void CalculateWord(string answer, List<char> guessed)
        {
            var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();
            Spotlight = string.Join(' ', temp);
        }

        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }

        private void HandleGuess(char letter)
        {
            if(guessed.IndexOf(letter) == -1)
            {
                guessed.Add(letter);
            }

            if(answer.IndexOf(letter) >= 0)
            {
                CalculateWord(answer, guessed);
                CheckIfGameWon();
            }

            else if(answer.IndexOf(letter) == -1) 
            {
                mistakes++;
                UpdateStatus();
                CheckIfGameLost();
                CurrentImage = $"hangman{mistakes + 1}.png";
            }

        }

        private void CheckIfGameLost()
        {
            if(mistakes == maxWrong)
            {
                Message = "You Lose! Try Again!";
                DisableLetters();
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Errors: {mistakes} of {maxWrong}";
        }

        private void CheckIfGameWon()
        {
            if(Spotlight.Replace(" ", "") == answer)
            {
                Message = "You Win!";
                UpdateStatus();
                DisableLetters();
            }
        }

        private void DisableLetters()
        {
            foreach(var children in LettersContainer.Children)
            {
                var btn = children as Button;
                if(btn != null)
                {
                    btn.IsEnabled = false;
                }
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            mistakes = 0;
            guessed = new List<char>();
            CurrentImage = "hangman1.png";
            PickWord();
            CalculateWord(answer, guessed);
            Message = "";
            UpdateStatus();
            EnableLetters();
        }

        private void EnableLetters()
        {
            foreach (var children in LettersContainer.Children)
            {
                var btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }

    }

}
