using MVVM_LoginPage.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MVVM_LoginPage.Services
{
    class DataHandler
    {
        private SQLiteConnection database;

        private static object collisionLock = new object();

        public ObservableCollection<UserModel> UserModels { get; set; }
        
        public DataHandler()
        {
            
            database =
                 DependencyService.Get<IDatabaseConnection>().DbConnection();
            database.CreateTable<UserModel>();

            this.UserModels = new ObservableCollection<UserModel>(database.Table<UserModel>());
            if (!database.Table<UserModel>().Any())
            {
                database.Insert(new UserModel { userName = "admin", password = "admin" });
                database.Insert(new UserModel { userName = "xuandong", password = "xuandong" });
            }

        }
        public List<UserModel> GetAllAccountToList()
        {
            //Get all Accounts.
            return database.Table<UserModel>().ToList();
        }
        public void AddAccount(UserModel p)
        {
                database.Insert(p);
        }
    }
}
