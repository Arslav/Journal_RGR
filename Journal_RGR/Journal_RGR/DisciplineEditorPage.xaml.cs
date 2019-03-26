using Journal_RGR.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisciplineEditorPage : ContentPage
    {
        private Discipline _discipline;

        public DisciplineEditorPage(Discipline discipline = null)
        {
            InitializeComponent();
            _discipline = discipline;
            if (discipline != null)
            {
                NameEntry.Text = discipline.Name;
                TeacherEntry.Text = discipline.Teacher;
                TypePicker.SelectedIndex = discipline.Type;
                SemestyEntry.Text = discipline.Semester.ToString();
            }
            else DeleteButton.IsVisible = false;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            Discipline discipline = _discipline;
            if (_discipline == null) discipline = new Discipline();
            discipline.Name = NameEntry.Text;
            discipline.Teacher = TeacherEntry.Text;
            discipline.Type = TypePicker.SelectedIndex;
            if (!Int32.TryParse(SemestyEntry.Text, out int semester))
            {
                await DisplayAlert("Ошибка", "Семестр должен быть числом!", "OK");
                return;
            }
            else discipline.Semester = semester;
            if (_discipline == null) App.DB.Disciplines.Add(discipline);
            App.DB.SaveChanges();
            MessagingCenter.Send(this, "DiscUpdate");
            App.Back();
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            App.DB.Disciplines.Remove(_discipline);
            App.DB.SaveChanges();
            MessagingCenter.Send(this, "DiscUpdate");
            App.Back();
        }
    }
}