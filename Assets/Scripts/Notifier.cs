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
    }

    public static void Notify(string message)
    {
        if (Notification.isInstanced) return;
        Transform mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Transform>();
        GameObject notification;
        notification = Instantiate(notificationPrefab, mainCanvas);
        TextMeshProUGUI textMesh = notification.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = message;
    }

    public static void Notify(string message, string message2)
    {
        if (Notification.isInstanced) return;
        Transform mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Transform>();
        GameObject notification;
        notification = Instantiate(notificationPrefab, mainCanvas);
        TextMeshProUGUI textMesh = notification.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = message;
        notification.GetComponent<Notification>().SetupSecondButton(message2);
    }

    public static void NotifyInstant(string message)
    {
        Transform mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Transform>();
        GameObject notification;
        notification = Instantiate(instantNotificationPrefab, mainCanvas);
        TextMeshProUGUI textMesh = notification.GetComponentInChildren<TextMeshProUGUI>();
        textMesh.text = message;
    }
}
