using Dragoncounter.Logic;
using DragonCounter.Controls;
using DragonCounter.Logic;
using DragonCounter.Properties;
using HearthDb.Enums;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Stats;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace DragonCounter
{
    public partial class DragonCounter : IDisposable, INotifyPropertyChanged
    {
        private int _dragonsPlayedCount;
        private const string PanelName = "pluginStackPanelView";
        private readonly DragonList _dragonList;
        public static InputMoveManager InputMoveManager;
        private PlugInDisplayControl stackPanel;

        public int DragonsPlayed
        {
            get => _dragonsPlayedCount;
            private set
            {
                _dragonsPlayedCount = value;
                OnPropertyChanged(nameof(DragonsPlayed));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DragonCounter()
        {
            _dragonList = new DragonList(); // Initialize _dragonList

            InitViewPanel();

            // Initialize InputMoveManager after stackPanel is created
            InputMoveManager = new InputMoveManager(stackPanel);

            GameEvents.OnGameStart.Add(GameTypeCheck);
            GameEvents.OnPlayerPlay.Add(OnPlayerPlay);
            GameEvents.OnGameEnd.Add(OnGameEnd); // Subscribe to the GameEnd event
            GameEvents.OnPlayerPlayToGraveyard.Add(OnPlayerPlayToGraveyard); // Subscribe to OnPlayerPlayToGraveyard event
            GameEvents.OnInMenu.Add(OnInMenu); // Subscribe to OnInMenu event to handle game state changes
        }

        private void OnPlayerPlay(Card card)
        {
            // Check if the played card is a dragon
            if (_dragonList.IsDragon(card.DbfId))
            {
                DragonsPlayed++;
            }
        }

        private void OnPlayerPlayToGraveyard(Card card)
        {
            // Check if the game is running and if the card is one of the desired cards
            if (Core.Game.IsRunning)
            {
                // Check if the card is one of the desired cards and increment the count accordingly
                if (card.DbfId == 103491)
                {
                    DragonsPlayed++;
                }
                else if (card.DbfId == 107774)
                {
                    DragonsPlayed++;
                }
            }
        }

        private void GameTypeCheck()
        {
            if (Core.Game.CurrentGameType == GameType.GT_RANKED ||
                Core.Game.CurrentGameType == GameType.GT_CASUAL ||
                Core.Game.CurrentGameType == GameType.GT_FSG_BRAWL ||
                Core.Game.CurrentGameType == GameType.GT_ARENA)
            {
                ShowStackPanel();
            }
            else
            {
                HideStackPanel();
            }
        }

        private void OnInMenu()
        {
            if (Core.Game.IsRunning)
            {
                GameTypeCheck(); // Ensure the panel is shown if a game is running
            }
        }

        private void OnGameEnd()
        {
            HideStackPanel();
            ResetCount();
        }

        private void InitViewPanel()
        {
            stackPanel = new PlugInDisplayControl(this) { Name = PanelName, Visibility = System.Windows.Visibility.Visible };
            Core.OverlayCanvas.Children.Add(stackPanel);

            Canvas.SetTop(stackPanel, Settings.Default.Top);
            Canvas.SetLeft(stackPanel, Settings.Default.Left);

            Settings.Default.PropertyChanged += SettingsChanged;
            SettingsChanged(null, null);
        }

        private void ShowStackPanel()
        {
            if (stackPanel == null)
            {
                InitViewPanel();
                // Re-initialize InputMoveManager with the new stackPanel
                InputMoveManager = new InputMoveManager(stackPanel);
            }

            // Show the stack panel
            stackPanel.Visibility = System.Windows.Visibility.Visible;

            // Add the stack panel to the overlay canvas if not already added
            if (!Core.OverlayCanvas.Children.Contains(stackPanel))
            {
                Core.OverlayCanvas.Children.Add(stackPanel);
            }
        }

        private void HideStackPanel()
        {
            if (stackPanel != null)
            {
                stackPanel.Visibility = System.Windows.Visibility.Collapsed;
                Core.OverlayCanvas.Children.Remove(stackPanel);
            }
        }

        private void ResetCount()
        {
            DragonsPlayed = 0;
        }

        private void SettingsChanged(object sender, PropertyChangedEventArgs e)
        {
            stackPanel.RenderTransform = new ScaleTransform(Settings.Default.Scale / 100, Settings.Default.Scale / 100);
            stackPanel.Opacity = Settings.Default.Opacity / 100;
        }

        public void CleanUp()
        {
            HideStackPanel();
            InputMoveManager?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                InputMoveManager?.Dispose();
            }
            // Dispose unmanaged resources
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
