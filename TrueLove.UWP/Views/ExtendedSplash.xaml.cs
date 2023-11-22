using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.Connectivity;
using TrueLove.Lib.Spider;
using Windows.Storage;
using System.Threading.Tasks;
using System.IO;
using TrueLove.Lib.Helpers;
using TrueLove.Lib.Models.Enum;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/p/?LinkID=234238

namespace TrueLove.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class ExtendedSplash : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();

                // Optional: Add a progress ring to your splash screen to show users that content is loading
                PositionRing();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();

            // Restore the saved session state if necessary
            //RestoreState(loadState);
        }

        void RestoreState(bool loadState)
        {
            if (loadState)
            {
                // TODO: write code to load state
            }
        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;

        }

        void PositionRing()
        {
            splashProgressRing.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashProgressRing.Width * 0.5));
            splashProgressRing.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                PositionRing();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Complete app setup operations here...
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) => CatchStream();

        /// <summary>
        /// Initialize stream data and save as local file
        /// </summary>
        async void CatchStream()
        {
            var path = ApplicationData.Current.LocalFolder.Path + @"/OfflineData.txt";
            do
            {
                await Task.Delay(0); // 使初始画面活动起来
                var isInternetAvailable = NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable;
                if (isInternetAvailable)
                {
                    splashProgressRing.IsActive = true;
                    await Task.Delay(5000);
                    //var localFolder = ApplicationData.Current.LocalFolder;
                    //var file = await localFolder.CreateFileAsync("OfflineData.txt",
                    //    CreationCollisionOption.ReplaceExisting);
                    //var _src = await new ReviewStream().GetStreamAsync($"https://avicii.com/page/11", false);
                    //await FileIO.AppendTextAsync(file, _src);
                }
            }
            while (!File.Exists(path));

            DismissExtendedSplash();
        }

        /// <summary>
        /// 沉淀状态栏 for PC
        /// </summary>
        private void CollapseTitleBar()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = Colors.White;

            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.Gray;

            titleBar.ButtonHoverBackgroundColor = Colors.PaleTurquoise;
            titleBar.ButtonHoverForegroundColor = Colors.Black;

            titleBar.ButtonPressedBackgroundColor = Colors.PaleTurquoise;
            titleBar.ButtonPressedForegroundColor = Colors.White;
        }

        /// <summary>
        /// 沉淀状态栏 for Phone
        /// </summary>
        private void CollapseStatusBar()
        {
            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            var s = StatusBar.GetForCurrentView();

            applicationView.VisibleBoundsChanged += (e, o) =>
            {

            };
        }

        void DismissExtendedSplash()
        {
            if (Generic.DeviceFamilyMatch(DeviceFamilyType.Desktop))
                CollapseTitleBar();
            //else if(Generic.DeviceFamilyMatch(DeviceFamilyType.Mobile))
            //    CollapseStatusBar();

            // Navigate to mainpage
            rootFrame.Navigate(typeof(MainPage));
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }
    }
}