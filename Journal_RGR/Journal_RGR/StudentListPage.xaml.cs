using Journal_RGR.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentListPage : ContentPage
    {
        public StudentListPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<StudentEditorPage>(this, "StudentUpdate", ReloadPage);
        }

        private void ReloadPage(Object obj)
        {
            App.Page.Detail = new NavigationPage(this);
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            App.Navigate(new StudentEditorPage());
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var list = App.DB.Students.OrderBy(s => s.Name).ToList();
            StudentListView.ItemsSource = null;
            StudentListView.ItemsSource = list;
        }
    }
}
