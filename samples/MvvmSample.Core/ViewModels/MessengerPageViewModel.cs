// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MvvmSample.Core.Services;

namespace MvvmSample.Core.ViewModels;

public class MessengerPageViewModel : SamplePageViewModel
{
    public MessengerPageViewModel(IFilesService filesService) : base(filesService)
    {
        RequestCurrentUsernameCommand = new RelayCommand(RequestCurrentUsername);
        ResetCurrentUsernameCommand = new RelayCommand(ResetCurrentUsername);
    }

    public ICommand RequestCurrentUsernameCommand { get; }
    public ICommand ResetCurrentUsernameCommand { get; }

    public UserSenderViewModel SenderViewModel { get; } = new();

    public UserReceiverViewModel ReceiverViewModel { get; } = new();

    // Simple viewmodel for a module sending a username message
    public class UserSenderViewModel : ObservableRecipient
    {
        public UserSenderViewModel()
        {
            SendUserMessageCommand = new RelayCommand(SendUserMessage);
        }

        public ICommand SendUserMessageCommand { get; }

        private string username = "Bob";

        public string Username
        {
            get => username;
            private set => SetProperty(ref username, value);
        }

        protected override void OnActivated()
        {
            Messenger.Register<UserSenderViewModel, CurrentUsernameRequestMessage>(this, (r, m) => m.Reply(r.Username));
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
        private string username = "";

        public string Username
        {
            get => username;
            private set => SetProperty(ref username, value);
        }

        protected override void OnActivated()
        {
            Messenger.Register<UserReceiverViewModel, UsernameChangedMessage>(this, (r, m) => r.Username = m.Value);
        }
    }

    private string? username;

    public string? Username
    {
        get => username;
        private set => SetProperty(ref username, value);
    }

    public void RequestCurrentUsername()
    {
        Username = WeakReferenceMessenger.Default.Send<CurrentUsernameRequestMessage>();
    }

    public void ResetCurrentUsername()
    {
        Username = null;
    }

    // A sample message with a username value
    public sealed class UsernameChangedMessage : ValueChangedMessage<string>
    {
        public UsernameChangedMessage(string value) : base(value)
        {
        }
    }

    // A sample request message to get the current username
    public sealed class CurrentUsernameRequestMessage : RequestMessage<string>
    {
    }
}
