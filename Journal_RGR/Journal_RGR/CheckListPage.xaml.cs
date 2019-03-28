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

        public CheckListPage(Discipline discipline) : this()
        {
            _discipline = discipline;
        }

        public CheckListPage(DateTime dateTime, Discipline discipline) : this()
        {
            _dateTime = dateTime;
            _discipline = discipline;
            update_flag = true;
            Title = "Редактировать запись";
        }

        private async void UpdateSemesterPicker()
        {
            var semesters = new List<string>();
            var disciplines = App.DB.Disciplines.ToList();
            if (disciplines.Count == 0)
            {
                var result = await DisplayAlert("Предупреждение", "В базе не найдены дисциплины, перейти к созданию?", "Да", "Нет");
                if (result) App.Page.Detail = new DisciplineListPage();
                return;
            }
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
            foreach(StudentCheckView item in JournalStackLayout.Children)
            {
                var student = App.DB.Students.First(s => s.Id == item.Index);
                if (!update_flag)
                {
                    var discipline = (DisciplinePicker.SelectedItem as Discipline);
                    if(discipline == null)
                    {
                        await DisplayAlert("Ошибка!", "Выберите дисциплину!", "ОК");
                        return;
                    }
                    var datetime = DatePicker.Date;
                    var count = App.DB.Journals.Where(j => j.DisciplineId == discipline.Id).Count(j => (datetime >= j.DateTime));
                    if (count > 0)
                    {
                        var max_date = App.DB.Journals.Where(j => j.DisciplineId == discipline.Id).Max(j => j.DateTime);
                        datetime = max_date.AddMilliseconds(1);
                    }
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
                else
                {
                    var journal = App.DB.Journals.SingleOrDefault(j => j.StudentId == student.Id && j.Discipline == _discipline && j.DateTime == _dateTime);
                    if (journal == null)
                    {
                        journal = new Journal
                        {
                            StudentId = student.Id,
                            Student = student,
                            DateTime = _dateTime,
                            DisciplineId = _discipline.Id,
                            Discipline = _discipline,
                            Check = item.IsChecked,
                        };
                        App.DB.Journals.Add(journal);
                    }
                    else
                    {
                        journal.Check = item.IsChecked;
                    }
                }
            }
            App.DB.SaveChanges();
            if (update_flag) App.Back();
            else await DisplayAlert("Успех!", "Запись успешно сохранена", "ОК");
          
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var students = App.DB.Students.OrderBy(s => s.Name).ToList();
            if(students.Count == 0)
            {
                var result = await DisplayAlert("Предупреждение", "В базе не найдены студенты, перейти к созданию?", "Да","Нет");
                if (result) App.Page.Detail = new StudentListPage();
            }
            JournalStackLayout.Children.Clear();
            foreach (var item in students)
            {
                JournalStackLayout.Children.Add(new StudentCheckView
                {
                    Index = item.Id,
                    Name = item.Name
                });
            }
            UpdateSemesterPicker();
            if(_discipline != null)
            {
                SemesterPicker.SelectedItem = _discipline.Semester.ToString();
                DisciplinePicker.SelectedItem = _discipline;
            }
            if(update_flag)
            {
                DeleteButton.IsVisible = true;
                SemesterPicker.IsEnabled = false;
                DisciplinePicker.IsEnabled = false;
                DatePicker.IsEnabled = false;
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

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Удаление", "Вы уверены что хотите удалить запись?", "Да", "Нет");
            if (result)
            {
                var journal = App.DB.Journals.Where(r => r.DisciplineId == _discipline.Id && r.DateTime == _dateTime).ToList();
                App.DB.Journals.RemoveRange(journal);
                App.DB.SaveChanges();
                App.Back();
            }
        }
    }
}
