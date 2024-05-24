using DragonCounter.Controls;
using DragonCounter.Properties;
using Hearthstone_Deck_Tracker.API;
using System;
using System.Reflection;
using System.Windows.Controls;
using Core = Hearthstone_Deck_Tracker.API.Core;

namespace DragonCounter
{
    public class HDTBootstrap : Hearthstone_Deck_Tracker.Plugins.IPlugin, IDisposable
    {
        private DragonCounter pluginInstance;

        public string Author => "SneakyTurtle";
        public string ButtonText => LocalizeTools.GetLocalized("LabelSettings");
        public MenuItem MenuItem { get; set; } = null;
        public string Name => LocalizeTools.GetLocalized("TextName");
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string Description => LocalizeTools.GetLocalized("TextDescription");

        public void OnButtonPress()
        {
            SettingsView.Flyout.IsOpen = true;
        }

        public void OnLoad()
        {
            // Subscribe to the OnGameStart event
            GameEvents.OnGameStart.Add(OnGameStart);

            // Initialize the plugin instance and add the menu item
            pluginInstance = new DragonCounter();
            AddMenuItem();
        }

        private void OnGameStart()
        {
            // Ensure the panel is added to the overlay canvas when the game starts
            Core.OverlayCanvas.Children.Add(new PlugInDisplayControl(pluginInstance));
        }

        private void AddMenuItem()
        {
            this.MenuItem = new MenuItem()
            {
                Header = Name
            };

            this.MenuItem.Click += (sender, args) =>
            {
                OnButtonPress();
            };
        }

        public void OnUnload()
        {
            Settings.Default.Save();
            pluginInstance?.CleanUp();
            pluginInstance = null;
        }

        public void OnUpdate() { }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            pluginInstance?.Dispose();
        }

    protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                pluginInstance?.Dispose();
            }
            // Dispose unmanaged resources
        }
    }
}
