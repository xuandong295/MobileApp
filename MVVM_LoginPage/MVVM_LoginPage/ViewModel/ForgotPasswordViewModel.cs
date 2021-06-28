using MVVM_LoginPage.Services;
using MVVM_LoginPage.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MVVM_LoginPage.ViewModel
{
    class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ILoginService ilog = DependencyService.Get<ILoginService>();
        public Command cmdExit { get; set; }
        public Command cmdConfirm { get; set; }
        public ForgotPasswordViewModel()
        {
            cmdExit = new Command(gotoLogin);
            cmdConfirm = new Command(showPassword);
        }

        private void showPassword(object obj)
        {
            for_Password = "Your password is: " + ilog.checkUsernameForPassword(for_Username);
        }

        private void gotoLogin(object obj)
        {
            App.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
        private string for_username;
        public string for_Username
        {
            get { return for_username; }
            set
            {
                for_username = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("for_Username"));
            }
        }
        private string for_password;
        public string for_Password
        {
            get { return for_password; }
            set
            {
                for_password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("for_Password"));
            }
        }
    }
}
