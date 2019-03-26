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
        private Discipline _discipline;
        public DisciplineCheckPage(Discipline discipline)
        {
            InitializeComponent();
            _discipline = discipline;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            TeacherLabel.Text = _discipline.Teacher;
            TypeLabel.Text = _discipline.Type.ToString(); //TODO: выход значений
            SemesterLabel.Text = _discipline.Semester.ToString();
            Title = _discipline.Name;

            var students = App.DB.Students.ToList();
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

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            for (var i = 0; i < students.Count; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.Children.Add(new Label
                {
                    Text = students[i].Name,
                    //HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                }, 0, i + 1);
            }

            //Log.Warning("Dates count", dates.Count.ToString());
            for (var i = 0; i < dates.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                grid.Children.Add(new JournalDateTimeLabel(dates[i],_discipline), i + 1, 0);

                for(var j = 0; j < students.Count; j++)
                {
                    var check = false;
                    var record = records.Where(r => r.StudentId == students[j].Id && r.DisciplineId == _discipline.Id && r.DateTime == dates[i]).First();
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
    }
}