﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage, IMainPageView
    {
        public MainPage()
        {
            InitializeComponent();
            
            ViewModel.Initialize(this);
        }

        public void ShowMessage(string message, string caption)
        {
            DisplayAlert(caption, message, "OK");
        }

        public async void ShowCurrentBudgetsPage()
        {
            var page = new NavigationPage(new BudgetsPage());
            await Navigation.PushAsync(page);
        }
    }
}