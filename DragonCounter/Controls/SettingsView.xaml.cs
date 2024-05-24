using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DragonCounter.Controls
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ScrollViewer
    {
        private static Flyout _flyout;

        // ToDo: The window shouldn't be statically named
        private static readonly string panelName = "pluginStackPanelView";
        private StackPanel stackPanel;

        public SettingsView()
        {
            InitializeComponent();
            this.GetPanel();
            InitTranslation();
        }

        public static Flyout Flyout
        {
            get
            {
                if (_flyout == null)
                {
                    _flyout = CreateSettingsFlyout();
                }
                return _flyout;
            }
        }

        public IEnumerable<Orientation> OrientationTypes => Enum.GetValues(typeof(Orientation)).Cast<Orientation>();

        private static Flyout CreateSettingsFlyout()
        {
            var settings = new Flyout
            {
                Position = Position.Left
            };
            Panel.SetZIndex(settings, 100);
            settings.Header = LocalizeTools.GetLocalized("LabelSettings");
            settings.Content = new SettingsView();
            Core.MainWindow.Flyouts.Items.Add(settings);
            return settings;
        }

        /// <summary>
        /// Gets the reference to our display StackPanel.
        /// </summary>
        private void GetPanel()
        {
            this.stackPanel = Core.OverlayCanvas.FindChild<StackPanel>(panelName);
        }
        /// <summary>
        /// Does our default translation, just till I fix the XAML Hooks.
        /// </summary>
        public void InitTranslation()
        {
            LblOpacity.Content = LocalizeTools.GetLocalized("LabelOpacity");
            LblScale.Content = LocalizeTools.GetLocalized("LabelScale");
        }
    }
}