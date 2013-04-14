using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreAudioApi;
using System.Diagnostics;
using System.Timers;

namespace VolumeVisualizer.ViewModels
{
    public class MainViewModel : ViewModel
    {
        MMDeviceEnumerator deviceEnumerator;
        MMDevice device;

        private Timer visibilityTimer;

        private uint volumeLevel;
        private int percentBarWidth;

        private uint windowWidth = 400;
        private uint windowHeight = 50;

        #region Window settings

        /// <summary>
        /// Gets or sets the Window width
        /// </summary>
        public uint WindowWidth
        {
            get { return windowWidth; }
            private set
            {
                windowWidth = value;
                NotifyPropertyChanged("WindowWidth");
            }
        }

        /// <summary>
        /// Gets or sets the Window height
        /// </summary>
        public uint WindowHeight
        {
            get { return windowHeight; }
            private set
            {
                windowHeight = value;
                NotifyPropertyChanged("WindowHeight");
            }
        }

        #endregion

        #region Volume Visualization

        /// <summary>
        /// Gets the current Master volume (0 ... 100)
        /// </summary>
        public uint VolumeLevel
        {
            get { return volumeLevel; }
            private set
            {
                volumeLevel = value;
                NotifyPropertyChanged("VolumeLevel");
            }
        }

        /// <summary>
        /// Gets the current width of the bar representing the volume
        /// </summary>
        public int PercentBarWidth
        {
            get { return percentBarWidth; }
            private set
            {
                percentBarWidth = value;
                NotifyPropertyChanged("PercentBarWidth");
            }
        }

        #endregion

        public MainViewModel()
            : base()
        {
            visibilityTimer = new Timer(2000);
            visibilityTimer.Elapsed += visibilityTimerElapsed;
            visibilityTimer.AutoReset = false;

            deviceEnumerator = new MMDeviceEnumerator();
            device = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            device.AudioEndpointVolume.OnVolumeNotification += VolumeChanged;
        }

        private void visibilityTimerElapsed(object sender, ElapsedEventArgs e)
        {
            HideWindow();
        }

        /// <summary>
        /// Show the window
        /// </summary>
        private void ShowWindow()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                App.Current.MainWindow.Visibility = System.Windows.Visibility.Visible;
            }));
        }

        /// <summary>
        /// Hide the window
        /// </summary>
        private void HideWindow()
        {
            App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                App.Current.MainWindow.Visibility = System.Windows.Visibility.Hidden;
            }));
        }

        //data.MasterVolume = 0 ... 1 (float)
        private void VolumeChanged(AudioVolumeNotificationData data)
        {
            ShowWindow();
            visibilityTimer.Stop();
            VolumeLevel = (uint)(data.MasterVolume * 100);
            PercentBarWidth = (int)(data.MasterVolume * 360);
            visibilityTimer.Start();
        }

    }
}
