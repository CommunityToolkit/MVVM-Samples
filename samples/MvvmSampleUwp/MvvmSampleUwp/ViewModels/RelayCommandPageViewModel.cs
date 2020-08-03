using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;

namespace MvvmSampleUwp.ViewModels
{
    public class RelayCommandPageViewModel : SamplePageViewModel
    {
        public RelayCommandPageViewModel()
        {
            IncrementCounterCommand = new RelayCommand(IncrementCounter);
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> responsible for incrementing <see cref="Counter"/>.
        /// </summary>
        public ICommand IncrementCounterCommand { get; }

        private int counter;

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public int Counter
        {
            get => counter;
            private set => SetProperty(ref counter, value);
        }

        /// <summary>
        /// Increments <see cref="Counter"/>.
        /// </summary>
        private void IncrementCounter() => Counter++;

    }
}
