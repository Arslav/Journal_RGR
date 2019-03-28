using System;
using System.Linq;
using Xamarin.Forms;

namespace Journal_RGR.ViewCells
{
    public class DisciplineViewCell : ViewCell
    {
        private readonly Label _nameLabel;

        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(DisciplineViewCell), "");
        public static readonly BindableProperty IndexProperty = BindableProperty.Create("Index", typeof(int), typeof(DisciplineViewCell), 0);

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

        public DisciplineViewCell()
        {
            _nameLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Center
            };
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition(),
                    new ColumnDefinition { Width = new GridLength(30,GridUnitType.Absolute) }
                }
            };
            var image = new Image {
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
            var discipline = App.DB.Disciplines.Where(s => s.Id == Index).First();
            App.Navigate(new DisciplineEditorPage(discipline));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                _nameLabel.Text = Name;
            }
        }
    }
}
