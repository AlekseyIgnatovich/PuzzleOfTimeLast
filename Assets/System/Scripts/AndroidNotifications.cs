using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#endif

public class AndroidNotifications : MonoBehaviour {

    [SerializeField] string notificationChannel = "default_channel";
    [SerializeField] string notificationChannelName = "Default Channel";

#if UNITY_ANDROID
    public void RequestAuthorization() {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    public void RegisterNotificationChannel() {
        AndroidNotificationChannel _channel = new AndroidNotificationChannel {
            Id = notificationChannel,
            Name = notificationChannelName,
            Importance = Importance.Default,
            Description = "Test Notification"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(_channel);
    }

    public void SendNotification(string _title, string _text, float _fireTimeSec) {
        AndroidNotification _notf = new AndroidNotification();

        _notf.Title = _title;
        _notf.Text = _text;
        _notf.FireTime = System.DateTime.Now.AddSeconds(_fireTimeSec);
        _notf.SmallIcon = "icon_0";
        _notf.LargeIcon = "icon_1";

        AndroidNotificationCenter.SendNotification(_notf, notificationChannel);
    }
#endif
}
