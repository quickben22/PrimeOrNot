using MLToolkit.Forms.SwipeCardView.Core;
using PrimeOrNot.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Lottie.Forms;
using System.Collections.Generic;
using System.Text;

namespace PrimeOrNot.ViewModel
{
    public class TinderPageViewModel : BasePageViewModel
    {
        private ObservableCollection<Number> _numbers = new ObservableCollection<Number>();

        private uint _threshold;

        public TinderPageViewModel(List<Number> numbers = null)
        {
            if (numbers == null)
                InitializeProfiles();
            else
                addProfiles(numbers);
            Threshold = (uint)(App.ScreenWidth / 3);

            SwipedCommand = new Command<SwipedCardEventArgs>(OnSwipedCommand);
            DraggingCommand = new Command<DraggingCardEventArgs>(OnDraggingCommand);


            ClearItemsCommand = new Command(OnClearItemsCommand);
            AddItemsCommand = new Command(OnAddItemsCommand);



        }

        public ObservableCollection<Number> Numbers
        {
            get => _numbers;
            set
            {
                _numbers = value;
                RaisePropertyChanged();
            }
        }

        public uint Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SwipedCommand { get; }

        public ICommand DraggingCommand { get; }

        public ICommand ClearItemsCommand { get; }

        public ICommand AddItemsCommand { get; }

        private void OnSwipedCommand(SwipedCardEventArgs eventArgs)
        {
        }

        private void OnDraggingCommand(DraggingCardEventArgs eventArgs)
        {
            switch (eventArgs.Position)
            {
                case DraggingCardPosition.Start:
                    return;

                case DraggingCardPosition.UnderThreshold:
                    break;

                case DraggingCardPosition.OverThreshold:
                    break;

                case DraggingCardPosition.FinishedUnderThreshold:
                    return;

                case DraggingCardPosition.FinishedOverThreshold:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnClearItemsCommand()
        {
            Numbers.Clear();
        }

        private void OnAddItemsCommand()
        {
        }

        private void InitializeProfiles()
        {
            int max = 2;
            Random r = new Random();
            for (int i = 1; i < 100; i++)
            {

                var v = r.Next(max + 1, (max + 1) + i * 3);
                if (v % 2 == 0)
                    v++;
                if (v % 5 == 0 && v != 5)
                    v += 2;
                max = v;
                Numbers.Add(new Number { Id = i, Value = v.ToString(), Prime = IsPrime(v) });

            }


            //Profiles.Add(new Profile { ProfileId = 11, Name = "Steve", Age = 31, Gender = Gender.Male, Photo = "p327144.jpg" });
        }

        private void addProfiles(List<Number> numbers)
        {
            foreach (var n in numbers)
                n.Factors = LogFactorList(PrimeFactors(Convert.ToInt32(n.Value)));
            Numbers = new ObservableCollection<Number>(numbers);
        }

        private List<int> PrimeFactors(int a)
        {
            List<int> retval = new List<int>();
            for (int b = 2; a > 1; b++)
            {
                while (a % b == 0)
                {
                    a /= b;
                    retval.Add(b);
                }
            }
            return retval;
        }

        private string LogFactorList(List<int> factors)
        {
            if (factors.Count == 1)
            {
                return ("Prime");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < factors.Count; ++i)
                {
                    if (i > 0)
                    {
                        sb.Append('x');
                    }
                    sb.AppendFormat("{0}", factors[i]);
                }
                return (sb.ToString());
            }
        }


        public bool IsPrime(int number)
        {
            if (number <= 1)
                return false;
            if (number == 2)
                return true;
            if (number % 2 == 0)
                return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
    }
}
