// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmSample.Core.ViewModels
{
    public class ObservableObjectPageViewModel : SamplePageViewModel
    {
        public ObservableObjectPageViewModel()
        {
            ReloadTaskCommand = new RelayCommand(ReloadTask);
        }

        /// <summary>
        /// Gets the <see cref="ICommand"/> responsible for setting <see cref="MyTask"/>.
        /// </summary>
        public ICommand ReloadTaskCommand { get; }

        private string? name;

        /// <summary>
        /// Gets or sets the name to display.
        /// </summary>
        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private TaskNotifier? myTask;

        /// <summary>
        /// Gets or sets the name to display.
        /// </summary>
        public Task? MyTask
        {
            get => myTask;
            private set => SetPropertyAndNotifyOnCompletion(ref myTask, value);
        }

        /// <summary>
        /// Simulates an asynchronous method.
        /// </summary>
        public void ReloadTask()
        {
            MyTask = Task.Delay(3000);
        }
    }
}
