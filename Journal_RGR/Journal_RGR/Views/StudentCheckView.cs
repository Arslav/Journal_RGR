using Journal_RGR.Models;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Journal_RGR.Views
{
    class StudentCheckView : ContentView
    {
        private readonly Label _nameLabel;
        private readonly CheckBox _checkBox;

        public int Index { get; set; }
        public string Name
        {
            get => _nameLabel.Text;
            set => _nameLabel.Text = value;
        }
        public bool IsChecked
        {
            get => _checkBox.IsChecked;
            set => _checkBox.IsChecked = value;
        }

        public StudentCheckView()
        {
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition(),
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    }
                }
            };
            _nameLabel = new Label
            {
                VerticalTextAlignment = TextAlignment.Center
            };
            _checkBox = new CheckBox();
            grid.Children.Add(_nameLabel, 0, 0);
            grid.Children.Add(_checkBox, 1, 0);
            Content = grid;
        }

    }
}
