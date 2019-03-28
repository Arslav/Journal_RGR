using Journal_RGR.Models;
using Journal_RGR.Views;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Journal_RGR
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisciplineCheckPage : ContentPage
    {
        private List<string> _typeList = new List<string>()
        {
            "Зачет",
            "Диф. зачет",
            "Экзамен"
        };

        private Discipline _discipline;
        public DisciplineCheckPage(Discipline discipline)
        {
            InitializeComponent();
            _discipline = discipline;
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            TeacherLabel.Text = _discipline.Teacher;
            TypeLabel.Text = _typeList[_discipline.Type];
            SemesterLabel.Text = _discipline.Semester.ToString();
            Title = _discipline.Name;

            var students = App.DB.Students.OrderBy(s => s.Name).ToList();
            if (students.Count == 0)
            {
                var result = await DisplayAlert("Предупреждение", "В базе не найдены студенты, перейти к созданию?", "Да", "Нет");
                if (result) App.Page.Detail = new StudentListPage();
            }
            var records = App.DB.Journals.Where(j => j.DisciplineId == _discipline.Id).ToList();

            var dates = new List<DateTime>();
            foreach (var item in records)
            {
                var add_flag = true;
                foreach (var date in dates)
                {
                    if (date == item.DateTime)
                    {
                        add_flag = false;
                        break;
                    }
                }
                if (add_flag) dates.Add(item.DateTime);
            }
            dates.Sort();

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            for (var i = 0; i < students.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Children.Add(new Label
                {
                    Text = students[i].Name,
                    VerticalTextAlignment = TextAlignment.Center,
                }, 0, i + 1);
            }

            for (var i = 0; i < dates.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.Children.Add(new JournalDateTimeLabel(dates[i],_discipline), i + 1, 0);

                for(var j = 0; j < students.Count; j++)
                {
                    var check = false;
                    var record = records.SingleOrDefault(r => r.StudentId == students[j].Id && r.DisciplineId == _discipline.Id && r.DateTime == dates[i]);
                    if (record != null) check = record.Check;
                    grid.Children.Add(new CheckBox
                    {
                        IsChecked = check,
                        IsDisabled = true,
                        HorizontalOptions = LayoutOptions.Center
                    }, i + 1, j + 1);
                }
            }
            TableScrollView.Content = grid;
        }

        private void AddButton_Clicked(object sender, EventArgs e)
        {
            App.Navigate(new CheckListPage(_discipline));
        }
    }
}