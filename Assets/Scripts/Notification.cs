using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    public static bool isInstanced = false;
    [SerializeField]
    private GameObject secondButton;
    //public Notifier.ButtonMethod method;
    // Start is called before the first frame update
    void Awake()
    {
        isInstanced = true;
    }

    public void SetupSecondButton(string message, Notifier.ButtonMethod method)
    {
        secondButton.GetComponentInChildren<TextMeshProUGUI>(true).text = message;
        Button button = secondButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => method());
        button.onClick.AddListener(Close);
        secondButton.SetActive(true);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        isInstanced = false;
        secondButton.SetActive(false);
    }
}
