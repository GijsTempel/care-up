using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class MobileNotifications
{
    public string title;
    public string body;
    public string largeIconName;
}

/// <summary>
/// Manages notifications, accesses notification`s data and allows to schedule more.
/// </summary>
/// 
[System.Serializable]
public class CareUpNotification : MonoBehaviour
{
    // On iOS, this represents the notification's Category Identifier, and is optional
    // On Android, this represents the notification's channel, and is required (at least one).
    // Channels defined as global constants so can be referred to from GameController.cs script when setting/sending notification

    [SerializeField]
    private GameNotificationsManager manager;

    [SerializeField]
    [Tooltip("First notification after game update.")]
    private string deliverInHours;

    [SerializeField]
    [Tooltip("Time interval between next notifications ")]
    private string intervalInHours;

    private const int channelsCount = 7;

    [SerializeField]
    public MobileNotifications[] notifications = new MobileNotifications[channelsCount];

    // Update pending notifications in the next update.
    private bool updatePendingNotifications;

    private void Start()
    {
        if (notifications != null)
        {
            int channelsCount = 5;

            GameNotificationChannel[] channels = new GameNotificationChannel[channelsCount];

            // Set up channels (mostly for Android)
            for (int i = 0; i < channelsCount; i++)
            {
                channels[i] = new GameNotificationChannel($"channel{i}", "Default", "Reminder");
            }

            manager.Initialize(channels);
                       
            if (string.IsNullOrEmpty(deliverInHours))
            {
                Debug.LogError("Notification delivery time not set.");
            }

            DateTime deliveryTime = DateTime.Now.ToLocalTime();

            if (float.TryParse(deliverInHours, out float hours))
            {
                deliveryTime = DateTime.Now.ToLocalTime() + TimeSpan.FromSeconds(hours);
            }

            float interval = 0;
            float.TryParse(intervalInHours, out interval);

            int index;
            for (int i = 0; i < channelsCount; i++)
            {
                index = i <= notifications.Length ? i : notifications.Length - 1;

                SendNotification(notifications[index].title, notifications[index].body, deliveryTime, null, true,
                    $"channel{index}", "icon_0", notifications[index].largeIconName);

                deliveryTime += TimeSpan.FromSeconds(interval);
            }
        }
    }

    private void OnEnable()
    {
        if (manager != null)
        {
            manager.LocalNotificationDelivered += OnDelivered;
            manager.LocalNotificationExpired += OnExpired;
        }
    }

    private void OnDisable()
    {
        if (manager != null)
        {
            manager.LocalNotificationDelivered -= OnDelivered;
            manager.LocalNotificationExpired -= OnExpired;
        }
    }

    /// <summary>
    /// Queue a notification with the given parameters.
    /// </summary>
    /// <param name="title">The title for the notification.</param>
    /// <param name="body">The body text for the notification.</param>
    /// <param name="deliveryTime">The time to deliver the notification.</param>
    /// <param name="badgeNumber">The optional badge number to display on the application icon.</param>
    /// <param name="reschedule">
    /// Whether to reschedule the notification if foregrounding and the notification hasn't yet been shown.
    /// </param>
    /// <param name="id">Channel ID to use. If this is null/empty then it will use the default ID. For Android
    /// the channel must be registered in <see cref="GameNotificationsManager.Initialize"/>.</param>
    /// <param name="smallIcon">Notification small icon.</param>
    /// <param name="largeIcon">Notification large icon.</param>
    public void SendNotification(string title, string body, DateTime deliveryTime, int? badgeNumber = null,
                                 bool reschedule = true, string id = null,
                                 string smallIcon = null, string largeIcon = null)
    {
        IGameNotification notification = manager.CreateNotification();

        if (notification == null)
        {
            return;
        }

        notification.Title = title;
        notification.Body = body;
        notification.Group = !string.IsNullOrEmpty(id) ? id : "channel0";
        notification.DeliveryTime = deliveryTime;
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;
        if (badgeNumber != null)
        {
            notification.BadgeNumber = badgeNumber;
        }

        PendingNotification notificationToDisplay = manager.ScheduleNotification(notification);
        notificationToDisplay.Reschedule = reschedule;
        updatePendingNotifications = true;

        LogNotification(notificationToDisplay);
    }

    /// <summary>
    /// Cancel a given pending notification
    /// </summary>
    public void CancelPendingNotificationItem(PendingNotification itemToCancel)
    {
        manager.CancelNotification(itemToCancel.Notification.Id.Value);

        updatePendingNotifications = true;

        Debug.Log($"Cancelled notification with ID \"{itemToCancel.Notification.Id}\"");
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            StartCoroutine(UpdatePendingNotificationsNextFrame());
        }
    }

    private void OnDelivered(PendingNotification deliveredNotification)
    {
        // Schedule this to run on the next frame
        StartCoroutine(ShowDeliveryNotificationCoroutine(deliveredNotification.Notification));
    }

    private void OnExpired(PendingNotification obj)
    {
        Debug.Log($"Notification \"{obj.Notification.Id}\" with title \"{obj.Notification.Title}\" expired and was not displayed.");
    }

    private IEnumerator ShowDeliveryNotificationCoroutine(IGameNotification deliveredNotification)
    {
        yield return null;

        Debug.Log($"Notification with ID \"{deliveredNotification.Id}\" shown in foreground.");

        updatePendingNotifications = true;
    }

    private IEnumerator UpdatePendingNotificationsNextFrame()
    {
        yield return null;
        updatePendingNotifications = true;
    }

    private void Update()
    {
        if (updatePendingNotifications)
        {
            UpdatePendingNotifications();
        }
    }

    private void UpdatePendingNotifications()
    {
        updatePendingNotifications = false;

        // Recreate based on currently pending list
        // Note: Using ToArray because the list can change during the loop.
        if (manager.PendingNotifications != null)
        {
            foreach (PendingNotification scheduledNotification in manager.PendingNotifications.ToArray())
            {
                //Debug.Log("PendingNotification:");
                //LogNotification(scheduledNotification);
            }
        }
    }

    private void LogNotification(PendingNotification notification)
    {
        if (notification.Notification.Id.HasValue)
        {
            Debug.Log($"Notification {notification.Notification.Id.Value} title = {notification.Notification.Title}, delivery time = " +
                $"{notification.Notification.DeliveryTime.Value.ToString("yy-MM-dd HH:mm:ss")}");
        }
    }
}
