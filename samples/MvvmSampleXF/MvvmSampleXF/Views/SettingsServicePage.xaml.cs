﻿using MvvmSample.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmSampleXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsServicePage : ContentPage
    {
        public SettingsServicePage()
        {
            InitializeComponent();

            ViewModel = Ioc.Default.GetRequiredService<SettingsServicePageViewModel>();

            BindingContext = ViewModel;
        }

        public SettingsServicePageViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.LoadDocsCommand.Execute("PuttingThingsTogether");
        }
    }
}