using Journal_RGR.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    public partial class App : Application
    {
        public static MasterDetailPage Page { get; set; }
        public static ApplicationContext DB { get; set; }

        public App()
        {
            InitializeComponent();
            if (!DesignMode.IsDesignModeEnabled)
            {
                var dbName = "journal.db";
                var directory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var fullPath = Path.Combine(directory, dbName);
                DB = new ApplicationContext(fullPath);
                DB.Database.Migrate();
            }
            Page = new Main();
            MainPage = Page;
        }

        public static void Navigate(Page page)
        {
            Page.Detail.Navigation.PushAsync(page);
        }

        public static void Back()
        {
            Page.Detail.Navigation.PopAsync();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
