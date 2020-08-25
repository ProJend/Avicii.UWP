using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace True_Love.Helpers
{
    class MobileScreenTrigger : StateTriggerBase
    {
        public MobileScreenTrigger()
        {
            Window.Current.SizeChanged += (s, e) => UpdateTrigger();
        }

        /// <summary>
        /// The target device family.
        /// </summary>
        public UserInteractionMode InteractionMode
        {
            get => _interactionMode;
            set
            {
                _interactionMode = value;
                UpdateTrigger();
            }
        }

        private UserInteractionMode _interactionMode;

        private void UpdateTrigger()
        {
            // Get the current device family and interaction mode.
            var currentDeviceFamily = AnalyticsInfo.VersionInfo.DeviceFamily;
            var currentInteractionMode = UIViewSettings.GetForCurrentView().UserInteractionMode;

            // The trigger will be activated if the current device family is Windows.Mobile
            // and the UserInteractionMode matches the interaction mode value in XAML.
            SetActive(InteractionMode == currentInteractionMode && currentDeviceFamily == "Windows.Mobile");
        }
    }
}
