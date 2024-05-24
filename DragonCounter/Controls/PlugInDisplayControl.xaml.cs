using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragonCounter.Controls
{
    public partial class PlugInDisplayControl : UserControl, INotifyPropertyChanged
    {
        private readonly DragonCounter _dragonCounter;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlugInDisplayControl(DragonCounter dragonCounter)
        {
            InitializeComponent();
            _dragonCounter = dragonCounter;
            _dragonCounter.PropertyChanged += DragonCounter_PropertyChanged;
            DataContext = _dragonCounter; // Ensure DataContext is set correctly

            // Bind DragonsPlayed property to TextBlock and set its initial color
            DragonsPlayedTextBlock.Text = _dragonCounter.DragonsPlayed.ToString();
            SetTextColor(_dragonCounter.DragonsPlayed);
        }

        private void DragonCounter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DragonCounter.DragonsPlayed))
            {
                OnPropertyChanged(nameof(DragonsPlayed));
                DragonsPlayedTextBlock.Text = _dragonCounter.DragonsPlayed.ToString();
                SetTextColor(_dragonCounter.DragonsPlayed);
            }
        }

        public int DragonsPlayed => _dragonCounter.DragonsPlayed;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetTextColor(int dragonsPlayed)
        {
            if (dragonsPlayed > 7)
            {
                DragonsPlayedTextBlock.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else
            {
                DragonsPlayedTextBlock.Foreground = new SolidColorBrush(Colors.Firebrick);
            }
        }
    }
}
