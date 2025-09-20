using UnityEngine;
using System.Collections;

#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class IOSNotifications : MonoBehaviour
{
    [SerializeField] string categoryId = "default_category";
    [SerializeField] string threadId = "thread1";

#if UNITY_IOS
    public IEnumerator RequestAuthorization()
    {
        using AuthorizationRequest _request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
        while (!_request.IsFinished)
        {
            yield return null;
        }
    }

    public void SendNotification(string _title, string _body, string _subtitle, int _fireTimeSec)
    {
        iOSNotificationTimeIntervalTrigger _timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new System.TimeSpan(0, 0, _fireTimeSec),
            Repeats = false
        };

        iOSNotification _notf = new iOSNotification()
        {
            Identifier = "Test Notification",
            Title = _title,
            Subtitle = _subtitle,
            ShowInForeground = true,
            ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
            CategoryIdentifier = categoryId,
            ThreadIdentifier = threadId,
            Trigger = _timeTrigger
        };

        iOSNotificationCenter.ScheduleNotification(_notf);
    }
#else
    // Métodos vacíos para otras plataformas
    public IEnumerator RequestAuthorization()
    {
        yield break;
    }

    public void SendNotification(string _title, string _body, string _subtitle, int _fireTimeSec)
    {
        Debug.LogWarning("iOS Notifications are not supported on this platform.");
    }
#endif
}
