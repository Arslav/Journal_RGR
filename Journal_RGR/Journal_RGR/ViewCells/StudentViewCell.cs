using Journal_RGR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Journal_RGR.ViewCells
{
    public class StudentViewCell : ViewCell
    {
        private readonly Label _indexLabel;
        private readonly Label _nameLabel;

        public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(StudentViewCell), 0);
        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(StudentViewCell), "");

        public int Index
        {
            get => (int)GetValue(IndexProperty);
            set => SetValue(IndexProperty, value);
        }

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public StudentViewCell()
        {
            _indexLabel = new Label {
                VerticalTextAlignment = TextAlignment.Center
            };
            _nameLabel = new Label {
                VerticalTextAlignment = TextAlignment.Center
            };
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) }
                }
            };
            var _editButton = new Button { Text = "E" };
            grid.Children.Add(_indexLabel, 0, 0);
            grid.Children.Add(_nameLabel, 1, 0);
            grid.Children.Add(_editButton, 2, 0);
            _editButton.Clicked += EditButton_Clicked;
            View = grid;

        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            var student = App.DB.Students.Where(s => s.Id == Index).First();
            App.Navigate(new StudentEditorPage(student));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                _indexLabel.Text = $"{Index}.";
                _nameLabel.Text = Name;
            }
        }
    }
}
