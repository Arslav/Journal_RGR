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
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(30, GridUnitType.Absolute) }
                }
            };
            var image = new Image
            {
                Source = ImageSource.FromFile("PencilIcon"),
                Margin = new Thickness(5)
            };
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += TapGesture_Tapped;
            image.GestureRecognizers.Add(tapGesture);
            grid.Children.Add(_nameLabel, 0, 0);
            grid.Children.Add(image, 1, 0);
            View = grid;
        }

        private void TapGesture_Tapped(object sender, EventArgs e)
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
