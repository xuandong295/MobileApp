using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM_LoginPage.Services
{
    interface ILoginService
    {
        bool login(string userName, string password);
        void addData(string username, string password);
        void LoadData();
        bool cmpPassRepass(string password, string rePassword);
        bool checkDuplicateUsername(string username);
        string checkUsernameForPassword(string username);
    }
}
