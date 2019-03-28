using Journal_RGR.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisciplineListPage : ContentPage
    {
        public DisciplineListPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<DisciplineEditorPage>(this, "DiscUpdate", ReloadPage);
            UpdateSemesterPicker();
        }

        private void UpdateSemesterPicker()
        {
            var semesters = new List<string>(); 
            var disciplines = App.DB.Disciplines.ToList();
            foreach (var item in disciplines)
            {
                var add_flag = true;
                for (var i = 0; i < semesters.Count; i++)
                {
                    if (semesters[i] == item.Semester.ToString())
                    {
                        add_flag = false;
                        break;
                    }
                }
                if (add_flag) semesters.Add(item.Semester.ToString());
            }
            semesters.Sort();
            semesters.Insert(0, "Все");
            SemesterPicker.ItemsSource = semesters;
            SemesterPicker.SelectedIndex = 0;
            if (App.Current.Properties.TryGetValue("last_semester", out object semester_obj))
            {
                var semester_string = (string)semester_obj;
                if (semesters.Contains(semester_string))
                {
                    var index = semesters.IndexOf(semester_string);
                    SemesterPicker.SelectedIndex = index;
                }
            }
        }

        private void ReloadPage(Object obj)
        {
            App.Page.Detail = new NavigationPage(this);
            UpdateSemesterPicker();
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            List<Discipline> list = null;
            App.Current.Properties["last_semester"] = SemesterPicker.SelectedItem;
            if (SemesterPicker.SelectedIndex != 0)
            {
                var semester = Convert.ToInt32(SemesterPicker.SelectedItem);
                list = App.DB.Disciplines.Where(d => d.Semester == semester).ToList();
            }
            else list = App.DB.Disciplines.ToList();
            DisciplineListView.ItemsSource = list;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            App.Navigate(new DisciplineEditorPage());
        }

        private void DisciplineListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var discipline = e.Item as Discipline;
            App.Navigate(new DisciplineCheckPage(discipline));
            DisciplineListView.SelectedItem = null;
        }
    }
}
