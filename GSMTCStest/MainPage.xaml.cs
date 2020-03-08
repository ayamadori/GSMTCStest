using System;
using System.Diagnostics;
using Windows.Media.Control;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace GSMTCStest
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // https://docs.microsoft.com/en-us/uwp/api/windows.media.control
        // Need <uap7:Capability Name = "globalMediaControl" /> in appxmanifest
        GlobalSystemMediaTransportControlsSessionManager manager;
        GlobalSystemMediaTransportControlsSession session;
        GlobalSystemMediaTransportControlsSessionMediaProperties properties;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            session = manager.GetCurrentSession();
            session.MediaPropertiesChanged += Session_MediaPropertiesChanged;
            session.PlaybackInfoChanged += Session_PlaybackInfoChanged;
        }

        private async void Session_PlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            var info = session.GetPlaybackInfo();
            string status = "[PlaybackInfoChanged] PlaybackType: " + info.PlaybackType + ", Status: " + info.PlaybackStatus;
            Debug.WriteLine(status);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Status.Text = status; });
        }

        private async void Session_MediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            properties = await session.TryGetMediaPropertiesAsync();
            string status = "[MediaPropertiesChanged] Artist: " + properties.Artist + ", Title: " + properties.Title;
            Debug.WriteLine(status);
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Status.Text = status; });
        }
    }
}
