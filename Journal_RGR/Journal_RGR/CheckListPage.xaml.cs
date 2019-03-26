using Journal_RGR.Models;
using Journal_RGR.ViewCells;
using Journal_RGR.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckListPage : ContentPage
    {
        private DateTime _dateTime;
        private Discipline _discipline;
        private bool update_flag = false;

        public CheckListPage()
        {
            InitializeComponent();
        }

        public CheckListPage(DateTime dateTime, Discipline discipline) : this()
        {
            _dateTime = dateTime;
            _discipline = discipline;
            update_flag = true;
            Title = "Редактировать запись";
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
        }

        private void SemesterPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Discipline> list = null;
            if (SemesterPicker.SelectedIndex != 0)
            {
                var semester = Convert.ToInt32(SemesterPicker.SelectedItem);
                list = App.DB.Disciplines.Where(d => d.Semester == semester).ToList();
            }
            else list = App.DB.Disciplines.ToList();
            DisciplinePicker.ItemsSource = list;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            //TODO: сделать валидацию
            var discipline = (DisciplinePicker.SelectedItem as Discipline);
            var datetime = DatePicker.Date;
            var count = App.DB.Journals.Where(j => j.DisciplineId == discipline.Id).Count(j => (datetime >= j.DateTime));
            if (count > 0)
            {
                var max_date = App.DB.Journals.Where(j => j.DisciplineId == discipline.Id).Max(j => j.DateTime);
                datetime = max_date.AddMilliseconds(1);
            }

            foreach(StudentCheckView item in JournalStackLayout.Children)
            {
                //TODO: выбирать студента, а не искать по индексу
                var student = App.DB.Students.First(s => s.Id == item.Index);
                var journal = new Journal
                {
                    StudentId = student.Id,
                    Student = student,
                    DateTime = datetime,
                    DisciplineId = discipline.Id,
                    Discipline = discipline,
                    Check = item.IsChecked,
                };
                App.DB.Journals.Add(journal);
            }
            App.DB.SaveChanges();
            await DisplayAlert("Успех!", "Запись успешно сохранена", "ОК");
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var students = App.DB.Students.ToList();
            JournalStackLayout.Children.Clear();
            foreach (var item in students)
            {
                JournalStackLayout.Children.Add(new StudentCheckView
                {
                    Index = item.Id, //TODO: хранить студента, а не его индекс
                    Name = item.Name
                });
            }
            UpdateSemesterPicker();
            if(update_flag)
            {
                
                DeleteButton.IsVisible = true;
                SemesterPicker.IsEnabled = false;
                DisciplinePicker.IsEnabled = false;
                DatePicker.IsEnabled = false;
                SemesterPicker.SelectedItem = _discipline.Semester.ToString();
                DisciplinePicker.SelectedItem = _discipline;
                DatePicker.Date = _dateTime;

                var journal = App.DB.Journals.Where(r => r.DisciplineId == _discipline.Id && r.DateTime == _dateTime).ToList();
                foreach (StudentCheckView item in JournalStackLayout.Children)
                {
                    foreach (var record in journal)
                    {
                        if(record.StudentId == item.Index)
                        {
                            item.IsChecked = record.Check;
                        }
                    }
                }
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
