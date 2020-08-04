using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace MvvmSampleUwp.ViewModels
{
    public class MessengerPageViewModel : SamplePageViewModel
    {
        public UserSenderViewModel SenderViewModel { get; } = new UserSenderViewModel();

        public UserReceiverViewModel ReceiverViewModel { get; } = new UserReceiverViewModel();

        // Simple viewmodel for a module sending a username message
        public class UserSenderViewModel : ObservableRecipient
        {
            private string username = "Bob";

            public string Username
            {
                get => username;
                private set => SetProperty(ref username, value);
            }

            public void SendUserMessage()
            {
                Username = Username == "Bob" ? "Alice" : "Bob";

                Messenger.Send(new UsernameChangedMessage(Username));
            }
        }

        // Simple viewmodel for a module receiving a username message
        public class UserReceiverViewModel : ObservableRecipient
        {
            private string username = "Bob";

            public string Username
            {
                get => username;
                private set => SetProperty(ref username, value);
            }

            protected override void OnActivated()
            {
                Messenger.Register<UsernameChangedMessage>(this, m => Username = m.Value);
            }
        }

        // A sample message with a username value
        public sealed class UsernameChangedMessage : ValueChangedMessage<string>
        {
            public UsernameChangedMessage(string value) : base(value)
            {
            }
        }
    }
}
