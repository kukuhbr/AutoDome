using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Notifier : MonoBehaviour
{
    //public static Notifer notifier;
    private static GameObject notificationPrefab;
    private static GameObject instantNotificationPrefab;
    private Canvas mainCanvas;
    void Awake()
    {
        notificationPrefab = Resources.Load<GameObject>("Notification");
        instantNotificationPrefab = Resources.Load<GameObject>("InstantNotification");
        Debug.Log(notificationPrefab);
        Debug.Log(instantNotificationPrefab);
    }

    public static void Notify(string message, bool instant)
    {
        Transform mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Transform>();
        GameObject notification;
        if (instant) {
            notification = Instantiate(instantNotificationPrefab, mainCanvas);
        } else {
            notification = Instantiate(notificationPrefab, mainCanvas);
        }
        TextMeshProUGUI textMesh = notification.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = message;

    }
}
